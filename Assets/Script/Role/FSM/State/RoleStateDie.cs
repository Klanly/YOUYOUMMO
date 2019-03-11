using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 死亡状态
/// </summary>
public class RoleStateDie : RoleStateAbstract
{
    /// <summary>
    /// 角色死亡委托
    /// </summary>
    public Action OnDie;
    /// <summary>
    /// 角色销毁委托
    /// </summary>
    public Action OnDestroy;
    /// <summary>
    /// 是否已经销毁
    /// </summary>
    private bool isDestroy;

    private float m_BeginDieTime = 0f;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateDie(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        if (CurrRoleFSMMgr.CurrRoleCtrl.IsDied)
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDied.ToString(), true);
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), true);

            //播放受伤特效
            EffectMgr.Instance.PlayEffect("Download/Prefab/Effect/Common/Effect_PenXue", "Effect_PenXue",
                (Transform obj) =>
                {

                    obj.transform.position = CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
                    obj.transform.rotation = CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
                    EffectMgr.Instance.DestroyEffect(obj, 6f);

                    if (OnDie != null)
                    {
                        OnDie();
                    }
                    m_BeginDieTime = 0;
                }
                
                );
       
        }

    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (CurrRoleFSMMgr.CurrRoleCtrl.IsDied)
        {
            CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
            if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Died.ToString()))
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Died);
            }
        }
        else
        {
            m_BeginDieTime += Time.deltaTime;
            //确保只执行一次
            if (!isDestroy)
            {
                if (m_BeginDieTime >= 6)
                {
                    if (OnDestroy != null)
                    {
                        OnDestroy();
                        isDestroy = true;
                    }
                    return;
                }
            }


            CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
            if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Die.ToString()))
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Die);
            }
        }


     
      
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), false);
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDied.ToString(), false);
    }
}