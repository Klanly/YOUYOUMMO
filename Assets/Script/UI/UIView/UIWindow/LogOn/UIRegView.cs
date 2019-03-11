using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegView : UIWindowViewBase
{
    public InputField txtAccount;

    public InputField txtPwd;

    protected override void OnStart()
    {
        base.OnStart();
        txtAccount = Global.FindChild(transform, "input_Account").GetComponent<InputField>();
        txtPwd = Global.FindChild(transform, "input_Pwd").GetComponent<InputField>();

    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnToLogOn":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIRegView_btnToLogOn);

                break;
            case "btnReg":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIRegView_btnReg);
                break;

        }


    }

}
