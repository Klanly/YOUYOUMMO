using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 怪AI
/// </summary>
public class RoleMonsterAI : IRoleAI
{
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRole
    {
        get;
        set;
    }

    /// <summary>
    /// 下次巡逻时间
    /// </summary>
    private float m_NextPatrolTime = 0f;

    /// <summary>
    /// 下次攻击时间
    /// </summary>
    private float m_NextAttackTime = 0f;

    /// <summary>
    /// 怪的信息
    /// </summary>
    private RoleInfoMonster roleInfoMonster;
    /// <summary>
    /// 要使用的技能ID
    /// </summary>
    private int usedSkillId = 0;
    /// <summary>
    /// 攻击类型
    /// </summary>
    private RoleAttackType roleAttackType;
    /// <summary>
    /// 要移动到的目标点
    /// </summary>
    private Vector3 m_MoverToPoint;

    private RaycastHit hitInfo;

    private Vector3 point;
    /// <summary>
    /// 下次思考时间
    /// </summary>
    private float m_NextThinkTime;
    /// <summary>
    /// 是否发呆中
    /// </summary>
    private bool m_IsDaze;

    public RoleMonsterAI(RoleCtrl roleCtrl, RoleInfoMonster info)
    {
        CurrRole = roleCtrl;
        roleInfoMonster = info;
    }

    public void DoAI()
    {
        //return;
      
        //如果当前玩家不存在
        if (GlobalInit.Instance==null||GlobalInit.Instance.CurrPlayer==null) return;

        if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die||CurrRole.isRigidty) return;

        if (CurrRole.LockEnemy == null)
        {
            //如果是待机状态
            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
            {
                if (Time.time > m_NextPatrolTime)
                {
                    m_NextPatrolTime = Time.time + UnityEngine.Random.Range(5f, 10f);
                    m_MoverToPoint = new Vector3(CurrRole.BornPoint.x + UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange), CurrRole.BornPoint.y, CurrRole.BornPoint.z + UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange));

                     point = new Vector3(m_MoverToPoint.x, m_MoverToPoint.y + 50, m_MoverToPoint.z);
                    if (Physics.Raycast(point, new Vector3(0, -200, 0), out hitInfo, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                    {
                        return;
                    }

                    //进行巡逻
                    CurrRole.MoveTo(m_MoverToPoint);
                }
            }

            //如果主角在怪的视野范围内
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = GlobalInit.Instance.CurrPlayer;
                //下次攻击时刻=当前时刻+延迟攻击时间
                m_NextAttackTime = Time.time + roleInfoMonster.SpriteEntity.DelaySec_Attack;
            }
        }
        else
        {
            //如果角色死亡
            if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            {
                CurrRole.LockEnemy = null;
                return;
            }

            if (Time.time>m_NextThinkTime+UnityEngine.Random.Range(3,3.5f))
            {
                //让角色休息
                CurrRole.ToIdle(RoleIdleState.IdleFight);
                m_NextThinkTime = Time.time;
                m_IsDaze = true;
            }
            //如果休息中
            if (m_IsDaze)
            {
                if (Time.time> m_NextThinkTime+ UnityEngine.Random.Range(1, 1.5f))
                {
                    m_IsDaze = false;
                }
                else
                {
                    //如果角色休息中直接返回
                    return;
                }
            }

           if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Idle) return;
 

            //如果有锁定敌人
            //1.如果我和锁定敌人的距离 超过了我的视野范围 则取消锁定
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) > CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = null;
                return;
            }
            //如果有锁定敌人
            //1.得到要使用的技能ID（包括物理攻击ID）
            if (roleInfoMonster.SpriteEntity.PhysicalAttackRate>=UnityEngine.Random.Range(0,100))
            {
                //物理攻击
                usedSkillId = roleInfoMonster.SpriteEntity.UsedPhyAttackArr[UnityEngine.Random.Range(0, roleInfoMonster.SpriteEntity.UsedPhyAttackArr.Length)];
                roleAttackType = RoleAttackType.PhyAttack;
            }
            else
            {
                //技能攻击
                usedSkillId = roleInfoMonster.SpriteEntity.UsedSkillListArr[UnityEngine.Random.Range(0, roleInfoMonster.SpriteEntity.UsedSkillListArr.Length)];
                roleAttackType = RoleAttackType.SkillAttack;
            }

            SkillEntity skillEntity = SkillDBModel.Instance.Get(usedSkillId);
            if (skillEntity == null) return;

            //2.判断敌人是否在此技能攻击范围内
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= CurrRole.AttackRange)
            {
                //让怪朝向主角
                CurrRole.transform.LookAt(new Vector3(CurrRole.LockEnemy.transform.position.x,CurrRole.transform.position.y,CurrRole.LockEnemy.transform.position.z));

                //如果当前时刻大于下次攻击时间 并且 当前角色不处于攻击状态
                if (Time.time > m_NextAttackTime && CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Attack)
                {
                    m_NextAttackTime = Time.time + UnityEngine.Random.Range(0f, 1f)+roleInfoMonster.SpriteEntity.Attack_Interval;
                    CurrRole.ToAttack(roleAttackType, usedSkillId);
                }
            }
            else
            {
                //3.如果在我的视野范围之内 攻击范围之外 进行追击
                if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
                {
                    //半圈内找
                    m_MoverToPoint = GameUtil.GetRandomPos(CurrRole.transform.position, CurrRole.LockEnemy.transform.position, skillEntity.AttackRange);
                    point = new Vector3(m_MoverToPoint.x, m_MoverToPoint.y + 50, m_MoverToPoint.z);
                    if (Physics.Raycast(point, new Vector3(0, -200, 0), out hitInfo, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                    {
                        return;
                    }
                    //移动到敌人周围 攻击半径内的随机点
                    CurrRole.MoveTo(m_MoverToPoint);
                }

            }
        }
    }
}