using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 下载管理器
/// </summary>
public class DownloadMgr:Singleton<DownloadMgr>
{
    /// <summary>
    /// 超时时间
    /// </summary>
    public const int DownLoadTimeOut = 5;
    public static string DownloadBasUrl = "";//这个地址，以后应该改成从服务器读取
    public const int DownloadRoutineNum = 5;//下载器的数量
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
    public static string DownloadUrl = DownloadBasUrl + "Windows/";
#elif UNITY_ANDROID
    public static string DownloadUrl = DownloadBasUrl + "Android/";
#elif UNITY_IPHONE
    public static string DownloadUrl = DownloadBasUrl + "IOS/";
#endif
    public string LocalFilePath = Application.persistentDataPath + "/";//本地资源路径
    
    private List<DownloadDataEntity> m_NeddDownloadDataList = new List<DownloadDataEntity>();//需要下载的数据列表

    private List<DownloadDataEntity> m_LocalDataList = new List<DownloadDataEntity>();//本地数据列表

    private List<DownloadDataEntity> m_ServerDataList = new List<DownloadDataEntity>();//服务器数据列表

    private string m_LoaclVersionPath;//本地版本文件路径

    private const string m_VersionFuleName = "VersionFule.txt";//版本文件名字

    private string m_StreamingAssetsPath;

    public Action OnInitComplete;

    /// <summary>
    ///1. 初始化资源
    /// </summary>
    public void InitStreamingAssetsPath(Action onInitComplete)
    {
        OnInitComplete = onInitComplete;
        m_LoaclVersionPath = LocalFilePath + m_VersionFuleName;
        //本地是否已经有资源
        if (File.Exists(m_LoaclVersionPath))
        {
            //检查更新
            InitCheckVersion();
        }
        else
        {
            //执行初始化 再检查更新
            m_StreamingAssetsPath = "file:///" + Application.streamingAssetsPath + "/AssetBundle/";
#if UNITY_ANDROID && !UNITY_EDITOR
              m_StreamingAssetsPath =Application.streamingAssetsPath + "/AssetBundles/";
#endif
            string versionFileUrl = m_StreamingAssetsPath + m_VersionFuleName;

            GlobalInit.Instance.StartCoroutine(ReadStreamingAssetVersionFile(versionFileUrl, OnReadStreamingAssetOver));
        }

    }
    /// <summary>
    /// 读取资源目录的版本文件
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="onReadStreamingAssetOver"></param>
    /// <returns></returns>
    private IEnumerator ReadStreamingAssetVersionFile(string fileUrl,Action<string> onReadStreamingAssetOver)
    {
        UISceneInitCtrl.Instance.SetProgress("正在准备进行资源初始化",0);
        using (WWW www = new WWW(fileUrl))
        {
            yield return www;
            if (www.error==null)
            {
                if (onReadStreamingAssetOver!=null)
                {
                    onReadStreamingAssetOver(Encoding.UTF8.GetString(www.bytes));
                }
               
            }
            else
            {
                onReadStreamingAssetOver("");
            }
        }
    }
    /// <summary>
    /// 读取完毕
    /// </summary>
    /// <param name="obj"></param>
    private void OnReadStreamingAssetOver(string obj)
    {
        GlobalInit.Instance.StartCoroutine(InitStreamingAssetList(obj));
    }
    /// <summary>
    /// 初始化资源清单
    /// </summary>
    /// <param name="conent"></param>
    /// <returns></returns>
    private IEnumerator InitStreamingAssetList(string conent)
    {
        if (string.IsNullOrEmpty(conent))
        {
            InitCheckVersion();
            yield break;
        }
        string[] arr = conent.Split('\n');
        //循环解压
        for (int i = 0; i < arr.Length; i++)
        {
            string[] arrInfo = arr[i].Split(' ');

            string fileUrl = arrInfo[0];//短路径
           
            yield return GlobalInit.Instance.StartCoroutine(AsserLoadToLocal(m_StreamingAssetsPath+fileUrl,LocalFilePath+fileUrl));

            float value = (i + 1) / (float)arr.Length;
            UISceneInitCtrl.Instance.SetProgress(string.Format("初始化资源不消耗流量 {0}/{1}",i+1,arr.Length),value);

          //  Debug.Log(string.Format("已经初始化资源{0}/{1}",i+1,arr.Length));
          
        }
        //解压版本文件
        yield return GlobalInit.Instance.StartCoroutine(AsserLoadToLocal(m_StreamingAssetsPath + m_VersionFuleName, LocalFilePath + m_VersionFuleName));
        InitCheckVersion();


    }
    /// <summary>
    /// 解压某个文件到本地
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="toPath"></param>
    /// <returns></returns>
    private IEnumerator AsserLoadToLocal(string fileUrl,string toPath)
    {
        using (WWW www=new WWW(fileUrl))
        {
            yield return www;
            if (www.error == null)
            {
                int lastIndexOf = toPath.LastIndexOf("\\");
                if (lastIndexOf != -1)
                {
                    string localPath = toPath.Substring(0, lastIndexOf);//出去文件名以外的路径

                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }

                }
                using (FileStream fs = File.Create(toPath, www.bytes.Length))
                {
                    fs.Write(www.bytes, 0, www.bytes.Length);
                    fs.Close();
                }
            }
        }
    }


    /// <summary>
    /// 2.检查版本文件
    /// </summary>
    public void InitCheckVersion()
    {
        UISceneInitCtrl.Instance.SetProgress("正在检查版本更新",0);

        string strVersionUrl = DownloadUrl + m_VersionFuleName;//版本文件路径

        AssetBundleDownload.Instance.InitServerVersion(strVersionUrl,OnInitVersionCallBack);
    }

    /// <summary>
    /// 初始化版本文件回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnInitVersionCallBack(List<DownloadDataEntity> serverDownloadData)
    {

        m_ServerDataList = serverDownloadData;
        if (File.Exists(m_LoaclVersionPath))
        {
            //与服务器对比
            //服务器数据
            Dictionary<string, string> serverDic = PackDownloadDataDic(serverDownloadData);
            //本地数据
            string content = IOUtil.GetFileText(m_LoaclVersionPath);
            Dictionary<string, string> clientDic = PackDownloadDataDic(content);
            m_LocalDataList = PackDownloadData(content);

            //1.新加的初始资源
            for (int i = 0; i < serverDownloadData.Count; i++)
            {
                if (serverDownloadData[i].IsFirstData && !clientDic.ContainsKey(serverDownloadData[i].FullName))
                {
                    m_NeddDownloadDataList.Add(serverDownloadData[i]);
                }

            }
            //2.对比已经下载过的 但是有更新的资源
            foreach (var item in clientDic)
            {
                //如果md5不一样
                if (serverDic.ContainsKey(item.Key) && serverDic[item.Key] != item.Value)
                {
                    //
                    DownloadDataEntity entity = GetDownloadData(item.Key, serverDownloadData);
                    if (entity != null)
                    {
                        m_NeddDownloadDataList.Add(entity);
                    }
                }
            }


        }
        else
        {
            //如果本地没有版本文件，初始资源需要下载的文件
            for (int i = 0; i < serverDownloadData.Count; i++)
            {
                if (serverDownloadData[i].IsFirstData)
                {
                    m_NeddDownloadDataList.Add(serverDownloadData[i]);
                }
            }
        }


            if (m_NeddDownloadDataList.Count==0)
            {
            UISceneInitCtrl.Instance.SetProgress("资源更新完毕",1);
                if (OnInitComplete!=null)
                {
                    OnInitComplete();
                }
                return;
            }

        //进行下载 拿到下载列表 
        AssetBundleDownload.Instance.DownloadFiles(m_NeddDownloadDataList);


    }
    /// <summary>
    /// 根据资源名称 获取资源实体
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="lst"></param>
    /// <returns></returns>
    private DownloadDataEntity GetDownloadData(string fullName,List<DownloadDataEntity>lst)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].FullName.Equals(fullName,StringComparison.CurrentCultureIgnoreCase))
            {
                return lst[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 根据文本内容 封装下载数据列表
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public List<DownloadDataEntity>PackDownloadData(string content)
    {
        List<DownloadDataEntity> lst = new List<DownloadDataEntity>();
        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; i++)
        {
            string[] arrData = arrLines[i].Split(' ');
            if (arrData.Length==4)
            {
                DownloadDataEntity entity = new DownloadDataEntity();
                entity.FullName = arrData[0];
                entity.MD5 = arrData[1];
                entity.Size = arrData[2].ToInt();
                entity.IsFirstData = arrData[3].ToInt() == 1;
                lst.Add(entity);
            }

        }

        return lst;
    }

    /// <summary>
    /// 封装字典
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    public Dictionary<string,string>PackDownloadDataDic(List<DownloadDataEntity>lst)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        for (int i = 0; i < lst.Count; i++)
        {
            dic[lst[i].FullName] = lst[i].MD5;
        }
        return dic;
    }
   /// <summary>
   /// 分装字典
   /// </summary>
   /// <param name="content"></param>
   /// <returns></returns>
    public Dictionary<string, string> PackDownloadDataDic(string content)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; i++)
        {
            string[] arrData = arrLines[i].Split(' ');
            if (arrData.Length == 4)
            {
                dic[arrData[0]] = arrData[1];
            }

        }

        return dic;
    }

    /// <summary>
    /// 修改本地文件
    /// </summary>
    /// <param name="entity"></param>
    public void ModifyLocalData(DownloadDataEntity entity)
    {
        if (m_LocalDataList == null) return;
        bool isExists = false;

        for (int i = 0; i < m_LocalDataList.Count; i++)
        {
            if (m_LocalDataList[i].FullName.Equals(entity.FullName))
            {
                m_LocalDataList[i].MD5 = entity.MD5;
                m_LocalDataList[i].Size = entity.Size;
                m_LocalDataList[i].IsFirstData = entity.IsFirstData;
                isExists = true;
                break;
            }

        }
        if (!isExists)
        {
            m_LocalDataList.Add(entity);
        }

        SaveLocalVersion();
    }

 
    /// <summary>
    /// 保存本地版本文件
    /// </summary>
    public void SaveLocalVersion()
    {
        StringBuilder sbContent = new StringBuilder();

        for (int i = 0; i < m_LocalDataList.Count; i++)
        {
            sbContent.AppendLine(string.Format("{0} {1} {2} {3}", m_LocalDataList[i].FullName, m_LocalDataList[i].MD5, m_LocalDataList[i].Size, m_LocalDataList[i].IsFirstData?1:0));
        }
        IOUtil.CreateTextFile(m_LoaclVersionPath,sbContent.ToString());
    }

    /// <summary>
    /// 根据路径获取服务器端数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public DownloadDataEntity GetServerData(string path)
    {
        if (m_ServerDataList == null) return null;
       // Debug.Log("path=" + path);
        for (int i = 0; i < m_ServerDataList.Count; i++)
            {
         //   Debug.Log("FullName=" + m_ServerDataList[i].FullName);
            
            if (path.Replace("/","\\").Equals(m_ServerDataList[i].FullName,StringComparison.CurrentCultureIgnoreCase))
                {
                Debug.Log("FullName=" + m_ServerDataList[i].FullName);
                return m_ServerDataList[i];
                }
            }
        return null;
        
    }

}
