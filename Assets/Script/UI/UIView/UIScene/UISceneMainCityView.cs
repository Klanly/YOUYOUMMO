using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneMainCityView : UISceneViewBase
{
    /// <summary>
    /// 自动战斗容器
    /// </summary>
    [SerializeField]
    private GameObject AutoFigthContainer;
    /// <summary>
    /// 自动战斗按钮
    /// </summary>
    [SerializeField]
    private GameObject BtnAutoFight;
    /// <summary>
    /// 取消自动战斗按钮
    /// </summary>
    [SerializeField]
    private GameObject BtnCancelAutoFight;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (OnloadComplete!=null)
        {
            OnloadComplete();
        }
        AutoFigthContainer.SetActive(SceneMgr.Instance.CurrentSceneType == SceneType.GameLevel);
    }
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnTopMenu":
                ChangeMenuState(go);
                break;
            case "btnMenu_Role":
                UIViewMgr.Instance.OpenWindow(WindowUIType.RoleInfo);
                break;
            case "btnMenu_GameLevel":
                UIViewMgr.Instance.OpenWindow(WindowUIType.GameLevelMap);
                break;
            case "btnAutoFight":
                AutoFight(true);
                break;
            case "btnCancelAutoFight":
                AutoFight(false);
                break;
            case "btnMenu_WorldMap":
                UIViewMgr.Instance.OpenWindow(WindowUIType.WorldMap);
                break;
            case "btnMenu_RechargeCtrl":
                LuaHelper.Instance.LoadLuaView("RechargeCtrl");
                break;

            default:
                break;
        }
    }
    
    /// <summary>
    /// 设置自动战斗
    /// </summary>
    /// <param name="isAutoFight"></param>
    public void AutoFight(bool isAutoFight)
    {
        BtnAutoFight.SetActive(!isAutoFight);
        BtnCancelAutoFight.SetActive(isAutoFight);

        //设置主角自动战斗状态
        GlobalInit.Instance.CurrPlayer.roleAttack.IsAutoFight = isAutoFight;

    }

    public void ChangeMenuState(GameObject go)
    {
        UIMainCityMenusView.Instance.ChangeState(() =>
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x,go.transform.localScale.y*-1,go.transform.localScale.z); 

        });
    }
}
