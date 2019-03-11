using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogOnView : UIWindowViewBase
{

    public InputField txtUserName;

    public InputField txtPwd;

    protected override void OnStart()
    {
        base.OnStart();
        txtUserName = Global.FindChild(transform, "input_Account").GetComponent<InputField>();
        txtPwd = Global.FindChild(transform, "input_Pwd").GetComponent<InputField>();

    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnLogOn":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnLogOn);

                break;
            case "btnToReg":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnToReg);
                break;

        }

    }


}
