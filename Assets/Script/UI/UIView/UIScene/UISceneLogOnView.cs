using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLogOnView : UISceneViewBase
{

    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(OpenLogOnWindow());
    }

    private IEnumerator OpenLogOnWindow()
    {
        yield return new WaitForSeconds(.2f);

        AccountCtrl.Instance.QuickLogOn();



      //  UIViewMgr.Instance.OpenWindow(WindowUIType.LogOn);
        //AccountCtrl.Instance.OpenLogOnView();

    }
}
