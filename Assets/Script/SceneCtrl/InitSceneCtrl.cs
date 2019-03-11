using UnityEngine;
using System.Collections;
using System;

public class InitSceneCtrl : MonoBehaviour 
{
	void Start ()
	{
        DelegateDefine.Instance.OnChannelInitOk = () =>
        {
            // StartCoroutine(LoadLogOn());
           
#if DISABLE_ASSETBUNDLE
            SceneMgr.Instance.LoadToLogOn();
#else
        DownloadMgr.DownloadBasUrl = GlobalInit.Instance.CurrChannelInitConfig.SourceUrl;
        DownloadMgr.Instance.InitStreamingAssetsPath(OnInitComplete);
#endif
        };


    }

    private void OnInitComplete()
    {
        StartCoroutine(LoadLogOn());
    }

    private IEnumerator LoadLogOn()
    {
        yield return new WaitForSeconds(0.5f);
        SceneMgr.Instance.LoadToLogOn();
    }
}