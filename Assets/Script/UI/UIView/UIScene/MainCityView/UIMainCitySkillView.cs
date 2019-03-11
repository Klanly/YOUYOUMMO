using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainCitySkillView : UISubViewBase
{
    public static UIMainCitySkillView Instance;
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        m_Dic = new Dictionary<int, UIMainCitySkillSlotsView>();
    }
    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill1;
    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill2;
    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill3;
    [SerializeField]
    private UIMainCitySkillSlotsView Btn_AddHP;

    public System.Action<int> OnSkillClick;

    private Dictionary<int, UIMainCitySkillSlotsView> m_Dic;

    public void SetUI(List<TransferData> lst, Action<int> onSkillClick)
    {
        for (int i = 0; i < lst.Count; i++)
        {
           int skillSlotNo = lst[i].GetValue<byte>(ConstDefine.SkillSlotsNo);
           int skillId = lst[i].GetValue<int>(ConstDefine.SkillId);
           int skillLevel = lst[i].GetValue<int>(ConstDefine.SkillLevel);
           string skillPic= lst[i].GetValue<string>(ConstDefine.SkillPic);
           float skillCDTime= lst[i].GetValue<float>(ConstDefine.SkillCDTime);
            switch (skillSlotNo)
            {
                case 1:
                    Btn_Skill1.SetUI(skillId, skillLevel,skillCDTime,skillPic, onSkillClick);
                    m_Dic[skillId] = Btn_Skill1;
                    break;                              
                case 2:                                 
                    Btn_Skill2.SetUI(skillId, skillLevel,skillCDTime, skillPic, onSkillClick);
                    m_Dic[skillId] = Btn_Skill2;
                    break;                               
                case 3:                                  
                    Btn_Skill3.SetUI(skillId, skillLevel, skillCDTime, skillPic, onSkillClick);
                    m_Dic[skillId] = Btn_Skill3;
                    break;
                default:
                    break;
            }
        }

    }
    /// <summary>
    /// ¿ªÊ¼ÀäÈ´
    /// </summary>
    /// <param name="skillId"></param>
    public void BeginCD(int skillId)
    {
        if (m_Dic.ContainsKey(skillId))
        {
            UIMainCitySkillSlotsView view = m_Dic[skillId];
            view.BeginCD();
        }
    }

}
