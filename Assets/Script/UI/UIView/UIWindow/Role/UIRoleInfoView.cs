using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoleInfoView :UIWindowViewBase
{
    [SerializeField]
    private UIRoleEquipView roleEquipView;

    /// <summary>
    /// 角色详情视图
    /// </summary>
    [SerializeField]
    private UIRoleInfoDetailView m_UIRoleInfoDetailView;

    /// <summary>
    /// 
    /// </summary>
    public void SetRoleInfo(TransferData data)
    {
        roleEquipView.SetUI(data);
        m_UIRoleInfoDetailView.SetUI(data);
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
    }

    protected override void BeforeOnDestroy()
    {
        roleEquipView = null;
        m_UIRoleInfoDetailView = null;
    }


}
