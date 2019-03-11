using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoleAttack
{
    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr m_CurrRoleFSMMgr = null;
    /// <summary>
    /// 物理攻击信息
    /// </summary>
    public List<RoleAttackInfo> phyAttackInfoLst;
    /// <summary>
    /// 技能攻击信息
    /// </summary>
    public List<RoleAttackInfo> skillAttackInfoLst;

    private RoleStateAttack m_RoleAttack;

    private RoleCtrl m_CurrRoleCtrl;
    /// <summary>
    /// 敌人列表
    /// </summary>
    private List<RoleCtrl> m_EnemyLst = null;
    /// <summary>
    /// 搜索到的敌人列表
    /// </summary>
    private List<Collider> m_SerchLst = null;
    /// <summary>
    /// 后续技能编号
    /// </summary>
    private int m_FollowSkillId;
    /// <summary>
    /// 自动战斗
    /// </summary>
    [HideInInspector]
    public bool IsAutoFight;
    /// <summary>
    /// 特效的路径
    /// </summary>
    public string EffectPath;
    public int FollowSkillId
    {
        get
        {
            return m_FollowSkillId;
        }
    }

    /// <summary>
    /// 根据索引号 获取攻击信息
    /// </summary>
    /// <param name="roleAttackType"></param>
    /// <param name="index"></param>
    private RoleAttackInfo GetRoleAttackInfoByIndex(RoleAttackType roleAttackType, int index)
    {
        if (roleAttackType == RoleAttackType.PhyAttack)
        {
            for (int i = 0; i < phyAttackInfoLst.Count; i++)
            {
                DebugApp.Log(phyAttackInfoLst[i].Index);
                if (phyAttackInfoLst[i].Index == index)
                {

                    return phyAttackInfoLst[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < skillAttackInfoLst.Count; i++)
            {
                if (skillAttackInfoLst[i].Index == index)
                {
                    DebugApp.Log(skillAttackInfoLst[i].Index);
                    return skillAttackInfoLst[i];
                }
            }
        }
        return null;

    }

    /// <summary>
    /// 根据技能ID 获取攻击信息
    /// </summary>
    /// <param name="roleAttackType"></param>
    /// <param name="skillId"></param>
    private RoleAttackInfo GetRoleAttackInfo(RoleAttackType roleAttackType, int skillId)
    {
        if (roleAttackType == RoleAttackType.PhyAttack)
        {
            for (int i = 0; i < phyAttackInfoLst.Count; i++)
            {
                //DebugApp.Log(phyAttackInfoLst[i].Index);
                if (phyAttackInfoLst[i].SkillId == skillId)
                {

                    return phyAttackInfoLst[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < skillAttackInfoLst.Count; i++)
            {
                if (skillAttackInfoLst[i].SkillId == skillId)
                {
                    //DebugApp.Log(skillAttackInfoLst[i].SkillId);
                    return skillAttackInfoLst[i];
                }
            }
        }
        return null;

    }

    public void SetFSM(RoleFSMMgr roleFSMMgr)
    {
        m_CurrRoleFSMMgr = roleFSMMgr;
        m_CurrRoleCtrl = roleFSMMgr.CurrRoleCtrl;
        m_EnemyLst = new List<RoleCtrl>();
        m_SerchLst = new List<Collider>();
    }

    public void ToAttackByIndex(RoleAttackType roleAttackType, int index)
    {
#if DEBUG_ROLESTATE
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.isRigidty) return;

        RoleAttackInfo info = GetRoleAttackInfo(roleAttackType, index);
        if (info != null)
        {
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackInfo = info;
            GameObject obj = Object.Instantiate(info.EffectObj);
            obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
            obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
            Object.Destroy(obj, info.EffectLifeTime);
        }
        //震屏
        if (CameraCtrl.Instance != null && info.IsShake)
        {
            CameraCtrl.Instance.CameraShake(info.ShakeDelay);

        }

        if (m_RoleAttack == null)
        {
            //获取攻击状态
            //用于修改状态机中的参数
            m_RoleAttack = m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack) as RoleStateAttack;
        }
        m_RoleAttack.m_AnimatorCondition = string.Format(roleAttackType == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
        m_RoleAttack.m_AnimatorConditionValue = index;
        m_RoleAttack.m_RoleAnimatorState = GameUtil.GetRoleAnimatorState(roleAttackType, index);
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
#endif
    }

    /// <summary>
    /// 发起攻击（是否使用技能成功）
    /// </summary>
    /// <param name="roleAttackType"></param>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool ToAttack(RoleAttackType roleAttackType, int skillId)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.isRigidty)
        {
            if (roleAttackType==RoleAttackType.SkillAttack)
            {
                m_FollowSkillId = skillId;
            }         
            return false;
        }
        m_FollowSkillId = -1;
        //1角色类型 只有主角和怪才参与技能数值计算
        if (m_CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer || m_CurrRoleCtrl.CurrRoleType == RoleType.Monster)
        {
            //2 获取技能信息
            SkillEntity skillEntity = SkillDBModel.Instance.Get(skillId);
            if (skillEntity == null) return false;

            int skillLevel = m_CurrRoleCtrl.CurrRoleInfo.GetSkillLevel(skillId);
            SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(skillId, skillLevel);
            //3如果是主角
            if (m_CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
            {
                if (skillLevelEntity.SpendMP > m_CurrRoleCtrl.CurrRoleInfo.CurrMP)
                {
                    return false;
                }
                else
                {
                    m_CurrRoleCtrl.CurrRoleInfo.CurrMP -= skillLevelEntity.SpendMP;
                    if (m_CurrRoleCtrl.CurrRoleInfo.CurrMP<=0)
                    {
                        m_CurrRoleCtrl.CurrRoleInfo.CurrMP = 0;
                    }
                    if (m_CurrRoleCtrl.OnMPChange!=null)
                    {
                        m_CurrRoleCtrl.OnMPChange(ValueChangeType.Subtrack);
                    }

                }
            }
            //4如果是主角 找敌人
            if (m_CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
            {
                #region 找敌人
                int attackTargetCount = skillEntity.AttackTargetCount;
                 m_EnemyLst.Clear();
                if (attackTargetCount == 1)
                {
                    #region 单体 攻击
                    //单体攻击 ,如果有锁定敌人
                    if (m_CurrRoleCtrl.LockEnemy != null)
                    {
                        m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                    }
                    else
                    {
                        //找离当前 攻击者最近的 的敌人
                        Collider[] searchLst = Physics.OverlapSphere(m_CurrRoleCtrl.transform.position, skillEntity.AreaAttackRadius, 1 << LayerMask.NameToLayer("Role"));
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
                            if (Vector3.Distance(c1.transform.position, m_CurrRoleCtrl.transform.position) < Vector3.Distance(c2.transform.position, m_CurrRoleCtrl.transform.position))
                            {
                                ret = -1;
                            }
                            else
                            {
                                ret = 1;
                            }

                            return ret;
                        });
                        
                            m_CurrRoleCtrl.LockEnemy = m_SerchLst[0].GetComponent<RoleCtrl>();
                            m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                        

                    }
                    #endregion

                }
                else
                {
                    #region 群攻
                    //需要攻击的数量
                    int needAttact = attackTargetCount;
                    //找离当前 攻击者最近的 的敌人
                    Collider[] searchLst = Physics.OverlapSphere(m_CurrRoleCtrl.transform.position, skillEntity.AreaAttackRadius, 1 << LayerMask.NameToLayer("Role"));
                    m_SerchLst.Clear();

                    if (searchLst != null && searchLst.Length > 0)
                    {
                        for (int i = 0; i < searchLst.Length; i++)
                        {
                            if (searchLst[i].GetComponent<RoleCtrl>().CurrRoleType!=RoleType.MainPlayer)
                            {
                                m_SerchLst.Add(searchLst[i]);
                            } 
                           
                        }

                    }
                    //对敌人排序，找到最近的
                    m_SerchLst.Sort((Collider c1, Collider c2) =>
                    {
                        int ret = 0;
                        if (Vector3.Distance(c1.transform.position, m_CurrRoleCtrl.transform.position) < Vector3.Distance(c2.transform.position, m_CurrRoleCtrl.transform.position))
                        {
                            ret = -1;
                        }
                        else
                        {
                            ret = 1;
                        }

                        return ret;
                    });

                    //锁定敌人是必须攻击的
                    if (m_CurrRoleCtrl.LockEnemy != null)
                    {
                        if (m_CurrRoleCtrl.LockEnemy.CurrRoleType != RoleType.MainPlayer)
                        {
                            m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                            //需要攻击-1
                            needAttact--;

                        }


                        //计算其他需要伤害的敌人
                        for (int i = 0; i < m_SerchLst.Count; i++)
                        {
                            RoleCtrl ctrl = m_SerchLst[i].GetComponent<RoleCtrl>();
                            //避免重复加入
                            if (ctrl.CurrRoleInfo.RoleId != m_CurrRoleCtrl.LockEnemy.CurrRoleInfo.RoleId && ctrl.CurrRoleType != RoleType.MainPlayer)
                            {
                                if ((i + 1) > needAttact) break;
                                //向列表中加入敌人
                                m_EnemyLst.Add(ctrl);
                            }

                        }
                    }
                    else
                    {
                        if (m_SerchLst.Count > 0)
                        {

                            if (m_SerchLst[0].GetComponent<RoleCtrl>().CurrRoleType != RoleType.MainPlayer)
                            {
                                m_CurrRoleCtrl.LockEnemy = m_SerchLst[0].GetComponent<RoleCtrl>();                              
                            }
                            //计算其他需要伤害的敌人
                            for (int i = 0; i < m_SerchLst.Count; i++)
                            {
                                RoleCtrl ctrl = m_SerchLst[i].GetComponent<RoleCtrl>();                              
                                        //if (ctrl.CurrRoleInfo.RoldId != m_CurrRoleCtrl.LockEnemy.CurrRoleInfo.RoldId)
                                        //{
                                            if ((i + 1) > needAttact) break;
                                            //向列表中加入敌人
                                            m_EnemyLst.Add(ctrl);
                                        //}                                                                   

                            }
                        }


                    }
                }

                #endregion
                #endregion
            }
            else if((m_CurrRoleCtrl.CurrRoleType == RoleType.Monster))
            {
                if (m_CurrRoleCtrl.LockEnemy)
                {
                    m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                }
               
            }
            //拿到敌人列表

            //=====================PVP和PVE区别=====================

            if (SceneMgr.Instance.CurrPlayerType==PlayType.PVE)
            {
               
                //5 让敌人受伤
                for (int i = 0; i < m_EnemyLst.Count; i++)
                {
                    RoleTransferAttackInfo roleTransferAttackInfo = CalculateHurtValue(m_EnemyLst[i], skillLevelEntity); ;

                    m_EnemyLst[i].ToHurt(roleTransferAttackInfo);
                }
            }
            else if(SceneMgr.Instance.CurrPlayerType == PlayType.PVP)
            {
                WorldMap_CurrRoleUseSkillProto proto = new WorldMap_CurrRoleUseSkillProto();

                proto.SkillId = skillId;
                proto.SkillLevel = skillLevel;
                proto.RolePosX = m_CurrRoleCtrl.transform.position.x;
                proto.RolePosY = m_CurrRoleCtrl.transform.position.y;
                proto.RolePosZ = m_CurrRoleCtrl.transform.position.z;
                proto.RoleYAngle = m_CurrRoleCtrl.transform.localEulerAngles.y;

                proto.BeAttackCount = m_EnemyLst.Count;
                proto.ItemList = new List<WorldMap_CurrRoleUseSkillProto.BeAttackItem>();

                for (int i = 0; i < m_EnemyLst.Count; i++)
                {
                    proto.ItemList.Add(new WorldMap_CurrRoleUseSkillProto.BeAttackItem() { BeAttackRoleId= m_EnemyLst[i].CurrRoleInfo.RoleId});
                }

                NetWorkSocket.Instance.SendMsg(proto.ToArray());
                m_CurrRoleFSMMgr.CurrRoleCtrl.isRigidty = true;
            }

            //============================================

            //如果是PVE 播放攻击动画  pvp 则是只发送消息给服务器 播放动画 由服务器通知客户端使用技能后 再播放
            if (SceneMgr.Instance.CurrPlayerType == PlayType.PVE)
            {
                PlayAttack(skillId);
            }


        }
        return true;
    }
    /// <summary>
    /// 播放攻击动画
    /// </summary>
    /// <param name="skillId"></param>
    public void PlayAttack(int skillId)
    {
       
        RoleAttackType type = SkillDBModel.Instance.Get(skillId).IsPhyAttack == 1 ? RoleAttackType.PhyAttack : RoleAttackType.SkillAttack;

        #region 动画 特效相关
        RoleAttackInfo info = GetRoleAttackInfo(type, skillId);
        // AssetBudnle中加载
        if (info == null) return ;

        m_CurrRoleFSMMgr.CurrRoleCtrl.PlayAudio(info.FireAudio.AudioClipName,info.FireAudio.DelayTime);
        m_CurrRoleFSMMgr.CurrRoleCtrl.PlayAudio(info.AttackRoleAudio.AudioClipName, info.AttackRoleAudio.DelayTime);

        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackInfo = info;
        EffectMgr.Instance.PlayEffect(EffectPath+info.EffectName, info.EffectName,
            (Transform obj)=>
            {
                obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
                obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
                EffectMgr.Instance.DestroyEffect(obj, info.EffectLifeTime);
                // Object.Destroy(obj, info.EffectLifeTime);

                //震屏
                if (CameraCtrl.Instance != null && info.IsShake)
                {
                    CameraCtrl.Instance.CameraShake(info.ShakeDelay);

                }

                if (m_RoleAttack == null)
                {
                    //获取攻击状态
                    //用于修改状态机中的参数
                    m_RoleAttack = m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack) as RoleStateAttack;
                }
                m_RoleAttack.m_AnimatorCondition = string.Format(type == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
                m_RoleAttack.m_AnimatorConditionValue = info.Index;
                m_RoleAttack.m_RoleAnimatorState = GameUtil.GetRoleAnimatorState(type, info.Index);
                m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
            }
            
            );
       
        #endregion
    }

    private RoleTransferAttackInfo CalculateHurtValue(RoleCtrl enemy, SkillLevelEntity skillLevelEntity)
    {
        if (enemy == null || skillLevelEntity == null) return null;

        SkillEntity skillEntity = SkillDBModel.Instance.Get(skillLevelEntity.SkillId);
        if (skillEntity == null) return null;

        RoleTransferAttackInfo roleTransferAttackInfo = new RoleTransferAttackInfo();
        roleTransferAttackInfo.AttackRoleId = m_CurrRoleCtrl.CurrRoleInfo.RoleId;
        roleTransferAttackInfo.AttackRolePos = m_CurrRoleCtrl.transform.position;
        roleTransferAttackInfo.BeAttackRoleId = enemy.CurrRoleInfo.RoleId;
        roleTransferAttackInfo.SkillId = skillEntity.Id;
        roleTransferAttackInfo.SkillLevel = skillLevelEntity.Level;
        roleTransferAttackInfo.IsAbnoraml = skillEntity.AbnormalState == 1;


        //计算伤害
        //1攻击数值
        float attackValue = m_CurrRoleCtrl.CurrRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);
        //2基础伤害
        float baseHurt = attackValue * attackValue / (attackValue + enemy.CurrRoleInfo.Defense);
        //3暴击概率
        float cri = 0.05f + (m_CurrRoleCtrl.CurrRoleInfo.Cri / (m_CurrRoleCtrl.CurrRoleInfo.Cri + enemy.CurrRoleInfo.Res))*0.1f;
        //暴击概率
        cri = cri > 0.5f ? 0.5f : cri;
        //4是否暴击
        bool isCri = Random.Range(0f, 1f) <= cri;
        //5暴击伤害倍率
        float criHurt = isCri ? 1.5f : 1;
        //6随机数0.9f-1.1之间
        float random = Random.Range(0.9f, 1.1f);
        //7最终伤害
        int hurtValue =Mathf.RoundToInt(baseHurt * criHurt * random);
        hurtValue = hurtValue < 1 ? 1 : hurtValue;

        roleTransferAttackInfo.HurtValue = hurtValue;
        roleTransferAttackInfo.IsCri = isCri;

        return roleTransferAttackInfo;



    }

}
