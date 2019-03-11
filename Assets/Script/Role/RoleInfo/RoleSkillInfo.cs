using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSkillInfo
{
    public int SkillId; //���ܱ��
    public int SkillLevel; //���ܵȼ�
    public byte SlotsNo; //���ܲ۱��

    private float skillCDTime=0f;//������ȴʱ��
    private int sendMP;//���ĵ�mp
    public float SkillCDEndTime;//��ȴ������ʱ��

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
