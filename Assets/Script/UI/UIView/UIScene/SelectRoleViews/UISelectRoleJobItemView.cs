using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

/// <summary>
/// ְҵ��
/// </summary>
public class UISelectRoleJobItemView : MonoBehaviour
{
    /// <summary>
    /// ְҵID
    /// </summary>
    [SerializeField]
    private int m_JobId;
    /// <summary>
    /// ��ת�Ƕ�
    /// </summary>
    [SerializeField]
    private int _RotateAngle;

    public delegate void OnSelectJobHandle(int jobId, int rotateAngle);

    public OnSelectJobHandle onSelectJobHandle;
    /// <summary>
    /// �ƶ�Ŀ��
    /// </summary>
    private Vector3 m_MoveTargetPos;
    private int m_SelectJobId;

    void Start ()
    {
        GetComponent<Button>().onClick.AddListener(ButtononClick);

        m_MoveTargetPos = transform.localPosition + new Vector3(50,0,0);
        transform.DOLocalMove(m_MoveTargetPos,0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause();
        SetSelected(m_SelectJobId);
    }

    public void SetSelected(int selectJobId)
    {
        m_SelectJobId = selectJobId;
        if (m_JobId== selectJobId)
        {
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }

    private void ButtononClick()
    {
        if (onSelectJobHandle!=null)
        {
            onSelectJobHandle(m_JobId, _RotateAngle);
        }
    }
}
