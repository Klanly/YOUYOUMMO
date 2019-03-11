using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSkillInfo
{
    public int SkillId; //技能编号
    public int SkillLevel; //技能等级
    public byte SlotsNo; //技能槽编号

    private float skillCDTime=0f;//技能冷却时间
    private int sendMP;//消耗的mp
    public float SkillCDEndTime;//冷却结束的时间

    public float SkillCDTime
    {
        get
        {
            if (skillCDTime==0)
            {
                skillCDTime = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillId, SkillLevel).SkillCDTime;
            }
            return skillCDTime;
        }
    }

    public int SendMP
    {
        get
        {
            if (sendMP == 0)
            {
                sendMP = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillId, SkillLevel).SpendMP;
            }
            return sendMP;
        }
    }
}
