using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SceneLoadingCtrl : MonoBehaviour 
{
    /// <summary>
    /// UI场景控制器
    /// </summary>
    [SerializeField]
    private UISceneLoadingCtrl m_UILoadingCtrl;

    private AsyncOperation m_Async = null;

    private AssetBundleCreateRequest request;

    private AssetBundle bundle;
    /// <summary>
    /// 当前的进度
    /// </summary>
    private int m_CurrProgress = 0;

	void Start ()
	{
        AssetBundleMgr.Instance.UnloadDpsAssetBundle();
        Resources.UnloadUnusedAssets();
        DelegateDefine.Instance.OnSenceLoadOk += OnScneLoadok;
        LayerUIMgr.Instance.Reset();
        StartCoroutine(LoadingScene());
        UIViewUtil.Instance.CloseAllWindows();
    }

    private void OnScneLoadok()
    {
        if (m_UILoadingCtrl!=null)
        {
            Destroy(m_UILoadingCtrl.gameObject);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        DelegateDefine.Instance.OnSenceLoadOk -= OnScneLoadok;
        if (bundle != null)
        {
            bundle.Unload(false);
        }
        request = null;
        bundle = null;
    }

    private IEnumerator LoadingScene()
    {
        string strSceneName = string.Empty;
        switch (SceneMgr.Instance.CurrentSceneType)
        {
            case SceneType.LogOn:
                strSceneName = "Scene_LogOn";
                break;
            case SceneType.SelectRole:
                strSceneName = "Scene_SelectRole";
                break;
            case SceneType.WorldMap:
                WorldMapEntity entity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId);
                if (entity!=null)
                {
                    strSceneName = entity.SceneName;
                }
                break;
            case SceneType.GameLevel:
                GameLevelEntity gameLevelEntity = GameLevelDBModel.Instance.Get(SceneMgr.Instance.CurrGameLevelId);
                if (gameLevelEntity != null)
                {
                    strSceneName = gameLevelEntity.SceneName;
                }               
                break;
        }

        if (string.IsNullOrEmpty(strSceneName))
        {
            yield break;
        }

        if (SceneMgr.Instance.CurrentSceneType== SceneType.SelectRole|| SceneMgr.Instance.CurrentSceneType == SceneType.WorldMap || SceneMgr.Instance.CurrentSceneType == SceneType.GameLevel)
        {
            StartCoroutine(Load(string.Format("Download/Scene/{0}.unity3d", strSceneName), strSceneName));
            //AssetBundleMgr.Instance.LoadAsync(string.Format("Scene/{0}.unity3d", strSceneName),strSceneName).OnLoadComplete=(UnityEngine.Object obj) => {
            //    m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
            //    m_Async.allowSceneActivation = false;
       // };
        }
        else
        {
            m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
            m_Async.allowSceneActivation = false;
            yield return m_Async;
        }

  
    }
    private IEnumerator Load(string path, string strSceneName)
    {
#if DISABLE_ASSETBUNDLE
        yield return null;
        path = path.Replace(".unity3d", "");
        m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = false;
#else
        string fullPath = LocalFileMgr.Instance.LocalFilePath + path;
        //AppDebug.Log("跳转场景=" + fullPath);
        if (!File.Exists(fullPath))
        {
            //AppDebug.Log("跳转场景不存在=" + fullPath);
            //如果路径不存在 就要进行下载
            DownloadDataEntity entity = DownloadMgr.Instance.GetServerData(path);
            //AppDebug.Log("跳转场景不存在path=" + path);
            if (entity != null)
            {
                StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity,
                    (bool isSuccess) =>
                    {
                        if (isSuccess)
                        {
                            StartCoroutine(LoadScene(fullPath, strSceneName));
                        }
                    }));
            }
        }
        else
        {
            //如果路径存在
            StartCoroutine(LoadScene(fullPath, strSceneName));
        }
        yield return null;
#endif
    }


    private IEnumerator LoadScene(string fullPath, string sceneName)
    {
        //AppDebug.Log("跳转场景=" + fullPath + "||" + sceneName);
        request = AssetBundle.LoadFromMemoryAsync(LocalFileMgr.Instance.GetBuffer(fullPath));
        yield return request;
        bundle = request.assetBundle;

        m_Async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = false;
    }

    void Update ()
	{
        if (m_Async == null) return;

        int toProgress = 0;

        if (m_Async.progress < 0.9f)
        {
            toProgress = Mathf.Clamp((int)m_Async.progress * 100, 1, 100);
        }
        else
        {
            toProgress = 100;
        }

        if (m_CurrProgress < toProgress)
        {
            m_CurrProgress++;
        }
        else
        {
            m_Async.allowSceneActivation = true;
        }

        m_UILoadingCtrl.SetProgressValue(m_CurrProgress * 0.01f);
    }
}