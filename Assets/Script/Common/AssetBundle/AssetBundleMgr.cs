
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AssetBundleMgr : Singleton<AssetBundleMgr>
{
    private AssetBundleManifest m_Manifest; //�����ļ�����

    /// <summary>
    /// ���������ļ�����
    /// </summary>
    private void LoadManifestBundle()
    {
        if (m_Manifest != null)
        {
            return;
        }

        string assetName = string.Empty;
#if UNITY_STANDALONE_WIN
        assetName = "Windows";
#elif UNITY_ANDROID
        assetName = "Android";
#elif UNITY_IPHONE
        assetName = "iOS";
#endif

        using (AssetBundleLoader loader = new AssetBundleLoader(assetName))
        {
            m_Manifest = loader.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //AppDebug.Log("���������ļ����� ���");
    }

    /// <summary>
    /// ���ؾ���
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Load(string path, string name)
    {
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", path.Replace("assetbundle", "prefab")));
#else
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
#endif
    }

    /// <summary>
    /// ���м��ص�Asset��Դ����
    /// </summary>
    private Dictionary<string, Object> m_AssetDic = new Dictionary<string, Object>();

    /// <summary>
    /// ��������б�
    /// </summary>
    private Dictionary<string, AssetBundleLoader> m_AssetBundleDic = new Dictionary<string, AssetBundleLoader>();

    public void LoadOrDownloadForLua(string path, string name,XLuaCustomExport.OnCreate OnCreate)
    {
        LoadOrDownload<GameObject>(path, name,null,OnCreate: OnCreate, type:0);
    }


    public void LoadOrDownload<T>(string path, string name, System.Action<T> onComplete, XLuaCustomExport.OnCreate OnCreate=null, byte type = 0) where T : Object
    {
        lock (this)
        {
#if DISABLE_ASSETBUNDLE
            
              string newPath = string.Empty;
              switch (type)
              {
                  case 0:
                      newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "prefab"));
                      break;
                  case 1:
                      newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "png"));
                      break;
              }
              if (onComplete != null)
              {
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(newPath);
                onComplete(obj as T);
               }
               if (OnCreate!=null)
               {
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
                OnCreate(obj as GameObject);
               }
           
#else
            //1.���������ļ�����
            LoadManifestBundle();

            //2.���������ʼ
            string[] arrDps = m_Manifest.GetAllDependencies(path);
            //�ȼ������������ �Ƿ��Ѿ����� û���صľ�����
            CheckDps(0, arrDps, () =>
            {
                //=============��������Դ��ʼ===================
                string fullPath = (LocalFileMgr.Instance.LocalFilePath + path).ToLower();

                //AppDebug.Log("fullPath=" + fullPath);

                #region ���ػ��߼�������Դ
                if (!File.Exists(fullPath))
                {
                    #region ����ļ������� ��Ҫ����
                    DownloadDataEntity entity = DownloadMgr.Instance.GetServerData(path);
                    if (entity != null)
                    {
                        AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity,
                            (bool isSuccess) =>
                            {
                                if (isSuccess)
                                {

                                    if (m_AssetDic.ContainsKey(fullPath))
                                    {
                                        if (onComplete != null)
                                        {
                                            onComplete(m_AssetDic[fullPath] as T);
                                        }
                                        return;
                                    }

                                    for (int i = 0; i < arrDps.Length; i++)
                                    {
                                        if (!m_AssetDic.ContainsKey((LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()))
                                        {
                                            AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                                            Object obj = loader.LoadAsset(GameUtil.GetFileName(arrDps[i]));
                                            m_AssetBundleDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = loader;
                                            m_AssetDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = obj;
                                        }
                                    }

                                    //ֱ�Ӽ���
                                    using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, isFullPath: true))
                                    {
                                        if (onComplete != null)
                                        {
                                            Object obj = loader.LoadAsset<T>(name);
                                            m_AssetDic[fullPath] = obj;
                                            //���лص�
                                            onComplete(obj as T);
                                        }
                                    }
                                }
                            }));
                    }
                    #endregion
                }
                else
                {
                    if (m_AssetDic.ContainsKey(fullPath))
                    {
                        if (onComplete != null)
                        {
                            onComplete(m_AssetDic[fullPath] as T);
                        }
                        return;
                    }

                    //===================================
                    for (int i = 0; i < arrDps.Length; i++)
                    {
                        if (!m_AssetDic.ContainsKey((LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()))
                        {
                            AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                            Object obj = loader.LoadAsset(GameUtil.GetFileName(arrDps[i]));
                            m_AssetBundleDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = loader;
                            m_AssetDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = obj;
                        }
                    }
                    //===================================
                    //ֱ�Ӽ���
                    using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, isFullPath: true))
                    {
                        if (onComplete != null)
                        {
                            Object obj = loader.LoadAsset<T>(name);
                            m_AssetDic[fullPath] = obj;
                            //���лص�
                            onComplete(obj as T);
                        }
                    }
                }
                #endregion

                //=============��������Դ����===================
            });
#endif
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="arrDps"></param>
    /// <param name="onComplete"></param>
    private void CheckDps(int index, string[] arrDps, System.Action onComplete)
    {
        lock (this)
        {
            if (arrDps == null || arrDps.Length == 0)
            {
                if (onComplete != null) onComplete();
                return;
            }

            string fullPath = LocalFileMgr.Instance.LocalFilePath + arrDps[index];

            if (!File.Exists(fullPath))
            {
                //����ļ������� ��Ҫ����
                DownloadDataEntity entity = DownloadMgr.Instance.GetServerData(arrDps[index]);
                if (entity != null)
                {
                    AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity,
                        (bool isSuccess) =>
                        {
                            index++;
                            if (index == arrDps.Length)
                            {
                                if (onComplete != null) onComplete();
                                return;
                            }

                            CheckDps(index, arrDps, onComplete);
                        }));
                }
            }
            else
            {
                index++;
                if (index == arrDps.Length)
                {
                    if (onComplete != null) onComplete();
                    return;
                }

                CheckDps(index, arrDps, onComplete);
            }
        }
    }

    /// <summary>
    /// ���ػ���������Դ
    /// </summary>
    /// <param name="path">��·��</param>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    public void LoadOrDownload(string path, string name, System.Action<GameObject> onComplete)
    {
        LoadOrDownload<GameObject>(path, name, onComplete);
    }

    /// <summary>
    /// ж��������
    /// </summary>
    public void UnloadDpsAssetBundle()
    {
        foreach (var item in m_AssetBundleDic)
        {
            item.Value.Dispose();
        }
        m_AssetBundleDic.Clear();

        m_AssetDic.Clear();
    }

    /// <summary>
    /// ���ؿ�¡
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadClone(string path, string name)
    {
#if DISABLE_ASSETBUNDLE
        GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", path.Replace("assetbundle", "prefab")));
        return Object.Instantiate(obj);
#else
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            GameObject obj = loader.LoadAsset<GameObject>(name);
            return Object.Instantiate(obj);
        }
#endif
    }

    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public AssetBundleLoaderAsync LoadAsync(string path, string name)
    {
        GameObject obj = new GameObject("AssetBundleLoadAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatComponent<AssetBundleLoaderAsync>();
        async.Init(path, name);
        return async;
    }
}