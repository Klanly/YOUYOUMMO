using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISceneSelectRoleView :UISceneViewBase {
    /// <summary>
    /// 拖拽视图
    /// </summary>
    public UISceneRoleDragView m_UISceneRoleDragView;
    /// <summary>
    /// 职业项
    /// </summary>
    public UISelectRoleJobItemView[] JobItem;
    /// <summary>
    /// 职业描述视图
    /// </summary>
    public UISelectRoleJobDescription uISelectRoleJobDescription;

    public UISelectRoleDeleteRoleView uISelectRoleDeleteRoleView;
    /// <summary>
    /// 名字输入
    /// </summary>
    public InputField txtNickName;

    public Action OnBtnBeginGameClick;
    /// <summary>
    /// 新建角色所需UI
    /// </summary>
    [SerializeField]
    private Transform[] m_UICreateRole;
    /// <summary>
    /// 选择角色UI
    /// </summary>
    [SerializeField]
    private Transform[] UISelectRole;
    /// <summary>
    /// 已有角色预设
    /// </summary>
    [SerializeField]
    private GameObject m_RoleItemPrefab;
    /// <summary>
    /// 已有角色列表容器
    /// </summary>
    [SerializeField]
    private Transform m_RoleListContainer;
    /// <summary>
    /// 角色头像
    /// </summary>
   [SerializeField]
    private Sprite[] m_RoleHeadPic;

    public Action OnBtnDeleteRoleClick;

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    public Action OnBtnReturnClick;
    /// <summary>
    /// 新建角色按钮点击
    /// </summary>
    public Action OnBtnCreateRoleClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_UISceneRoleDragView = Global.FindChild<UISceneRoleDragView>(transform, "SelectRoleDragView");
        txtNickName = Global.FindChild<InputField>(transform, "txtRoleName");
    }
    protected override void OnStart()
    {
        base.OnStart();

    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnRandomName":
                RandomName();
                break;
            case "btnBeginGame":
                if (OnBtnBeginGameClick!=null)
                {
                    OnBtnBeginGameClick();
                }
                break;
            case "btnDeleteRole":
                if (OnBtnDeleteRoleClick != null)
                {
                    OnBtnDeleteRoleClick();
                }
                break;
            case "btnReturn":
                if (OnBtnReturnClick != null) OnBtnReturnClick();
                break;
            case "btnCreateRole":
                if (OnBtnCreateRoleClick != null) OnBtnCreateRoleClick();
                break;
            default:
                break;
        }

    }

    public void RandomName()
    {
        txtNickName.text = GameUtil.RandomName();
    }
    /// <summary>
    /// 已有角色列表
    /// </summary>
    private List<UISelectRoleItemView> m_RoleItemViewList = new List<UISelectRoleItemView>();

    /// <summary>
    /// 设置新建角色所需UI
    /// </summary>
    /// <param name="isShow"></param>
    public void SetCreateRoleUI(bool isShow)
    {
        if (m_UICreateRole != null && m_UICreateRole.Length > 0)
        {
            for (int i = 0; i < m_UICreateRole.Length; i++)
            {
                m_UICreateRole[i].gameObject.SetActive(isShow);
            }
        }
    }


    #region SetUISelectRoleActive 设置选择角色UI 是否显示
    /// <summary>
    /// 设置选择角色UI 是否显示
    /// </summary>
    /// <param name="isActive"></param>
    public void SetUISelectRoleActive(bool isActive)
    {
        if (UISelectRole != null && UISelectRole.Length > 0)
        {
            for (int i = 0; i < UISelectRole.Length; i++)
            {
                UISelectRole[i].gameObject.SetActive(isActive);
            }
        }
    }
    #endregion

    /// <summary>
    /// 设置已有角色
    /// </summary>
    /// <param name="list"></param>
    public void SetRoleList(List<RoleOperation_LogOnGameServerReturnProto.RoleItem>list, Action<int> OnSelectRole)
    {
        ClearRoleListUI();


        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(m_RoleItemPrefab);
            UISelectRoleItemView view = obj.GetComponent<UISelectRoleItemView>();
            if (view!=null)
            {
                view.SetUI(list[i].RoleId, list[i].RoleNickName, list[i].RoleLevel, list[i].RoleJob, m_RoleHeadPic[list[i].RoleJob - 1], OnSelectRole);
                
                m_RoleItemViewList.Add(view);
            }
           
            obj.transform.parent = m_RoleListContainer;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0, -100 * i, 0);

        }
    }
    /// <summary>
    /// 设置选择的已有角色UI
    /// </summary>
    /// <param name="selectRoleId"></param>
    public void SetSelected(int selectRoleId)
    {
        if (m_RoleItemViewList != null && m_RoleItemViewList.Count > 0)
        {
            for (int i = 0; i < m_RoleItemViewList.Count; i++)
            {
                m_RoleItemViewList[i].SetSelected(selectRoleId);
            }
        }
    }

    /// <summary>
    /// 清楚已有角色UI
    /// </summary>
    public void ClearRoleListUI()
    {
        if (m_RoleItemViewList.Count > 0)
        {
            for (int i = 0; i < m_RoleItemViewList.Count; i++)
            {
                Destroy(m_RoleItemViewList[i].gameObject);
            }
            m_RoleItemViewList.Clear();
        }
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="nickName"></param>
    public void DeleteSelectRole(string nickName, Action onBtnOkClick)
    {
        uISelectRoleDeleteRoleView.Show(nickName,onBtnOkClick);

    }

    /// <summary>
    /// 关闭删除角色UI
    /// </summary>
    public void CloseDeleteRoleView()
    {
        uISelectRoleDeleteRoleView.Close();
    }

}
