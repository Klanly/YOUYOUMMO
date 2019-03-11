using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public class UIRoleEquipView : UISubViewBase
{

    /// <summary>
    /// 昵称
    /// </summary>

    public Text lblNickName;

    /// <summary>
    /// 角色等级
    /// </summary>
 
    public Text lblLevel;

    /// <summary>
    /// 综合战斗力
    /// </summary>

    public Text lblFighting;

    /// <summary>
    /// 职业编号
    /// </summary>
    public int m_JobId;
    /// <summary>
    /// 角色模型容器
    /// </summary>
    [SerializeField]
    private Transform RoleModelContainer;


    protected override void OnStart()
    {
        base.OnStart();
        CloneRoleModel();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SetUI(TransferData data)
    {
        m_JobId = data.GetValue<byte>(ConstDefine.JobId);
        lblNickName.text = data.GetValue<string>(ConstDefine.NickName);
        lblLevel.text = string.Format("Lv.{0}", data.GetValue<int>(ConstDefine.Level));
        lblFighting.text = string.Format("综合战斗力：<color='#ff0000'>{0}</color>", data.GetValue<int>(ConstDefine.Fighting));
    }

    /// <summary>
    /// 克隆角色模型
    /// </summary>
    public void CloneRoleModel()
    {
        GameObject obj = RoleMgr.Instance.LoadPlayer(m_JobId);
        obj.SetParent(RoleModelContainer);
        obj.SetLayer("UI");
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        RoleModelContainer = null;
        lblNickName = null;
        lblLevel = null;
        lblFighting = null;
    }

}
