using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectRoleItemView : MonoBehaviour
{
    /// <summary>
    /// ְҵID
    /// </summary>
    [SerializeField]
    private int m_RoleId;
    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    private Text m_LblNickName;
    /// <summary>
    /// �ȼ�
    /// </summary>
    [SerializeField]
    private Text m_LblLevel;
    /// <summary>
    /// ְҵ����
    /// </summary>
    [SerializeField]
    private Text m_LblJObName;
    /// <summary>
    /// ͷ��
    /// </summary>
    [SerializeField]
    private Image m_ImageRoleHead;
    /// <summary>
    /// ѡ�����н�ɫ
    /// </summary>
    private Action<int> OnSelectRole;
    private int m_SelectRoleId;
    private Vector3 m_MoveTargetPos;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonOnClick);
        m_MoveTargetPos = transform.localPosition + new Vector3(-50, 0, 0);
        transform.DOLocalMove(m_MoveTargetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause();

        SetSelected(m_SelectRoleId);
    }

    private void ButtonOnClick()
    {
        OnSelectRole(m_RoleId);
    }

    public void SetUI(int roleId,string nickName,int level,int jobId,Sprite headPic, Action<int> onSelectRole)
    {
        m_RoleId = roleId;
        m_LblNickName.text = nickName;
        m_LblLevel.text = string.Format("Lv{0}", level) ;
        m_LblJObName.text = JobDBModel.instance.Get(jobId).Name;
        m_ImageRoleHead.overrideSprite = headPic;
        if (onSelectRole!=null)
        {
            OnSelectRole = onSelectRole;
        }


    }
    /// <summary>
    /// ����ѡ��״̬
    /// </summary>
    /// <param name="selectRoleId"></param>
    public void SetSelected(int selectRoleId)
    {
        m_SelectRoleId = selectRoleId;
        if (m_RoleId == selectRoleId)
        {
            //ͻ����ʾ
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }

}
