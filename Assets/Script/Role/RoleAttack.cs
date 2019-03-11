using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoleAttack
{
    /// <summary>
    /// ��ǰ��ɫ����״̬��������
    /// </summary>
    public RoleFSMMgr m_CurrRoleFSMMgr = null;
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public List<RoleAttackInfo> phyAttackInfoLst;
    /// <summary>
    /// ���ܹ�����Ϣ
    /// </summary>
    public List<RoleAttackInfo> skillAttackInfoLst;

    private RoleStateAttack m_RoleAttack;

    private RoleCtrl m_CurrRoleCtrl;
    /// <summary>
    /// �����б�
    /// </summary>
    private List<RoleCtrl> m_EnemyLst = null;
    /// <summary>
    /// �������ĵ����б�
    /// </summary>
    private List<Collider> m_SerchLst = null;
    /// <summary>
    /// �������ܱ��
    /// </summary>
    private int m_FollowSkillId;
    /// <summary>
    /// �Զ�ս��
    /// </summary>
    [HideInInspector]
    public bool IsAutoFight;
    /// <summary>
    /// ��Ч��·��
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
    /// ���������� ��ȡ������Ϣ
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
    /// ���ݼ���ID ��ȡ������Ϣ
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
        //����
        if (CameraCtrl.Instance != null && info.IsShake)
        {
            CameraCtrl.Instance.CameraShake(info.ShakeDelay);

        }

        if (m_RoleAttack == null)
        {
            //��ȡ����״̬
            //�����޸�״̬���еĲ���
            m_RoleAttack = m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack) as RoleStateAttack;
        }
        m_RoleAttack.m_AnimatorCondition = string.Format(roleAttackType == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
        m_RoleAttack.m_AnimatorConditionValue = index;
        m_RoleAttack.m_RoleAnimatorState = GameUtil.GetRoleAnimatorState(roleAttackType, index);
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
#endif
    }

    /// <summary>
    /// ���𹥻����Ƿ�ʹ�ü��ܳɹ���
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
        //1��ɫ���� ֻ�����Ǻ͹ֲŲ��뼼����ֵ����
        if (m_CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer || m_CurrRoleCtrl.CurrRoleType == RoleType.Monster)
        {
            //2 ��ȡ������Ϣ
            SkillEntity skillEntity = SkillDBModel.Instance.Get(skillId);
            if (skillEntity == null) return false;

            int skillLevel = m_CurrRoleCtrl.CurrRoleInfo.GetSkillLevel(skillId);
            SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(skillId, skillLevel);
            //3���������
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
            //4��������� �ҵ���
            if (m_CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
            {
                #region �ҵ���
                int attackTargetCount = skillEntity.AttackTargetCount;
                 m_EnemyLst.Clear();
                if (attackTargetCount == 1)
                {
                    #region ���� ����
                    //���幥�� ,�������������
                    if (m_CurrRoleCtrl.LockEnemy != null)
                    {
                        m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                    }
                    else
                    {
                        //���뵱ǰ ����������� �ĵ���
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
                        //�Ե��������ҵ������
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
                    #region Ⱥ��
                    //��Ҫ����������
                    int needAttact = attackTargetCount;
                    //���뵱ǰ ����������� �ĵ���
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
                    //�Ե��������ҵ������
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

                    //���������Ǳ��빥����
                    if (m_CurrRoleCtrl.LockEnemy != null)
                    {
                        if (m_CurrRoleCtrl.LockEnemy.CurrRoleType != RoleType.MainPlayer)
                        {
                            m_EnemyLst.Add(m_CurrRoleCtrl.LockEnemy);
                            //��Ҫ����-1
                            needAttact--;

                        }


                        //����������Ҫ�˺��ĵ���
                        for (int i = 0; i < m_SerchLst.Count; i++)
                        {
                            RoleCtrl ctrl = m_SerchLst[i].GetComponent<RoleCtrl>();
                            //�����ظ�����
                            if (ctrl.CurrRoleInfo.RoleId != m_CurrRoleCtrl.LockEnemy.CurrRoleInfo.RoleId && ctrl.CurrRoleType != RoleType.MainPlayer)
                            {
                                if ((i + 1) > needAttact) break;
                                //���б��м������
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
                            //����������Ҫ�˺��ĵ���
                            for (int i = 0; i < m_SerchLst.Count; i++)
                            {
                                RoleCtrl ctrl = m_SerchLst[i].GetComponent<RoleCtrl>();                              
                                        //if (ctrl.CurrRoleInfo.RoldId != m_CurrRoleCtrl.LockEnemy.CurrRoleInfo.RoldId)
                                        //{
                                            if ((i + 1) > needAttact) break;
                                            //���б��м������
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
            //�õ������б�

            //=====================PVP��PVE����=====================

            if (SceneMgr.Instance.CurrPlayerType==PlayType.PVE)
            {
               
                //5 �õ�������
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

            //�����PVE ���Ź�������  pvp ����ֻ������Ϣ�������� ���Ŷ��� �ɷ�����֪ͨ�ͻ���ʹ�ü��ܺ� �ٲ���
            if (SceneMgr.Instance.CurrPlayerType == PlayType.PVE)
            {
                PlayAttack(skillId);
            }


        }
        return true;
    }
    /// <summary>
    /// ���Ź�������
    /// </summary>
    /// <param name="skillId"></param>
    public void PlayAttack(int skillId)
    {
       
        RoleAttackType type = SkillDBModel.Instance.Get(skillId).IsPhyAttack == 1 ? RoleAttackType.PhyAttack : RoleAttackType.SkillAttack;

        #region ���� ��Ч���
        RoleAttackInfo info = GetRoleAttackInfo(type, skillId);
        // AssetBudnle�м���
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

                //����
                if (CameraCtrl.Instance != null && info.IsShake)
                {
                    CameraCtrl.Instance.CameraShake(info.ShakeDelay);

                }

                if (m_RoleAttack == null)
                {
                    //��ȡ����״̬
                    //�����޸�״̬���еĲ���
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


        //�����˺�
        //1������ֵ
        float attackValue = m_CurrRoleCtrl.CurrRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);
        //2�����˺�
        float baseHurt = attackValue * attackValue / (attackValue + enemy.CurrRoleInfo.Defense);
        //3��������
        float cri = 0.05f + (m_CurrRoleCtrl.CurrRoleInfo.Cri / (m_CurrRoleCtrl.CurrRoleInfo.Cri + enemy.CurrRoleInfo.Res))*0.1f;
        //��������
        cri = cri > 0.5f ? 0.5f : cri;
        //4�Ƿ񱩻�
        bool isCri = Random.Range(0f, 1f) <= cri;
        //5�����˺�����
        float criHurt = isCri ? 1.5f : 1;
        //6�����0.9f-1.1֮��
        float random = Random.Range(0.9f, 1.1f);
        //7�����˺�
        int hurtValue =Mathf.RoundToInt(baseHurt * criHurt * random);
        hurtValue = hurtValue < 1 ? 1 : hurtValue;

        roleTransferAttackInfo.HurtValue = hurtValue;
        roleTransferAttackInfo.IsCri = isCri;

        return roleTransferAttackInfo;



    }

}
