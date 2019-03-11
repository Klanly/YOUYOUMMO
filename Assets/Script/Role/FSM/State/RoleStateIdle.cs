using UnityEngine;
using System.Collections;

/// <summary>
/// 待机状态
/// </summary>
public class RoleStateIdle : RoleStateAbstract
{
    /// <summary>
    /// 下次切换时间
    /// </summary>
    private float nextChangeTime = 0;
    /// <summary>
    /// 切换间隔
    /// </summary>
    private float changeStep = 5;
    /// <summary>
    /// 是否休闲状态中
    /// </summary>
    private bool isXiuxian;
    /// <summary>
    /// 次状态运行时间
    /// </summary>
    private float m_RunnigTime;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateIdle(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer|| CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.OtherPlayer)
        {
            if (CurrRoleFSMMgr.CurrIdleState == RoleIdleState.IdleNormal)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), true);
                nextChangeTime = changeStep + Time.time;
                isXiuxian = false;
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), true);
            }
            m_RunnigTime = 0;
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), true);
        }
       
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer || CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.OtherPlayer)
        {


            if (!IsChangeOver)
            {
                if (CurrRoleFSMMgr.CurrIdleState == RoleIdleState.IdleNormal)
                {


                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
                    if (isXiuxian)
                    {
                        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.XiuXian.ToString()))
                        {

                            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.XiuXian);
                            m_RunnigTime += Time.deltaTime;
                            if (m_RunnigTime > 0.1f)
                            {
                                IsChangeOver = true;
                            }
                            else
                            {
                                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
                            }

                        }
                    }
                    else
                    {
                        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Normal.ToString()))
                        {
                            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Normal);
                            m_RunnigTime += Time.deltaTime;
                            if (m_RunnigTime > 0.1f)
                            {
                                IsChangeOver = true;
                            }
                        }
                        else
                        {
                            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
                        }

                    }
                }
                else
                {
                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);

                    if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Fight.ToString()))
                    {
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Fight);
                        m_RunnigTime += Time.deltaTime;
                        if (m_RunnigTime > 0.1f)
                        {
                            IsChangeOver = true;
                        }
                    }
                    else
                    {
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
                    }
                }
            }

            if (CurrRoleFSMMgr.CurrIdleState == RoleIdleState.IdleNormal)
            {

                if (Time.time > nextChangeTime)
                {
                    nextChangeTime = Time.time + changeStep;
                    isXiuxian = true;
                    IsChangeOver = false;
                    CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), false);
                    CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), true);

                }
                if (isXiuxian)
                {
                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
                    if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.XiuXian.ToString()) && CurrRoleAnimatorStateInfo.normalizedTime > 1)
                    {
                        isXiuxian = false;
                        IsChangeOver = false;
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), false);
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), true);


                    }
                }
            }
        }
        else
        {
            //如果是怪
            CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);

            if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Fight.ToString()))
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Fight);
                m_RunnigTime += Time.deltaTime;
                if (m_RunnigTime > 0.1f)
                {
                    IsChangeOver = true;
                }

              
            }
            else
            {
                //防止怪原地跑
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(),0);
            }
        }

    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer || CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.OtherPlayer)
        {
            if (CurrRoleFSMMgr.CurrIdleState == RoleIdleState.IdleNormal)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), false);
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), false);
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), false);
            }
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), false);
        }
    }
}