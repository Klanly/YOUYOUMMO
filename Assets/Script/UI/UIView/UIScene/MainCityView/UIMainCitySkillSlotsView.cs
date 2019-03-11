using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCitySkillSlotsView : UISubViewBase
{
    public int SlotsNo;
    [HideInInspector]
    public int SkillId;
    [SerializeField]
    private Image SkillImg;
    /// <summary>
    /// CD img
    /// </summary>
    [SerializeField]
    private Image CDImg;
    /// <summary>
    /// 冷却时间
    /// </summary>
    private float m_SkillCDtime;
    /// <summary>
    /// 是否冷却中
    /// </summary>
    private bool m_IsCD = false;
    /// <summary>
    /// 开始冷却的时间
    /// </summary>
    private float m_BeginCDTime;
    /// <summary>
    /// 旋转的百分比
    /// </summary>
    private float m_CurrFillAmount = 0;

    private Action<int> OnSkillClick;
    protected override void OnAwake()
    {
        base.OnAwake();
        SkillImg.gameObject.SetActive(false);
        CDImg.gameObject.SetActive(false);
    }
    protected override void OnStart()
    {
        base.OnStart();
        EventTriggerListener.Get(gameObject).onClick += onClick;
      
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    /// <param name="go"></param>
    private void onClick(GameObject go)
    {
        //如果没有装上技能
        if (SkillId < 1) return;

        if (m_IsCD) return;


        if (OnSkillClick!=null)
        {
            OnSkillClick(SkillId);
        }

    }

    public void SetUI(int skillId, int skillLevel,float skillCDTime,string skillPic,Action<int> onSkillClick)
    {
        if (skillId == 0) return;

        SkillId = skillId;
        m_SkillCDtime = skillCDTime;
        OnSkillClick = onSkillClick;
        SkillImg.gameObject.SetActive(true);
        SkillImg.SetImage(RoleMgr.Instance.LoadSkillPic(skillPic));
    }
    /// <summary>
    /// 开始冷却
    /// </summary>
    public void BeginCD()
    {
        CDImg.gameObject.SetActive(true);
        m_IsCD = true;

        m_BeginCDTime = Time.time;

        m_CurrFillAmount = 1;

    }

    private void Update()
    {
        if (m_IsCD)
        {
            m_CurrFillAmount = Mathf.Lerp(1, 0, (Time.time - m_BeginCDTime )/ m_SkillCDtime);

            CDImg.fillAmount = m_CurrFillAmount;
            if (Time.time> m_BeginCDTime+ m_SkillCDtime)
            {
                m_IsCD = false;
            }
        }
    }

}
