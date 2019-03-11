using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 主角主城AI
/// </summary>
public class GameLevel_RoleMonsterAI : IRoleAI
{
    public RoleCtrl CurrRole
    {
        get;
        set;
    }

    /// <summary>
    /// 搜索到的敌人列表
    /// </summary>
    private List<Collider> m_SerchLst = null;

    /// <summary>
    /// 要移动到的目标点
    /// </summary>
    private Vector3 m_MoverToPoint;

    private RaycastHit hitInfo;

    private Vector3 point;

    public GameLevel_RoleMonsterAI(RoleCtrl roleCtrl)
    {
        CurrRole = roleCtrl;
        m_SerchLst = new List<Collider>();
    }
    /// <summary>
    /// 攻击索引
    /// </summary>
    private int phyIndex = 0;
    public void DoAI()
    {
        //执行AI
        if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) return;

        if (CurrRole.roleAttack.IsAutoFight)
        {
            AutoFightState();
        }
        else
        {
            NormalState();
        }
   

     
    }

    #region 自动战斗状态

    /// <summary>
    /// 自动战斗状态
    /// </summary>
    private void AutoFightState()
    {
        if (CurrRole.isRigidty)
        {
            return;
        }

        if (!GameLevelSceneCtrl.Instance.CurrRegionHasMonster)// 如果当前区域已经没有怪了 
        {
            //如果当前是最后一个区域直接返回
            if (GameLevelSceneCtrl.Instance.CurrRegionIsLast)
            {
                return;
            }
            //否则进入下一个区域
            else
            {
                CurrRole.MoveTo(GameLevelSceneCtrl.Instance.NetxRegionPlayerBornPos);
            }

        }
        else//找怪打怪
        {
            if (CurrRole.LockEnemy == null)//如果没有锁定敌人
            {
                //根据我的视野范围 搜索附近的怪
                //找离当前 攻击者最近的 的敌人
                Collider[] searchLst = Physics.OverlapSphere(CurrRole.transform.position, 1000, 1 << LayerMask.NameToLayer("Role"));
                m_SerchLst.Clear();

                if (searchLst != null && searchLst.Length > 0)
                {
                    for (int i = 0; i < searchLst.Length; i++)
                    {
                        if (searchLst[i].GetComponent<RoleCtrl>().CurrRoleType != RoleType.MainPlayer)
                        {
                            m_SerchLst.Add(searchLst[i]);
                        }
                    }

                }
                //对敌人排序，找到最近的
                m_SerchLst.Sort((Collider c1, Collider c2) =>
                {
                    int ret = 0;
                    if (Vector3.Distance(c1.transform.position, CurrRole.transform.position) < Vector3.Distance(c2.transform.position, CurrRole.transform.position))
                    {
                        ret = -1;
                    }
                    else
                    {
                        ret = 1;
                    }

                    return ret;
                });
                if (m_SerchLst.Count>0)
                {
                    if (m_SerchLst[0].GetComponent<RoleCtrl>().CurrRoleType != RoleType.MainPlayer)
                    {
                        CurrRole.LockEnemy = m_SerchLst[0].GetComponent<RoleCtrl>();
                    }
                }
               
                    //找最近的 当锁定敌人

                }
            else
            {
                //如果有锁定敌人
                //如果角色死亡
                if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
                {
                    CurrRole.LockEnemy = null;
                    return;
                }
                //定义要使用的技能id和技能类型
                //首先检测有没有可用的技能
                int skillId = CurrRole.CurrRoleInfo.GetCanUsedSkillId();
                RoleAttackType roleAttackType;
                //如果有可使用的技能
                if (skillId>0)
                {
                    //使用技能
                    //设置技能ID
                    roleAttackType = RoleAttackType.SkillAttack;
                }
                else
                {
                    //使用普通攻击
                    //设置物理攻击id
                    skillId = CurrRole.CurrRoleInfo.PhySkillds[phyIndex];
                    roleAttackType = RoleAttackType.PhyAttack;

                    phyIndex++;
                    if (phyIndex >= CurrRole.CurrRoleInfo.PhySkillds.Length)
                    {
                        phyIndex = 0;
                    }
                }

                SkillEntity skillEntity = SkillDBModel.Instance.Get(skillId);
                if (skillEntity == null) return;

                //2.判断敌人是否在此技能攻击范围内
                if (Vector3.Distance(CurrRole.transform.position, CurrRole.LockEnemy.transform.position) <= CurrRole.AttackRange)
                {
                    if (roleAttackType==RoleAttackType.SkillAttack)
                    {
                        PlayerCtrl.Instance.OnSkillClick(skillId);
                    }
                    else
                    {
                        //对敌人发起攻击
                        CurrRole.ToAttack(roleAttackType, skillId);
                    }
                    
                }
                else
                {
                    //在我的技能攻击范围外
                    //进行追击
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

    #endregion
    #region 普通状态

    /// <summary>
    /// 普通状态
    /// </summary>
    private void NormalState()
    {

        if (CurrRole.PreFightTime != 0)
        {
            //如果离上次战斗时间 超过30秒 切换普通待机
            if (Time.time > CurrRole.PreFightTime + 30)
            {
                CurrRole.ToIdle();
                CurrRole.PreFightTime = 0;
            }

        }
        //1.如果我有锁定敌人 就行攻击
        if (CurrRole.LockEnemy != null)
        {
            if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            {
                CurrRole.LockEnemy = null;
                return;
            }

            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
            {
                if (CurrRole.roleAttack.FollowSkillId > 0)
                {
                    //使用后续技能
                    PlayerCtrl.Instance.OnSkillClick(CurrRole.roleAttack.FollowSkillId);

                }
                else
                {
                    int skllId = CurrRole.CurrRoleInfo.PhySkillds[phyIndex];

                    CurrRole.ToAttack(RoleAttackType.PhyAttack, skllId);
                    phyIndex++;
                    if (phyIndex >= CurrRole.CurrRoleInfo.PhySkillds.Length)
                    {
                        phyIndex = 0;
                    }
                }
            }
        }
    }

    #endregion

}