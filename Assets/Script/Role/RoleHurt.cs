using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色受伤
/// </summary>
public class RoleHurt
{
    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr m_CurrRoleFSMMgr = null;
    /// <summary>
    /// 角色受伤委托
    /// </summary>
    public Action OnRoleHurt;
    public RoleHurt(RoleFSMMgr roleFSMMgr)
    {
        m_CurrRoleFSMMgr = roleFSMMgr;
    }

    public IEnumerator ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    {
        if (m_CurrRoleFSMMgr == null) yield break;
        //如果角色死亡
        if (m_CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) yield break;
        SkillEntity skillEntity = SkillDBModel.Instance.Get(roleTransferAttackInfo.SkillId);
        SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(roleTransferAttackInfo.SkillId, roleTransferAttackInfo.SkillLevel);
        if (skillEntity == null || skillLevelEntity == null) yield break;

        //延迟播放受伤动画
        yield return new WaitForSeconds(skillEntity.ShowHurtEffectDelaySecond);

        //减血
        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= roleTransferAttackInfo.HurtValue;

        //弹出受伤HUD
        int fontSize = 4;
        Color color = Color.red;
        if (roleTransferAttackInfo.IsCri)
        {
            fontSize = 8;
            color = Color.yellow;
        }

        UISceneCtrl.Instance.CurrentUIScene.HUDText.NewText("- " + roleTransferAttackInfo.HurtValue, m_CurrRoleFSMMgr.CurrRoleCtrl.transform, color, fontSize, 20f, -1f, 2.2f, bl_Guidance.RightDown);

        if (OnRoleHurt != null)
        {
            OnRoleHurt();
        }
        // DebugApp.Log("伤害"+roleTransferAttackInfo.HurtValue);
        //角色死亡时候，如果是pvp 是不直接通过死亡方法死亡的   而是要等服务器消息
        if (SceneMgr.Instance.CurrPlayerType==PlayType.PVP)
        {
            //角色死亡
            if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP <= 0)
            {
                m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 1;

            }
        }
        else
        {
            //角色死亡
            if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP <= 0)
            {
                m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 0;
                m_CurrRoleFSMMgr.CurrRoleCtrl.ToDie();
                yield break;
            }
        }
     


        //播放受伤特效
         EffectMgr.Instance.PlayEffect("Download/Prefab/Effect/Common/Effect_Hurt", "Effect_Hurt",
             (Transform obj) =>
             {

                 obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
                 obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
                 EffectMgr.Instance.DestroyEffect(obj, 2f);
             }


            );
        //屏幕泛红
        if (!m_CurrRoleFSMMgr.CurrRoleCtrl.isRigidty)
        {
            m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
        }
       
    }

}
