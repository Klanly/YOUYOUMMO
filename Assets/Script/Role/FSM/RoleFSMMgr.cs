using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 角色有限状态机管理器
/// </summary>
public class RoleFSMMgr 
{
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRoleCtrl { get; private set; }

    /// <summary>
    /// 当前角色状态枚举
    /// </summary>
    public RoleState CurrRoleStateEnum { get; private set; }

    /// <summary>
    /// 当前角色状态
    /// </summary>
    private RoleStateAbstract m_CurrRoleState = null;

    private Dictionary<RoleState, RoleStateAbstract> m_RoleStateDic;
    /// <summary>
    /// 要切换的待机状态
    /// </summary>
    public RoleIdleState ToIdleState { get; set; }
    /// <summary>
    /// 当前待机状态
    /// </summary>
    public RoleIdleState CurrIdleState { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currRoleCtrl"></param>
    public RoleFSMMgr(RoleCtrl currRoleCtrl, Action OnDie, Action OnDestroy)
    {
        CurrRoleCtrl = currRoleCtrl;
        m_RoleStateDic = new Dictionary<RoleState, RoleStateAbstract>();
        m_RoleStateDic[RoleState.Idle] = new RoleStateIdle(this);
        m_RoleStateDic[RoleState.Run] = new RoleStateRun(this);
        m_RoleStateDic[RoleState.Attack] = new RoleStateAttack(this);
        m_RoleStateDic[RoleState.Hurt] = new RoleStateHurt(this);
        m_RoleStateDic[RoleState.Die] = new RoleStateDie(this);
        m_RoleStateDic[RoleState.Select] = new RoleStateSelect(this);

        RoleStateDie roleStateDie = (RoleStateDie)m_RoleStateDic[RoleState.Die];
        roleStateDie.OnDestroy = OnDestroy;
        roleStateDie.OnDie = OnDie;

        if (m_RoleStateDic.ContainsKey(CurrRoleStateEnum))
        {
            m_CurrRoleState = m_RoleStateDic[CurrRoleStateEnum];
        }
    }


    public RoleStateAbstract GetRoleState(RoleState state)
    {
        if (!m_RoleStateDic.ContainsKey(state)) return null;

        return m_RoleStateDic[state];



    }


    #region OnUpdate 每帧执行
    /// <summary>
    /// 每帧执行
    /// </summary>
    public void OnUpdate()
    {
        if (m_CurrRoleState != null)
        {
            m_CurrRoleState.OnUpdate();
        }
    }
    #endregion

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void ChangeState(RoleState newState)
    {
        if (CurrRoleStateEnum == newState&& CurrRoleStateEnum != RoleState.Idle && CurrRoleStateEnum != RoleState.Attack) return;

        //调用以前状态的离开方法
        if (m_CurrRoleState != null)
            m_CurrRoleState.OnLeave();

        //更改当前状态枚举
        CurrRoleStateEnum = newState;

        //更改当前状态
        m_CurrRoleState = m_RoleStateDic[newState];

        if (CurrRoleStateEnum== RoleState.Idle)
        {
            CurrIdleState = ToIdleState;
        }

        ///调用新状态的进入方法
        m_CurrRoleState.OnEnter();
    }
}