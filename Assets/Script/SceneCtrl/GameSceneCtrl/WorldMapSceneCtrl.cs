using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;


/// <summary>
/// 世界地图场景的主控制器
/// </summary>
public class WorldMapSceneCtrl : GameSceneCtrlBase
{
    public static WorldMapSceneCtrl Instance;
    /// <summary>
    /// 主角出生点
    /// </summary>
    [SerializeField]
    private Transform m_PlayerBornPos;
    /// <summary>
    /// 当前实体
    /// </summary>
    private WorldMapEntity m_CurrWorldMapEntity;
    /// <summary>
    /// 传送点
    /// </summary>
    private Dictionary<int, WorldMapTransCtrl> m_TransPosDic;

    private WorldMap_PosProto m_WorldMapPosProto;

    private WorldMap_RoleAlreadyEnterProto m_WorldAllreadyEnterProto;

    private Dictionary<int, RoleCtrl> m_AllRoleDic;

    private float m_NextSendTime;
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        m_AllRoleDic = new Dictionary<int, RoleCtrl>();
        AddEventListener();
    }
    private void AddEventListener()
    {
        //服务器广播当前场景角色
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_InitRole, OnWorldMapInitRole);
        //服务器广播其他角色进入场景消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleLeave, OnWorldMapOtherRoleLeave);
        //服务器广播其他角色离开场景消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleEnter, OnWorldMapOtherRoleEnter);
        //服务器广播其他角色移动消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleMove, OnWorldMapOtherRoleMove);
        //服务器广播角色使用技能消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleUseSkill, OnWorldMapOtherRoleUseSkill);
        //服务器广播角色死亡消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleDie, OnWorldMap_OtherRoleDie);
        //服务器广播角色复活消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_OtherRoleResurgence, OnWorldMap_OtherRoleResurgence);

    }


    /// <summary>
    /// 服务器广播角色复活消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMap_OtherRoleResurgence(byte[] p)
    {
        WorldMap_OtherRoleResurgenceProto proto = WorldMap_OtherRoleResurgenceProto.GetProto(p);

        if (m_AllRoleDic.ContainsKey(proto.RoleId))
        {
            m_AllRoleDic[proto.RoleId].ToResurgence(RoleIdleState.IdleNormal);
        }
    }

    /// <summary>
    /// 服务器广播角色死亡消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMap_OtherRoleDie(byte[] p)
    {
        WorldMap_OtherRoleDieProto proto = WorldMap_OtherRoleDieProto.GetProto(p);
        if (proto.RoleIdList!=null&& proto.RoleIdList.Count>0)
        {
            for (int i = 0; i < proto.RoleIdList.Count; i++)
            {
                int dieRoleId = proto.RoleIdList[i];
                if (m_AllRoleDic.ContainsKey(dieRoleId))
                {
                    m_AllRoleDic[dieRoleId].ToDie();

                    if (m_AllRoleDic[dieRoleId].CurrRoleType==RoleType.MainPlayer)
                    {
                        //如果当前玩家死亡了则弹出复活UI窗口
                        WorldMapCtrl.Instance.EnemyNickName = m_AllRoleDic[proto.AttackRoleId].CurrRoleInfo.RoleNickName;
                        WorldMapCtrl.Instance.OpenView(WindowUIType.WorldMapFail);
                    }
                }

            }
        }


    }

    /// <summary>
    /// 服务器广播角色使用技能消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapOtherRoleUseSkill(byte[] p)
    {
        WorldMap_OtherRoleUseSkillProto proto = WorldMap_OtherRoleUseSkillProto.GetProto(p);
        //1.处理攻击者
        //让攻击者使用技能
        //如果攻击者存在
        if (m_AllRoleDic.ContainsKey(proto.AttackRoleId))
        {
            RoleCtrl attackRole = m_AllRoleDic[proto.AttackRoleId];

            //修正攻击者位置
            attackRole.transform.position = new Vector3(proto.RolePosX, proto.RolePosY, proto.RolePosZ);
            attackRole.transform.eulerAngles = new Vector3(0, proto.RoleYAngle, 0);

            attackRole.PlayAttack(proto.SkillId);

        }
        //2.处理被攻击者
        if (proto.ItemList!=null&&proto.ItemList.Count>0)
        {
            for (int i = 0; i < proto.ItemList.Count; i++)
            {
                WorldMap_OtherRoleUseSkillProto.BeAttackItem item = proto.ItemList[i];

                if (m_AllRoleDic.ContainsKey(item.BeAttackRoleId))
                {
                    RoleCtrl beAttackRole = m_AllRoleDic[item.BeAttackRoleId];
                    RoleTransferAttackInfo attackInfo = new RoleTransferAttackInfo();
                    attackInfo.AttackRoleId = proto.AttackRoleId;
                    attackInfo.BeAttackRoleId = item.BeAttackRoleId;
                    attackInfo.SkillId = proto.SkillId;
                    attackInfo.SkillLevel = proto.SkillLevel;
                    attackInfo.IsCri = item.IsCri==1;
                    attackInfo.HurtValue = item.ReduceHp;

                    //给被攻击者传递攻击信息
                    beAttackRole.ToHurt(attackInfo);


                }
            } 
        }


    }

    /// <summary>
    /// 服务器广播其他角色移动消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapOtherRoleMove(byte[] p)
    {
        WorldMap_OtherRoleMoveProto proto = WorldMap_OtherRoleMoveProto.GetProto(p);
        int roleId = proto.RoleId;
        Vector3 tagetPos = new Vector3(proto.TargetPosX,proto.TargetPosY,proto.TargetPosZ);
        long serverTime = proto.ServerTime;
        int needTime = proto.NeedTime;

        if (m_AllRoleDic.ContainsKey(roleId))
        {
            ((OtherRoleAI)m_AllRoleDic[roleId].CurrRoleAI).MoveTo(tagetPos,serverTime,needTime);           
        }

    }

    /// <summary>
    ///  服务器广播其他角色进入场景消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapOtherRoleEnter(byte[] p)
    {
       WorldMap_OtherRoleEnterProto proto = WorldMap_OtherRoleEnterProto.GetProto(p);

       int roleId = proto.RoleId;
       string roleNickName = proto.RoleNickName;
       int roleLevel = proto.RoleLevel;
       int roleJobId = proto.RoleJobId;
        int maxHp = proto.RoleMaxHP;
        int currHp = proto.RoleCurrHP;
        int maxMp = proto.RoleMaxMP;
        int currMp = proto.RoleCurrMP;
        Vector3 rolePos = new Vector3(proto.RolePosX, proto.RolePosY, proto.RolePosZ);
       float roleYAngle = proto.RoleYAngle;

       CreateOtherPlayer(roleId, roleNickName, roleLevel, roleJobId, maxHp, currHp, maxMp, currMp, rolePos, roleYAngle);
     }
    /// <summary>
    ///服务器广播其他角色离开场景消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapOtherRoleLeave(byte[] p)
    {
        WorldMap_OtherRoleLeaveProto proto = WorldMap_OtherRoleLeaveProto.GetProto(p);
        int roleId = proto.RoleId;

        DestroyOtherRole(roleId);
    }
    /// <summary>
    /// 销毁其他玩家
    /// </summary>
    /// <param name="roleId"></param>
    private void DestroyOtherRole(int roleId)
    {
        if (m_AllRoleDic.ContainsKey(roleId))
        {
            Destroy(m_AllRoleDic[roleId].gameObject);
            m_AllRoleDic.Remove(roleId);
        }
    }

    /// <summary>
    /// 服务器广播当前场景角色
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapInitRole(byte[] p)
    {
        WorldMap_InitRoleProto proto = WorldMap_InitRoleProto.GetProto(p);
        int roleCount = proto.RoleCount;

        List<WorldMap_InitRoleProto.RoleItem> lst = proto.ItemList;
        if (lst == null) return;
        for (int i = 0; i < lst.Count; i++)
        {
            int roleId = lst[i].RoleId;
            string roleNickName = lst[i].RoleNickName;
            int roleLevel = lst[i].RoleLevel;
            int roleJobId = lst[i].RoleJobId;
            int maxHp = lst[i].RoleMaxHP;
            int currHp = lst[i].RoleCurrHP;
            int maxMp = lst[i].RoleMaxMP;
            int currMp = lst[i].RoleCurrMP;
            Vector3 rolePos = new Vector3(lst[i].RolePosX, lst[i].RolePosY, lst[i].RolePosZ);
            float roleYAngle = lst[i].RoleYAngle;

            CreateOtherPlayer(roleId, roleNickName, roleLevel, roleJobId, maxHp, currHp, maxMp, currMp, rolePos, roleYAngle);
        }
    }
    /// <summary>
    /// 创建其他玩家
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="roleNickName"></param>
    /// <param name="roleLevel"></param>
    /// <param name="roleJobId"></param>
    /// <param name="rolePos"></param>
    /// <param name="roleYAngle"></param>
    private void CreateOtherPlayer(int roleId, string roleNickName, int roleLevel, int roleJobId,int maxHp,int currHp,int maxMp,int currMp, Vector3 rolePos, float roleYAngle)
    {
        RoleCtrl ctrl = RoleMgr.Instance.LoadOtherRole(roleId,  roleNickName,  roleLevel, roleJobId, maxHp, currHp, maxMp, currMp);

        ctrl.Born(rolePos);
        ctrl.transform.eulerAngles = new Vector3(0, roleYAngle, 0);

        m_AllRoleDic[roleId] = ctrl;
    }
    protected override void OnStart()
    {
        base.OnStart();
        if (GlobalInit.Instance.CurrPlayer!=null)
        {
            //设置主角自动战斗状态
            GlobalInit.Instance.CurrPlayer.roleAttack.IsAutoFight = false;
            
        }
        m_TransPosDic = new Dictionary<int, WorldMapTransCtrl>();
    }
    protected override void OnLoadCityViewCompelete(GameObject obj)
    {
        base.OnLoadCityViewCompelete(obj);
        if (GlobalInit.Instance == null) return;

        RoleMgr.Instance.InitMainPlayer();

        if (GlobalInit.Instance.CurrPlayer != null)
        {
            m_AllRoleDic[GlobalInit.Instance.CurrPlayer.CurrRoleInfo.RoleId] = GlobalInit.Instance.CurrPlayer;
            m_CurrWorldMapEntity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId);

            AudioBackGroundMgr.Instance.Play(m_CurrWorldMapEntity.Audio_BG);

            InitTransPos();
            //如果没设置玩家目标场景传送点Id 则使用表格上配置的出生点
            if (SceneMgr.Instance.TransWorldMapTransPosId == 0)
            {
                //如果服务器告诉了客户端 主角最后进入最后场景地图坐标信息不是空的
                if (!string.IsNullOrEmpty(PlayerCtrl.Instance.LastInWorldMapPos))
                {
                    string[] arr = PlayerCtrl.Instance.LastInWorldMapPos.Split('_');
                    Vector3 pos = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]));

                    GlobalInit.Instance.CurrPlayer.Born(pos);
                    GlobalInit.Instance.CurrPlayer.transform.eulerAngles = new Vector3(0,float.Parse(arr[3]),0);
                }
                else
                {
                    if (m_CurrWorldMapEntity.RoleBirthPostion != Vector3.zero)
                    {
                        GlobalInit.Instance.CurrPlayer.Born(m_CurrWorldMapEntity.RoleBirthPostion);
                        GlobalInit.Instance.CurrPlayer.transform.eulerAngles = new Vector3(0, m_CurrWorldMapEntity.RoleBirthEulerAnglesY, 0);
                    }
                    else
                    {
                        GlobalInit.Instance.CurrPlayer.Born(m_PlayerBornPos.position);
                    }
                }

               
            }
            else
            {
                //找传送点
                if (m_TransPosDic.ContainsKey(SceneMgr.Instance.TransWorldMapTransPosId))
                {
                    Vector3 newBornPos = m_TransPosDic[SceneMgr.Instance.TransWorldMapTransPosId].transform.forward.normalized * 3 + m_TransPosDic[SceneMgr.Instance.TransWorldMapTransPosId].transform.position;
                    Vector3 lookAtBornPos = m_TransPosDic[SceneMgr.Instance.TransWorldMapTransPosId].transform.forward.normalized * 3.5f + m_TransPosDic[SceneMgr.Instance.TransWorldMapTransPosId].transform.position;

                    GlobalInit.Instance.CurrPlayer.Born(newBornPos);
                    GlobalInit.Instance.CurrPlayer.transform.LookAt(lookAtBornPos);
                    SceneMgr.Instance.TransWorldMapTransPosId = 0;
                }
            }
            this.SendRoleAlreadyEnter(SceneMgr.Instance.CurrWorldMapId, GlobalInit.Instance.CurrPlayer.transform.position, GlobalInit.Instance.CurrPlayer.transform.eulerAngles.y);
            PlayerCtrl.Instance.SetMainCityRoleData();

            DebugApp.Log("加载完毕");


            if (DelegateDefine.Instance.OnSenceLoadOk != null)
            {
                DelegateDefine.Instance.OnSenceLoadOk();
            }
        }
        StartCoroutine(InitNPC());
        AutoMove();
    }
    private IEnumerator InitNPC()
    {

        yield return null;

        if (m_CurrWorldMapEntity == null) yield break;

        LoadNPC(0);
        //for (int i = 0; i < m_CurrWorldMapEntity.NPCWorldMapList.Count; i++)
        //{
        //    NPCWorldMapData data = m_CurrWorldMapEntity.NPCWorldMapList[i];

        //    NPCEntity entiy = NPCDBModel.Instance.Get(data.NPCId);

        //    string prefabName = entiy.PrefabName;

        //    GameObject obj = RoleMgr.Instance.LoadNPC(prefabName);

        //    obj.transform.position = data.NPCPostion;

        //    obj.transform.eulerAngles = new Vector3(0,data.EulerAnglesY,0);

        //    NPCCtrl ctrl = obj.GetComponent<NPCCtrl>();
        //    ctrl.Init(data);
        //}

    }

    private void LoadNPC(int index)
    {
        if (m_CurrWorldMapEntity.NPCWorldMapList.Count == 0) return;

        NPCWorldMapData data = m_CurrWorldMapEntity.NPCWorldMapList[index];
        NPCEntity entiy = NPCDBModel.Instance.Get(data.NPCId);
        RoleMgr.Instance.LoadNPC(entiy.PrefabName,
            (GameObject obj)=>
            {
                obj.transform.position = data.NPCPostion;

                obj.transform.eulerAngles = new Vector3(0, data.EulerAnglesY, 0);

                NPCCtrl ctrl = obj.GetComponent<NPCCtrl>();
                ctrl.Init(data);

                index++;
                if (index== m_CurrWorldMapEntity.NPCWorldMapList.Count)
                {
                    DebugApp.Log("NPC加载完毕");
                }
                else
                {
                    LoadNPC(index);
                }

            }


            );


    }

    /// <summary>
    /// 初始化传送点
    /// </summary>
    /// <returns></returns>
    private void InitTransPos()
    {

        string[] posInfoArr = m_CurrWorldMapEntity.TransPos.Split('|');
        AssetBundleMgr.Instance.LoadOrDownload("Download/Prefab/Effect/Common/Efflect_Trans.assetbundle", "Efflect_Trans",(GameObject obj)=> 
        {

            for (int i = 0; i < posInfoArr.Length; i++)
            {
                string[] posInfo = posInfoArr[i].Split('_');
                if (posInfo.Length == 7)
                {
                    Vector3 pos = new Vector3();

                    float f = 0;
                    float.TryParse(posInfo[0], out f);
                    pos.x = f;

                    float.TryParse(posInfo[1], out f);
                    pos.y = f;

                    float.TryParse(posInfo[2], out f);
                    pos.z = f;

                    //y轴旋转
                    float y = 0;
                    float.TryParse(posInfo[3], out y);

                    //当前编号
                    int currTranPosId = 0;
                    int.TryParse(posInfo[4], out currTranPosId);

                    int tragetTranSceneId = 0;
                    int targetSceneTranId = 0;
                    int.TryParse(posInfo[5], out tragetTranSceneId);
                    int.TryParse(posInfo[6], out targetSceneTranId);

                    ////克隆传送点
                    //GameObject obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.Effect, "Efflect_Trans", cache: true, returnClone: true);
                    obj = Instantiate(obj);
                    obj.transform.position = pos;
                    obj.transform.eulerAngles = new Vector3(0, y, 0);
                    WorldMapTransCtrl ctl = obj.GetComponent<WorldMapTransCtrl>();
                    if (ctl != null)
                    {
                        ctl.SetParam(currTranPosId, tragetTranSceneId, targetSceneTranId);
                    }
                    m_TransPosDic[currTranPosId] = ctl;
                }
            }


        });


     

    }

    /// <summary>
    /// 主角自动移动
    /// </summary>
    public void AutoMove()
    {
        if (!WorldMapCtrl.Instance.isAutoMove) return;
        if (SceneMgr.Instance.CurrWorldMapId==WorldMapCtrl.Instance.ToSceneId)
        {
            Debug.Log("到达目标场景");
            if (WorldMapCtrl.Instance.ToScenePos!=Vector3.zero)
            {
                GlobalInit.Instance.CurrPlayer.MoveTo(WorldMapCtrl.Instance.ToScenePos);
            }
          
            WorldMapCtrl.Instance.isAutoMove = false;
        }

        foreach (var item in m_TransPosDic)
        {
            if (item.Value.TagetTransScennId == WorldMapCtrl.Instance.ToSceneId)
            {
                //这个出口是我要走的
                GlobalInit.Instance.CurrPlayer.MoveTo(item.Value.transform.position);
            }
        }

        WorldMapCtrl.Instance.currSceneId = WorldMapCtrl.Instance.ToSceneId;
        if (WorldMapCtrl.Instance.SceneIdQueue.Count>0)
        {
            WorldMapCtrl.Instance.ToSceneId = WorldMapCtrl.Instance.SceneIdQueue.Dequeue();
        }

    }

    #region 发送消息
    /// <summary>
    /// 发送角色位置给服务器
    /// </summary>
    private void SendPlayerPos()
    {
        if (GlobalInit.Instance!=null&&GlobalInit.Instance.CurrPlayer!=null)
        {
            m_WorldMapPosProto.x = GlobalInit.Instance.CurrPlayer.transform.position.x;
            m_WorldMapPosProto.y = GlobalInit.Instance.CurrPlayer.transform.position.y;
            m_WorldMapPosProto.z = GlobalInit.Instance.CurrPlayer.transform.position.z;
            m_WorldMapPosProto.yAngle = GlobalInit.Instance.CurrPlayer.transform.eulerAngles.y;
            NetWorkSocket.Instance.SendMsg(m_WorldMapPosProto.ToArray());
        }
    }
    private void SendRoleAlreadyEnter(int worldMapSceneId,Vector3 currRolePos,float currRoleYAngel)
    {
        m_WorldAllreadyEnterProto.TargetWorldMapSceneId = worldMapSceneId;
        m_WorldAllreadyEnterProto.RolePosX = currRolePos.x;
        m_WorldAllreadyEnterProto.RolePosY = currRolePos.y;
        m_WorldAllreadyEnterProto.RolePosZ = currRolePos.z;

        NetWorkSocket.Instance.SendMsg(m_WorldAllreadyEnterProto.ToArray());
    }
    #endregion

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Time.time>m_NextSendTime)
        {
            m_NextSendTime +=1;
            SendPlayerPos();
        }
    }

    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        //服务器广播当前场景角色
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_InitRole, OnWorldMapInitRole);
        //服务器广播其他角色进入场景消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleLeave, OnWorldMapOtherRoleLeave);
        //服务器广播其他角色离开场景消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleEnter, OnWorldMapOtherRoleEnter);
        //服务器广播其他角色移动消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleMove, OnWorldMapOtherRoleMove);
        //服务器广播角色使用技能消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleUseSkill, OnWorldMapOtherRoleUseSkill);
        //服务器广播角色死亡消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleDie, OnWorldMap_OtherRoleDie);
        //服务器广播角色复活消息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.WorldMap_OtherRoleResurgence, OnWorldMap_OtherRoleResurgence);
    }
    #endregion

}