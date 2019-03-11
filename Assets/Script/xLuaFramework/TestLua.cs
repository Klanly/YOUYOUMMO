using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLua : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // LuaHelper.Instance.UISceneCtrl.LoadSceneUI("");
        //transform.Find("").GetComponent<UnityEngine.UI.Button>().onClick.AddListener();
        // LuaHelper.Instance.AssetBundleMgr.LoadOrDownloadForLua();
        //UnityEngine.Object.Instantiate();
        transform.transform.parent = null;
        transform.transform.localPosition = Vector3.zero;
        transform.transform.localScale = Vector3.one;


    }

    // Update is called once per frame
    void Update () {
		
	}
}
