using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主角信息
/// </summary>
public class RoleInfoMainPlayer : RoleInfoBase
{
    public byte JobId; //职业编号
    public int Money; //元宝
    public int Gold; //金币
    public int TotalRechargeMoney; //元宝



    public RoleInfoMainPlayer():base()
    {

    }

    public RoleInfoMainPlayer(RoleOperation_SelectRoleInfoReturnProto roleInfoProto)
    {
        RoleId= roleInfoProto.RoldId; //角色编号
        RoleNickName = roleInfoProto.RoleNickName; //角色昵称
        JobId= roleInfoProto.JobId; //职业编号
        Level = roleInfoProto.Level; //等级
        Money = roleInfoProto.Money; //元宝
        TotalRechargeMoney = roleInfoProto.TotalRechargeMoney; //总充值金额
        Gold = roleInfoProto.Gold; //金币
        Exp = roleInfoProto.Exp; //经验
        MaxHP = roleInfoProto.MaxHP; //最大HP
        MaxMP = roleInfoProto.MaxMP; //最大MP
        CurrHP = roleInfoProto.CurrHP; //当前HP
        CurrMP = roleInfoProto.MaxMP; //当前MP
        Attack = roleInfoProto.Attack; //攻击力
        Defense = roleInfoProto.Defense; //防御
        Hit = roleInfoProto.Hit; //命中
        Dodge = roleInfoProto.Dodge; //闪避
        Cri = roleInfoProto.Cri; //暴击
        Res = roleInfoProto.Res; //抗性
        Fighting = roleInfoProto.Fighting; //综合战斗力
        LastInWorldMapId = roleInfoProto.LastInWorldMapId; //最后进入的世界地图编号
        LastInWorldMapPos = roleInfoProto.LastInWorldMapPos; //最后进入的世界地图坐标

      
   }
    /// <summary>
    /// 加载主角学会的技能
    /// </summary>
    /// <param name="proto"></param>
    public void LoadSkill(RoleData_SkillReturnProto proto)
    {
        //DebugApp.Log(roleSkillInfosLst.Count);
        SkillInfosLst.Clear();

        for (int i = 0; i < proto.CurrSkillDataList.Count; i++)
        {
            SkillInfosLst.Add(new RoleSkillInfo()
            {
                SkillId = proto.CurrSkillDataList[i].SkillId,
                SkillLevel = proto.CurrSkillDataList[i].SkillLevel,
                SlotsNo= proto.CurrSkillDataList[i].SlotsNo,            
                
            });
        }
    }

}