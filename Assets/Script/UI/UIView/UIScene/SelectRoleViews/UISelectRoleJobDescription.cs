using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectRoleJobDescription : MonoBehaviour
{
    /// <summary>
    /// 职业名称
    /// </summary>
    [SerializeField]
    private Text lblJobName;
    /// <summary>
    /// 职业描述
    [SerializeField]
    private Text lblJobDesc;
    /// <summary>
    /// 移动的目标点
    /// </summary>
    private Vector3 m_MoveTagetPos;

    private bool isShow = false;

    void Start () {
        //lblJobName = Global.FindChild(transform, "lblJobName").GetComponent<Text>();

        //lblJobDesc = Global.FindChild(transform, "lblJobDesc").GetComponent<Text>();

        m_MoveTagetPos = transform.localPosition;

        Vector3 form = m_MoveTagetPos + new Vector3(0, 500, 0);

        transform.localPosition= form;

        transform.DOLocalMove(m_MoveTagetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause().OnComplete(()=>{
            isShow = true;
        }).OnRewind(()=>{
            transform.DOPlayForward();
        });
        DoAnim();
    }

    public void SetUI(string jobName,string jobDesc)
    {
        lblJobName.text = jobName;
        lblJobDesc.text = jobDesc;
        DoAnim();


    }
    private void DoAnim()
    {
        if (!isShow)
        {
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }
}



