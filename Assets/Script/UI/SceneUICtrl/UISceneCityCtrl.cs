using UnityEngine;
using System.Collections;

/// <summary>
/// 主城UI控制器
/// </summary>
public class UISceneCityCtrl : UISceneBase
{
    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "btnHead":
                OpenRoleInfo();
                break;
        }
    }

    private void OpenRoleInfo()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.RoleInfo.ToString(), (GameObject oj) =>
        {
            
        });
      //  UIViewUtil.Instance.OpenWindow(WindowUIType.RoleInfo);
    }
}