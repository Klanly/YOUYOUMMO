using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色传递的攻击信息
/// </summary>
public class RoleTransferAttackInfo
{
    /// <summary>
    /// 攻击者编号
    /// </summary>
    public int AttackRoleId;
    /// <summary>
    /// 攻击者的位置
    /// </summary>
    public Vector3 AttackRolePos;
    /// <summary>
    /// 被攻击者编号
    /// </summary>
    public int BeAttackRoleId;
    /// <summary>
    /// 伤害数值
    /// </summary>
    public int HurtValue;
    /// <summary>
    /// 攻击者使用的技能ID
    /// </summary>
    public int SkillId;
    /// <summary>
    /// 攻击者使用的技能等级
    /// </summary>
    public int SkillLevel;
    /// <summary>
    /// 是否附加异常状态
    /// </summary>
    public bool IsAbnoraml;
    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCri;

}
