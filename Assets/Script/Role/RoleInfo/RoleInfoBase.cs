using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色信息基类
/// </summary>
public class RoleInfoBase 
{

    public int RoleId; //角色编号
    public string RoleNickName; //角色昵称

    public int Level; //等级

    public int Exp; //经验
    public int MaxHP; //最大HP
    public int MaxMP; //最大MP
    public int CurrHP; //当前HP
    public int CurrMP; //当前MP
    public int Attack; //攻击力
    public int Defense; //防御
    public int Hit; //命中
    public int Dodge; //闪避
    public int Cri; //暴击
    public int Res; //抗性
    public int Fighting; //综合战斗力
    public int LastInWorldMapId; //最后进入的世界地图编号
    public string LastInWorldMapPos; //最后进入的世界地图坐标

    /// <summary>
    /// 学会的技能信息
    /// </summary>
    public List<RoleSkillInfo> SkillInfosLst;
    /// <summary>
    /// 物理攻击
    /// </summary>
    public int[] PhySkillds;
    public RoleInfoBase()
    {
        SkillInfosLst = new List<RoleSkillInfo>();
    }
    /// <summary>
    /// 获取技能等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public int GetSkillLevel(int skillId)
    {
        if (SkillInfosLst == null) return 1;

        for (int i = 0; i < SkillInfosLst.Count; i++)
        {
            if (SkillInfosLst[i].SkillId==skillId)
            {
                return SkillInfosLst[i].SkillLevel;
            }
        }
        return 1;
    }

    /// <summary>
    /// 获取角色物理攻击
    /// </summary>
    /// <param name="phySkillIds"></param>
    public void SetPhySkillId(string phySkillIds)
    {
       string[] ids =phySkillIds.Split(';');
        PhySkillds = new int[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            PhySkillds[i] = ids[i].ToInt();
        }

    }


    /// <summary>
    /// 设置技能的冷却结束时间
    /// </summary>
    /// <param name="skillLd"></param>
    /// <param name="endTime"></param>
    public void SetSkillCDEndTime(int skillLd)
    {
        if (SkillInfosLst.Count > 0)
        {
            for (int i = 0; i < SkillInfosLst.Count; i++)
            {
                if (SkillInfosLst[i].SkillId == skillLd)
                {
                    SkillInfosLst[i].SkillCDEndTime = SkillInfosLst[i].SkillCDTime + Time.time;
                    break;
                }
            }
        }

    }

    /// <summary>
    /// 获取可以使用的技能ID
    /// </summary>
    /// <returns></returns>
    public int GetCanUsedSkillId()
    {
        if (SkillInfosLst.Count > 0)
        {
            for (int i = 0; i < SkillInfosLst.Count; i++)
            {
                if (Time.time > SkillInfosLst[i].SkillCDEndTime && CurrMP >= SkillInfosLst[i].SendMP)
                {

                    return SkillInfosLst[i].SkillId;
                }
            }
        }
        return 0;
    }
}