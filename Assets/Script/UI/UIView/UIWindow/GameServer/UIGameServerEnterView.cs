using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerEnterView :UIWindowViewBase
{
    /// <summary>
    /// 区服名字
    /// </summary>
    [SerializeField]
    private Text lblDefaultGameServer;
    /// <summary>
    /// 设置区服名字
    /// </summary>
    public void SetUI(string gameServerName)
    {
        lblDefaultGameServer.text = gameServerName;
    }

    protected override void OnStart()
    {
        base.OnStart();
       // lblDefaultGameServer = Global.FindChild(transform, "lblDefaultGameServer").GetComponent<Text>();
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnSelectGameServer":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGameServerEnterView_btnSelectServer);
                break;
            case "btnEnterGame":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGameServerEnterView_btnEnterGameServer);
                break;

        }

    }

}
