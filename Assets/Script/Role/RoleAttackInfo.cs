using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class RoleAttackInfo
{
    /// <summary>
    /// 索引号
    /// </summary>
    public int Index;
    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillId;
    /// <summary>
    /// 特效名称(角色在真正战斗场景 使用)
    /// </summary>
    public string EffectName;

#if DEBUG_ROLESTATE
    /// <summary>
    /// 特效预设(在测试环境使用)
    /// </summary>
    public GameObject EffectObj;

#endif
    /// <summary>
    /// 特效存活时间
    /// </summary>
    public float EffectLifeTime=0;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange = 0f;
    /// <summary>
    /// 受伤延迟时间
    /// </summary>
    public float HurtDelayTime = 0;
    /// <summary>
    /// 是否震屏
    /// </summary>
    public bool IsShake = false;
    /// <summary>
    /// 震屏延迟时间
    /// </summary>
    public float ShakeDelay=0;
    /// <summary>
    /// 开火声音
    /// </summary>
    public DelayAudioClip FireAudio;
    /// <summary>
    /// 攻击者的喊声
    /// </summary>
    public DelayAudioClip AttackRoleAudio;
}
