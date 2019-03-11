using UnityEngine;
using System.Collections;

/// <summary>
/// 攻击状态
/// </summary>
public class RoleStateAttack : RoleStateAbstract
{
    /// <summary>
    /// 条件
    /// </summary>
    public string m_AnimatorCondition;
    /// <summary>
    /// 上一个条件
    /// </summary>
    public string m_OldAnimatorCondition;
    /// <summary>
    /// 条件值
    /// </summary>
    public int m_AnimatorConditionValue;
    /// <summary>
    /// 当前角色动画状态
    /// </summary>
    public RoleAnimatorState m_RoleAnimatorState;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateAttack(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        CurrRoleFSMMgr.CurrRoleCtrl.PreFightTime = Time.time;
        CurrRoleFSMMgr.CurrRoleCtrl.isRigidty = true;
        m_OldAnimatorCondition = m_AnimatorCondition;
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(m_AnimatorCondition, m_AnimatorConditionValue);

        if (CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy != null)
        {
            CurrRoleFSMMgr.CurrRoleCtrl.transform.LookAt(new Vector3(CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy.transform.position.x, CurrRoleFSMMgr.CurrRoleCtrl.transform.position.y, CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy.transform.position.z));
        }
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();

        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(m_RoleAnimatorState.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)m_RoleAnimatorState);

            //如果动画执行了一遍 就切换待机
            if (CurrRoleAnimatorStateInfo.normalizedTime > 1)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.isRigidty = false;


                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle(RoleIdleState.IdleFight);
            }
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.isRigidty = false;
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(m_OldAnimatorCondition, 0);
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(),0);
      
    }
}