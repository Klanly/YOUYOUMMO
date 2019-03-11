using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISceneSelectRoleView :UISceneViewBase {
    /// <summary>
    /// ��ק��ͼ
    /// </summary>
    public UISceneRoleDragView m_UISceneRoleDragView;
    /// <summary>
    /// ְҵ��
    /// </summary>
    public UISelectRoleJobItemView[] JobItem;
    /// <summary>
    /// ְҵ������ͼ
    /// </summary>
    public UISelectRoleJobDescription uISelectRoleJobDescription;

    public UISelectRoleDeleteRoleView uISelectRoleDeleteRoleView;
    /// <summary>
    /// ��������
    /// </summary>
    public InputField txtNickName;

    public Action OnBtnBeginGameClick;
    /// <summary>
    /// �½���ɫ����UI
    /// </summary>
    [SerializeField]
    private Transform[] m_UICreateRole;
    /// <summary>
    /// ѡ���ɫUI
    /// </summary>
    [SerializeField]
    private Transform[] UISelectRole;
    /// <summary>
    /// ���н�ɫԤ��
    /// </summary>
    [SerializeField]
    private GameObject m_RoleItemPrefab;
    /// <summary>
    /// ���н�ɫ�б�����
    /// </summary>
    [SerializeField]
    private Transform m_RoleListContainer;
    /// <summary>
    /// ��ɫͷ��
    /// </summary>
   [SerializeField]
    private Sprite[] m_RoleHeadPic;

    public Action OnBtnDeleteRoleClick;

    /// <summary>
    /// ���ذ�ť���
    /// </summary>
    public Action OnBtnReturnClick;
    /// <summary>
    /// �½���ɫ��ť���
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
    /// ���н�ɫ�б�
    /// </summary>
    private List<UISelectRoleItemView> m_RoleItemViewList = new List<UISelectRoleItemView>();

    /// <summary>
    /// �����½���ɫ����UI
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


    #region SetUISelectRoleActive ����ѡ���ɫUI �Ƿ���ʾ
    /// <summary>
    /// ����ѡ���ɫUI �Ƿ���ʾ
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
    /// �������н�ɫ
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
    /// ����ѡ������н�ɫUI
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
    /// ������н�ɫUI
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
    /// ɾ����ɫ
    /// </summary>
    /// <param name="nickName"></param>
    public void DeleteSelectRole(string nickName, Action onBtnOkClick)
    {
        uISelectRoleDeleteRoleView.Show(nickName,onBtnOkClick);

    }

    /// <summary>
    /// �ر�ɾ����ɫUI
    /// </summary>
    public void CloseDeleteRoleView()
    {
        uISelectRoleDeleteRoleView.Close();
    }

}
