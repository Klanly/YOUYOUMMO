using UnityEngine;
using System.Collections;
using Pathfinding;
using System;
using System.Collections.Generic;

/// <summary>
/// 角色控制器
/// </summary>
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(FunnelModifier))]
[Serializable]
public class RoleCtrl : MonoBehaviour
{
    #region 成员变量或属性
    /// <summary>
    /// 昵称挂点
    /// </summary>
    [SerializeField]
    private Transform m_HeadBarPos;

    /// <summary>
    /// 头顶UI条
    /// </summary>
    private GameObject m_HeadBar;

    /// <summary>
    /// 动画
    /// </summary>
    [SerializeField]
    public Animator Animator;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    [HideInInspector]
    public Vector3 TargetPos = Vector3.zero;

    /// <summary>
    /// 控制器
    /// </summary>
    [HideInInspector]
    public CharacterController CharacterController;
    /// <summary>
    /// 修正的移动速度（用于PVP中的其他玩家)
    /// </summary>
    public float ModifySpeed = 0f;

    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField]
    public float Speed = 10f;

    /// <summary>
    /// 出生点
    /// </summary>
    [HideInInspector]
    public Vector3 BornPoint;

    /// <summary>
    /// 视野范围
    /// </summary>
    public float ViewRange;

    /// <summary>
    /// 巡逻范围
    /// </summary>
    public float PatrolRange;

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 当前角色类型
    /// </summary>
    public RoleType CurrRoleType = RoleType.None;
    /// <summary>
    /// 死亡音效名字
    /// </summary>
    [SerializeField]
    private string DieAudioName;

    /// <summary>
    /// 当前角色信息
    /// </summary>
    public RoleInfoBase CurrRoleInfo = null;

    /// <summary>
    /// 当前角色AI
    /// </summary>
    public IRoleAI CurrRoleAI = null;

    /// <summary>
    /// 锁定敌人
    /// </summary>
    [HideInInspector]
    public RoleCtrl LockEnemy;

    /// <summary>
    /// 角色受伤委托
    /// </summary>
    public System.Action OnRoleHurt;

    /// <summary>
    /// 角色死亡
    /// </summary>
    public System.Action<RoleCtrl> OnRoleDie;
    /// <summary>
    /// 数字变化委托
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public delegate void OnValueChangeHander(ValueChangeType type);

    public OnValueChangeHander OnHPChange;

    public OnValueChangeHander OnMPChange;

    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr CurrRoleFSMMgr = null;

    private RoleHeadBarView roleHeadBarView = null;
    //===================寻路相关=================

    private Seeker m_Seeker;
    public Seeker Seeker
    {
        get { return m_Seeker; }
    }
    //路径
    [HideInInspector]
    public ABPath AStartPath;
    /// <summary>
    /// 当前要去的路径点
    /// </summary>
    public int AStartCurrWayPointIndex=1;

    //==================战斗相关==================
    /// <summary>
    /// 角色攻击
    /// </summary>
    public RoleAttack roleAttack;
    /// <summary>
    ///  角色攻击信息
    /// </summary>
    private RoleAttackInfo roleAttackInfo;
    /// <summary>
    /// 角色受伤
    /// </summary>
    private RoleHurt roleHurt;
    /// <summary>
    /// 是否僵直
    /// </summary>
    [HideInInspector]
    public bool isRigidty;

    /// <summary>
    ///  当前角色攻击信息
    /// </summary>
    public RoleAttackInfo CurrAttackInfo;

    /// <summary>
    /// 角色销毁委托
    /// </summary>
    public Action<Transform> OnRoleDestroy;

    /// <summary>
    /// 上次战斗的时间
    /// </summary>
    [HideInInspector]
    public float PreFightTime = 0f;
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit=false;

    /// <summary>
    /// 角色是否已经死亡
    /// </summary>
    [HideInInspector]
    public bool IsDied;

    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="roleType">角色类型</param>
    /// <param name="roleInfo">角色信息</param>
    /// <param name="ai">AI</param>
    public void Init(RoleType roleType, RoleInfoBase roleInfo, IRoleAI ai)
    {
        CurrRoleType = roleType;
        CurrRoleInfo = roleInfo;
        CurrRoleAI = ai;

        if (CharacterController != null)
        {
            CharacterController.enabled = true;
        }
        isInit = true;
    }

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        //寻路
        m_Seeker = GetComponent<Seeker>();



        if (CurrRoleType == RoleType.MainPlayer)
        {
            if (CameraCtrl.Instance != null)
            {
                CameraCtrl.Instance.Init();
            }
        }

        CurrRoleFSMMgr = new RoleFSMMgr(this,OnDieCallBack,OnDestroyCallBack);

         roleHurt = new RoleHurt(CurrRoleFSMMgr);
        roleHurt.OnRoleHurt = OnRoleHurtCallBack;
        //roleAttack = new RoleAttack();
        roleAttack.SetFSM(CurrRoleFSMMgr);
        if (CurrRoleType==RoleType.Monster)
        {
            ToIdle(RoleIdleState.IdleFight);
        }
        else
        {
            ToIdle();
        }
       
       
    }

    /// <summary>
    /// 角色销毁回调
    /// </summary>
    private void OnDestroyCallBack()
    {
        if (OnRoleDestroy!=null)
        {
            OnRoleDestroy(transform);
        }
        if (roleHeadBarView!=null)
        {
            Destroy(roleHeadBarView.gameObject);
        }
    }
    /// <summary>
    /// 角色死亡回调
    /// </summary>
    private void OnDieCallBack()
    {
        if (CharacterController!=null)
        {
            CharacterController.enabled = false;
        }
        if (OnRoleDie!=null&& CurrRoleInfo!=null)
        {
            OnRoleDie(this);
        }
        
    }
    /// <summary>
    /// 角色复活
    /// </summary>
    public void ToResurgence(RoleIdleState roleIdleState)
    {
        if (CharacterController != null)
        {
            CharacterController.enabled = true ;
        }
        //玩家复活
        CurrRoleInfo.CurrHP = GlobalInit.Instance.CurrPlayer.CurrRoleInfo.MaxHP;
        CurrRoleInfo.CurrMP = GlobalInit.Instance.CurrPlayer.CurrRoleInfo.MaxMP;
        LockEnemy = null;
        ToIdle(roleIdleState);
        if (OnHPChange != null)
        {
            OnHPChange(ValueChangeType.Add);
        }
        if (OnMPChange != null)
        {
            OnMPChange(ValueChangeType.Add);
        }
    }

    /// <summary>
    /// 角色受伤回调
    /// </summary>
    private void OnRoleHurtCallBack()
    {
        if (roleHeadBarView!=null)
        {
            roleHeadBarView.SetSliderHP((float)CurrRoleInfo.CurrHP / CurrRoleInfo.MaxHP);

        }
        if (OnHPChange!=null)
        {
            OnHPChange(ValueChangeType.Subtrack);
        }
    }

    /// <summary>
    /// 角色出生
    /// </summary>
    /// <param name="pos"></param>
    public void Born(Vector3 pos)
    {
        BornPoint = pos;
        transform.position = pos;
        InitHeadBar();
    }

    void Update()
    {

        if (CurrRoleFSMMgr != null)
            CurrRoleFSMMgr.OnUpdate();

        //如果角色没有AI 直接返回
        if (CurrRoleAI == null) return;
        CurrRoleAI.DoAI();
        //每次出生 初始化化一次状态
        if (isInit)
        {
            isInit = false;
            if (CurrRoleInfo.CurrHP<=0)
            {
                ToDie(isDied:true);
            }
            else
            {
                if (CurrRoleType == RoleType.Monster)
                {
                    ToIdle(RoleIdleState.IdleFight);
                }
                else
                {
                    ToIdle();
                }
            }

       
        }


        if (CharacterController == null) return;

        //让角色贴着地面
        if (!CharacterController.isGrounded)
        {
            CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }

        //if (Input.GetMouseButtonUp(1))
        //{
        //    Collider[] colliderArr = Physics.OverlapSphere(transform.position, 3, 1 << LayerMask.NameToLayer("Item"));
        //    if (colliderArr.Length > 0)
        //    {
        //        for (int i = 0; i < colliderArr.Length; i++)
        //        {
        //            Debug.Log("找到了附近的箱子" + colliderArr[i].gameObject.name);
        //        }
        //    }
        //}

        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item"));

            //if (hitArr.Length > 0)
            //{
            //    for (int i = 0; i < hitArr.Length; i++)
            //    {
            //        Debug.Log("找到了" + hitArr[i].collider.gameObject.name);
            //    }
            //}

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")))
            {
                BoxCtrl boxCtrl = hit.collider.GetComponent<BoxCtrl>();
                if (boxCtrl != null)
                {
                    boxCtrl.Hit();
                }
            }
        }

        //让角色贴着地面
        if (!CharacterController.isGrounded)
        {
            CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }

        if (CurrRoleType == RoleType.MainPlayer)
        {
            CameraAutoFollow();
            AutoSmellMap();
        }
       
    }


    private void AutoSmellMap()
    {
        if (SmallMapHelper.Instance == null || UIMainCitySmallMapView.Instance == null) return;

        SmallMapHelper.Instance.transform.position = transform.position;
        UIMainCitySmallMapView.Instance.SmallMap.transform.localPosition = new Vector3(SmallMapHelper.Instance.transform.localPosition.x*-512, SmallMapHelper.Instance.transform.localPosition.z * -512,1);

        UIMainCitySmallMapView.Instance.SmallMapArr.transform.localEulerAngles = new Vector3(0, 0, 360 - transform.eulerAngles.y);



    }

    /// <summary>
    /// 初始化头顶UI条
    /// </summary>
    private void InitHeadBar()
    {
        if (RoleHeadBarRoot.Instance == null) return;
        if (CurrRoleInfo == null) return;
        if (m_HeadBarPos == null) return;

        AssetBundleMgr.Instance.LoadOrDownload("Download/Prefab/UIPrefab/UIOther/RoleHeadBar.assetbundle", "RoleHeadBar",(GameObject obj)=> 
        {
            m_HeadBar = Instantiate(obj);
            m_HeadBar.transform.parent = RoleHeadBarRoot.Instance.gameObject.transform;
            m_HeadBar.transform.localScale = Vector3.one;
            m_HeadBar.transform.localPosition = Vector3.zero;

            roleHeadBarView = m_HeadBar.GetComponent<RoleHeadBarView>();

            //给预设赋值
            roleHeadBarView.Init(m_HeadBarPos, CurrRoleInfo.RoleNickName, isShowHPBar: (CurrRoleType == RoleType.MainPlayer || CurrRoleType == RoleType.OtherPlayer ? false : true), sliderHpvalue: (float)CurrRoleInfo.CurrHP / CurrRoleInfo.MaxHP);
        });

        ////克隆预设
        //m_HeadBar = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIOther, "RoleHeadBar");
       
    }


    #region 控制角色方法

    public void ToIdle(RoleIdleState roleIdleState= RoleIdleState.IdleNormal)
    {
        CurrRoleFSMMgr.ToIdleState= roleIdleState;
        CurrRoleFSMMgr.ChangeState(RoleState.Idle);
    }
    /// <summary>
    /// 临时测试用
    /// </summary>
    public void ToRun()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Run);
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) return;

        if (isRigidty) return;

        //如果目标点不是原点 进行移动
        if (targetPos == Vector3.zero) return;
        TargetPos = targetPos;

        m_Seeker.StartPath(transform.position,targetPos,(Path p)=> {
            if (!p.error)
            {
                AStartPath = (ABPath)p;
                if (Vector3.Distance(AStartPath.endPoint,new Vector3(AStartPath.originalEndPoint.x, AStartPath.endPoint.y, AStartPath.originalEndPoint.z))>0.5)
                {
                    DebugApp.Log("不能到达目标点");
                    AStartPath = null;
                    return;
                }

                if (CurrRoleType==RoleType.MainPlayer)
                {
                    //PVP发消息给服务器
                    SendPVPMove(targetPos, AStartPath.vectorPath);

                }


                AStartCurrWayPointIndex = 1;
                CurrRoleFSMMgr.ChangeState(RoleState.Run);
            }
            else
            {
                DebugApp.Log("寻路出错");
                AStartPath = null;
            }


        });
        
    }

    private void SendPVPMove(Vector3 targetPos,List<Vector3>path)
    {
        if (SceneMgr.Instance.CurrPlayerType==PlayType.PVP)
        {
            float pathLen = 0f;//路径的总长度 计算出路径

            for (int i = 0; i < path.Count; i++)
            {
                if (i == path.Count - 1) continue;

                float dis = Vector3.Distance(path[i],path[i+1]);
                pathLen += dis;
            }

            //时间=距离/速度
            float needTime = pathLen / Speed;

            WorldMap_CurrRoleMoveProto proto = new WorldMap_CurrRoleMoveProto();

            proto.TargetPosX = targetPos.x;
            proto.TargetPosY = targetPos.y;
            proto.TargetPosZ = targetPos.z;
            proto.ServerTime = GlobalInit.Instance.GetCurrServerTime();
            proto.NeedTime = (int)(needTime * 1000);

            NetWorkSocket.Instance.SendMsg(proto.ToArray());
        }
    }

    /// <summary>
    /// 发起攻击
    /// </summary>
    /// <param name="roleAttackType"></param>
    /// <param name="index"></param>
    public void ToAttackByIndex(RoleAttackType roleAttackType, int index)
    {
        roleAttack.ToAttack(roleAttackType,index);
    }

    /// <summary>
    /// 发起攻击
    /// </summary>
    /// <param name="roleAttackType"></param>
    /// <param name="index"></param>
    public bool ToAttack(RoleAttackType roleAttackType, int skillId)
    {
       return roleAttack.ToAttack(roleAttackType, skillId);
    }
    /// <summary>
    /// 播放攻击动画
    /// </summary>
    /// <param name="skillId"></param>
    public void PlayAttack(int skillId)
    {
        roleAttack.PlayAttack(skillId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue">受到的攻击力</param>
    /// <param name="delay">延迟时间</param>
    public void ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    {
        StartCoroutine(roleHurt.ToHurt(roleTransferAttackInfo));
    }

    public void ToDie(bool isDied=false)
    {
        IsDied = isDied;
        CurrRoleInfo.CurrHP = 0;
        PlayAudio(DieAudioName,0);
        CurrRoleFSMMgr.ChangeState(RoleState.Die);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="delayTime"></param>
    public void  PlayAudio(string audioName,float delayTime)
    {
        StartCoroutine(PlayAudioCoroutine(audioName, delayTime));

    }

    private IEnumerator PlayAudioCoroutine(string audioName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioEffectMgr.Instance.Play(string.Format("Download/Audio/Fight/{0}", audioName),transform.position,is3D:true);
    }

    public void ToSelect()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Select);
    }

    #endregion

    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    void OnDestroy()
    {
        if (m_HeadBar != null)
        {
            Destroy(m_HeadBar);
        }
    }
    #endregion

    #region CameraAutoFollow 摄像机自动跟随
    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtrl.Instance == null) return;
        AudioBackGroundMgr.Instance.transform.position = gameObject.transform.position;
        CameraCtrl.Instance.transform.position = gameObject.transform.position;
        CameraCtrl.Instance.AutoLookAt(gameObject.transform.position);
    }
    #endregion
}