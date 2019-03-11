using UnityEngine;
using System.Collections;

public class LogOnSceneCtrl : MonoBehaviour {



    void Awake()
    {
        UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.LogOn,(GameObject obj)=>
        {

        });
        AudioBackGroundMgr.Instance.Play("Audio_Bg_LogOn");

    }


	void Start () {
        if (DelegateDefine.Instance.OnSenceLoadOk != null)
        {
            DelegateDefine.Instance.OnSenceLoadOk();
        }

    }
	

}