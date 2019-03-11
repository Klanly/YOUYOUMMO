using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 主下载器 检查版本  分配下载器任务
/// </summary>
public class AssetBundleDownload : SingletonMono<AssetBundleDownload>
{
    private string m_VersionUrl;
    private Action<List<DownloadDataEntity>> m_OnInityVersion;
    /// <summary>
    /// 下载器的数组
    /// </summary>
    private AssetBundleDownloadRoutine[] m_Routines=new AssetBundleDownloadRoutine[DownloadMgr.DownloadRoutineNum];
    private int m_RoutineIndex = 0;//下载器索引

    private bool m_IsDownloadover;
    protected override void OnStart()
    {
        base.OnStart();
        //真正的运行
        StartCoroutine(DownLoadVersion(m_VersionUrl));
    }

    private float m_Time = 2;//采样时间
    private float m_AleadyTime = 0;//已经下载的时间

    private float m_NeedTime=0;//剩余时间
    private float m_Speed = 0;//下载速度


    protected override void OnUpdate()
    {
        base.OnUpdate();
        //如果需要下载的数量大于0 并且没有下载完成
        if (TotalCount>0&&!m_IsDownloadover)
        {
            int totalCompleteCount = CurrCompeletTotalCount();
            totalCompleteCount = totalCompleteCount == 0 ? 1 : totalCompleteCount;

            int totalCompleteSize = CurrCompeletTotalSize();

            m_AleadyTime += Time.deltaTime;
            if (m_AleadyTime> m_Time&& m_Speed==0)
            {
                m_Speed = totalCompleteSize / m_Time;
            }
            if (m_Speed>0)
            {
                //剩余时间=（总大小-已经下载的大小）/速度
                m_NeedTime = (ToalSize - totalCompleteSize) / m_Speed;
            }

            string str = string.Format("资源正在下载{0}/{1}", totalCompleteCount,TotalCount);
           // string strProgress = string.Format("下载进度={0}",totalCompleteSize/(float)ToalSize);

            UISceneInitCtrl.Instance.SetProgress(str,totalCompleteCount/(float)TotalCount);

            //DebugApp.Log(str);
            //DebugApp.Log(strProgress);

            if (m_NeedTime > 0)
            {
                string strNeedTime = string.Format("剩余{0}秒", m_NeedTime);
            }


            if (totalCompleteCount==TotalCount)
            {
                m_IsDownloadover = true;
                ///DebugApp.Log("资源更新完毕");
                UISceneInitCtrl.Instance.SetProgress("资源更新完毕", 1);
                if (DownloadMgr.Instance.OnInitComplete!=null)
                {
                    DownloadMgr.Instance.OnInitComplete();
                }
            }



        }
    }
    /// <summary>
    /// 初始化服务器的版本信息
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onInityVersion"></param>
    public void InitServerVersion(string url,Action<List<DownloadDataEntity>>onInityVersion)
    {
        m_VersionUrl = url;
        m_OnInityVersion = onInityVersion;
    }

    private IEnumerator DownLoadVersion(string url)
    {
        WWW www = new WWW(url);

        float timeOut = Time.time;
        float progress = www.progress;
        while (www!=null&&www.isDone)
        {
            if (progress<www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
            }
            if ((Time.time-timeOut)>DownloadMgr.DownLoadTimeOut)
            {
                DebugApp.Log("下载超时");
                yield break;
            }
        }
        yield return www;

        if (www!=null&&www.error==null)
        {
            string conten = www.text;
            if (m_OnInityVersion!=null)
            {
                m_OnInityVersion(DownloadMgr.Instance.PackDownloadData(conten));
            }
        }
        else
        {
            DebugApp.Log("下载失败 原因："+www.error);
        }
    }
    /// <summary>
    /// 总大小
    /// </summary>
    public int ToalSize
    {
        get;

        private set;
        
    }
    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalCount
    {
        get;

        private set;

    }
    /// <summary>
    /// 当前已经下载的文件总大小
    /// </summary>
    /// <returns></returns>
    public int CurrCompeletTotalSize()
    {
        int compeleteTotalSize = 0;

        for (int i = 0; i < m_Routines.Length; i++)
        {
            if (m_Routines[i] == null) continue;
            compeleteTotalSize += m_Routines[i].DownloadSize;
        }

        return compeleteTotalSize;
    }

    /// <summary>
    /// 当前已经下载的文件总数量
    /// </summary>
    /// <returns></returns>
    public int CurrCompeletTotalCount()
    {
        int compeleteTotalCount = 0;

        for (int i = 0; i < m_Routines.Length; i++)
        {
            if (m_Routines[i] == null) continue;
            compeleteTotalCount += m_Routines[i].CompleteCount;
        }

        return compeleteTotalCount;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="downloadLst"></param>
    public void DownloadFiles(List<DownloadDataEntity> downloadLst)
    {
        ToalSize = 0;
        TotalCount = 0;
        //初始化下载器
        for (int i = 0; i < m_Routines.Length; i++)
        {
            if (m_Routines[i]==null)
            {
                m_Routines[i] = gameObject.AddComponent<AssetBundleDownloadRoutine>();
            }
        }

        for (int i = 0; i < downloadLst.Count; i++)
        {
            m_RoutineIndex = m_RoutineIndex % m_Routines.Length;//0-4

            //其中一个下载器 分配一个文件
            m_Routines[m_RoutineIndex].AddDownload(downloadLst[i]);

            m_RoutineIndex++;
            ToalSize += downloadLst[i].Size;
            TotalCount++;

        }
        //让下载器开始下载

        for (int i = 0; i < m_Routines.Length; i++)
        {
            if (m_Routines[i] == null) continue;
            m_Routines[i].StartDownload();
        }

    }

    public IEnumerator DownloadData(DownloadDataEntity currDownLoadData,Action<bool> onComplete)
    {
        string dataUrl = DownloadMgr.DownloadUrl + currDownLoadData.FullName;//资源下载路径
        //短路径 用于创建文件夹
        string path = currDownLoadData.FullName.Substring(0, currDownLoadData.FullName.LastIndexOf('\\'));

        //得到本地路径
        string localFilePath = DownloadMgr.Instance.LocalFilePath + path;

        if (!Directory.Exists(localFilePath))
        {
            Directory.CreateDirectory(localFilePath);
        }

        WWW www = new WWW(dataUrl);

        float timeOut = Time.time;
        float progress = www.progress;

        while (www != null && www.isDone)
        {
            if (progress < www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
            }

            if (Time.time - timeOut > DownloadMgr.DownLoadTimeOut)
            {
                DebugApp.LogError("下载超时");
                onComplete(false);
                yield break;
            }

            yield return null; //等一帧  会卡死

        }

        yield return www;

        if (www != null && www.error == null)
        {
            using (FileStream fs = new FileStream(DownloadMgr.Instance.LocalFilePath + currDownLoadData.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }
        }

        //写入本地文件
        DownloadMgr.Instance.ModifyLocalData(currDownLoadData);

        if (onComplete!=null)
        {
            onComplete(true);
        }
    }

}
