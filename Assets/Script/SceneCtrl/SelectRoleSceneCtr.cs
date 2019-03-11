using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectRoleSceneCtr : MonoBehaviour {


    /// <summary>
    /// 职业列表
    /// </summary>
    private List<JobEntity> m_JobLst;
    /// <summary>
    /// 角色容器
    /// </summary>
    public Transform[] CreateRoleContainers;
    /// <summary>
    /// 角色列表
    /// </summary>
    private Dictionary<int, RoleCtrl> m_JobRoleDic = new Dictionary<int, RoleCtrl>();
    /// <summary>
    /// 选人视图
    /// </summary>
    private UISceneSelectRoleView m_UISceneSelectRoleView;
    /// <summary>
    /// 当前选择职业ID
    /// </summary>
    private int m_CurrSelectJobId;
    /// <summary>
    /// 创建角色所需场景模型
    /// </summary>
    [SerializeField]
    private Transform[] creteRoleSceneModle;
    /// <summary>
    /// 当前选择的角色模型
    /// </summary>
    private GameObject m_CurrSelectRoleModel;
    /// <summary>
    /// 当前选择角色编号
    /// </summary>
    private int m_CurrSelectRoleId;
    /// <summary>
    /// 是否新建角色
    /// </summary>
    private bool isCreateRole;

    private int m_LastInWorldMapId;
    private void Awake()
    {
     
    }
    void Start ()
    {
        UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.SelctRole, (GameObject obj) =>
        {
            m_UISceneSelectRoleView = obj.GetComponent<UISceneSelectRoleView>();

            // m_UISceneSelectRoleView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.SelctRole).GetComponent<UISceneSelectRoleView>();
            if (DelegateDefine.Instance.OnSenceLoadOk != null)
            {
                DelegateDefine.Instance.OnSenceLoadOk();
            }
            if (m_UISceneSelectRoleView != null)
            {
                m_UISceneSelectRoleView.m_UISceneRoleDragView.OnSelctDrag = OnSelctRoleDrag;

                if (m_UISceneSelectRoleView.JobItem != null && m_UISceneSelectRoleView.JobItem.Length > 0)
                {
                    for (int i = 0; i < m_UISceneSelectRoleView.JobItem.Length; i++)
                    {
                        m_UISceneSelectRoleView.JobItem[i].onSelectJobHandle = OnSelectJobCallBack;
                    }

                }

            }
            //监听协议
            //服务器返回登录信息
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_LogOnGameServerReturn, LogOnGameServerReturn);
            //服务器返回创建信息
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_CreateRoleReturn, CreateRoleReturn);
            //服务器返回进入游戏信息
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_EnterGameReturn, EnterGameReturn);
            //服务器返回删除角色信息
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_DeleteRoleReturn, DeleteRoleReturn);
            //服务器返回角色信息
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_SelectRoleInfoReturn, SelectRoleInfoReturn);
            //服务器返回角色学会的技能
            SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleData_SkillReturn, RoleData_SkillReturn);

            m_UISceneSelectRoleView.OnBtnBeginGameClick = OnBtnBeginGameClick;
            m_UISceneSelectRoleView.OnBtnDeleteRoleClick = OnBtnDeleteRoleClick;
            m_UISceneSelectRoleView.OnBtnReturnClick = OnBtnReturnClick;
            m_UISceneSelectRoleView.OnBtnCreateRoleClick = OnBtnCreateRoleClick;

            //加载角色镜像
            //
            LoadJobObj(OnLoadJobObjectComplete);



        });
    
    }


    /// <summary>
    /// 加载角色镜像完成
    /// </summary>
    private void OnLoadJobObjectComplete()
    {
        LogOnGameServer();
    }

    /// <summary>
    /// 服务器返回角色学会的技能信息
    /// </summary>
    /// <param name="p"></param>
    private void RoleData_SkillReturn(byte[] p)
    {
        RoleData_SkillReturnProto proto = RoleData_SkillReturnProto.GetProto(p);

        GlobalInit.Instance.MainPlayerInfo.LoadSkill(proto);
        //切换场景

        PlayerCtrl.Instance.LastInWorldMapId = m_LastInWorldMapId;


        SceneMgr.Instance.LoadToWorldMap(m_LastInWorldMapId);
    }

    /// <summary>
    /// 服务器返回角色信息
    /// </summary>
    /// <param name="p"></param>
    private void SelectRoleInfoReturn(byte[] p)
    {
        RoleOperation_SelectRoleInfoReturnProto proto = RoleOperation_SelectRoleInfoReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            GlobalInit.Instance.MainPlayerInfo = new RoleInfoMainPlayer(proto);
            m_LastInWorldMapId = proto.LastInWorldMapId;

            PlayerCtrl.Instance.LastInWorldMapId = m_LastInWorldMapId;
            PlayerCtrl.Instance.LastInWorldMapPos = proto.LastInWorldMapPos;

        }
    }

    /// <summary>
    /// 新建角色按钮点击
    /// </summary>
    private void OnBtnCreateRoleClick()
    {
        ToCreateRoleUI();
    }

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBtnReturnClick()
    {
        //如果是新建角色场景 并且当前没有角色 则返回选区场景

        //如果是新建角色场景 则返回已有角色

        //如果是已有角色场景 返回选区场景

        if (isCreateRole)
        {
            if (m_RoleList == null|| m_RoleList.Count == 0)
            {
                NetWorkSocket.Instance.DisConnected();
                SceneMgr.Instance.LoadToLogOn();
            }
            else
            {
                //并且存在已有角色
                //清楚新建角色时候的模型
                ClaerCloneCreateRole();
                m_CurrSelectRoleId = 0;
                m_DragTarget.eulerAngles = Vector3.up * 0;
                //选择已有角色
                isCreateRole = false;
                m_UISceneSelectRoleView.SetCreateRoleUI(false);
                m_UISceneSelectRoleView.SetUISelectRoleActive(true);
                SetCreateRoleModel(false);
                  
                m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
                m_UISceneSelectRoleView.SetSelected(m_RoleList[0].RoleId);
                SetSelectRole(m_RoleList[0].RoleId);
             


            }
        }
        else
        {
            NetWorkSocket.Instance.DisConnected();

            SceneMgr.Instance.LoadToLogOn();
        }
        
    }


    #region 登录服务器相关
    /// <summary>
    /// 服务器返回登录信息
    /// </summary>
    /// <param name="p"></param>
    private void LogOnGameServerReturn(byte[] p)
    {
        RoleOperation_LogOnGameServerReturnProto proto = RoleOperation_LogOnGameServerReturnProto.GetProto(p);

        int roleCount = proto.RoleCount;

        DebugApp.Log("roleCount" + roleCount);

        if (roleCount == 0)
        {
            //新建角色
            isCreateRole = true;
            m_UISceneSelectRoleView.SetCreateRoleUI(true);
            m_UISceneSelectRoleView.SetUISelectRoleActive(false);
            SetCreateRoleModel(true);
            CloneCreateRole();
            m_CurrSelectJobId = 1;
            SetSelectJob();

            m_UISceneSelectRoleView.RandomName();

        }
        else
        {
            //选择已有角色
            isCreateRole = false;
            m_UISceneSelectRoleView.SetCreateRoleUI(false);
            m_UISceneSelectRoleView.SetUISelectRoleActive(true);
            SetCreateRoleModel(false);
            if (proto.RoleList != null)
            {
                m_RoleList = proto.RoleList;
                m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
                m_UISceneSelectRoleView.SetSelected(m_RoleList[0].RoleId);
                SetSelectRole(m_RoleList[0].RoleId);
               
            }

        }

    }
    /// <summary>
    /// 发送登录区服消息
    /// </summary>
    /// <param name="p"></param>
    private void LogOnGameServer()
    {
        RoleOperation_LogOnGameServerProto proto = new RoleOperation_LogOnGameServerProto();

        proto.AccountId = GlobalInit.Instance.CurrAccount.Id;

        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }
    #endregion
    #region 创建角色服务器相关
    /// <summary>
    /// 返回服务器创建角色消息
    /// </summary>
    /// <param name="p"></param>
    private void CreateRoleReturn(byte[] p)
    {
        RoleOperation_CreateRoleReturnProto proto = RoleOperation_CreateRoleReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            DebugApp.Log("创建角色成功");
            SceneMgr.Instance.LoadToWorldMap(1);
        }
        else
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败");
        }

    }

    /// <summary>
    /// 开始游戏按钮点击创建角色
    /// </summary>
    private void OnBtnBeginGameClick()
    {
        if (isCreateRole)
        {
            //新建角色 逻辑
            RoleOperation_CreateRoleProto proto = new RoleOperation_CreateRoleProto();
            DebugApp.Log(m_CurrSelectJobId);
            proto.JobId = (byte)m_CurrSelectJobId;
            proto.RoleNickName = m_UISceneSelectRoleView.txtNickName.text;
            if (string.IsNullOrEmpty(proto.RoleNickName))
            {
                MessageCtrl.Instance.Show("提示", "请输入你的昵称");
                return;
            }


            NetWorkSocket.Instance.SendMsg(proto.ToArray());
        }
        else
        {
            //选择已有角色
            RoleOperation_EnterGameProto proto = new RoleOperation_EnterGameProto();

            proto.RoleId = m_CurrSelectRoleId;

            NetWorkSocket.Instance.SendMsg(proto.ToArray());


        }
       
    }
    #endregion

    #region 动态加载角色相关
    /// <summary>
    /// 加载角色镜像
    /// </summary>
    private void LoadJobObj(Action onComplete)
    {
        m_JobLst = JobDBModel.Instance.GetList();
        LoadJob(0, onComplete);     

    }
    /// <summary>
    /// 加载职业预设
    /// </summary>
    /// <param name="index"></param>
    /// <param name="onComplete"></param>
    private void LoadJob(int index,Action onComplete)
    {
        JobEntity entity = m_JobLst[index];


             AssetBundleMgr.Instance.LoadOrDownload(string.Format("Download/Prefab/RolePrefab/Player/{0}.assetbundle", entity.PrefabName), entity.PrefabName,
                (GameObject obj)=>
                {
                    if (obj != null)
                    {
                        GlobalInit.Instance.JobObjectDic[entity.Id] = obj;

                        index++;
                        if (index== m_JobLst.Count)
                        {
                            //加载完毕
                            if (onComplete!=null)
                            {
                                onComplete();
                            }                         
                        }
                        else
                        {
                            LoadJob(index, onComplete);
                        }
                    }
                }
                
                );

           
        
    }

    /// <summary>
    /// 克隆角色
    /// </summary>
    private void CloneCreateRole()
    {
        if (CreateRoleContainers == null || CreateRoleContainers.Length < 4) return;

        for (int i = 0; i < m_JobLst.Count; i++)
        {
            GameObject obj = Instantiate(GlobalInit.Instance.JobObjectDic[m_JobLst[i].Id]);

            obj.transform.parent = CreateRoleContainers[i];

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            obj.transform.localScale = Vector3.one;
            m_CloneCreateRoleLst.Add(obj);
            RoleCtrl roleCtrl = obj.GetComponent<RoleCtrl>();
            if (roleCtrl!=null)
            {
                m_JobRoleDic[m_JobLst[i].Id] = roleCtrl;
            }

        }
     
    }
    /// <summary>
    /// 克隆的角色
    /// </summary>
    private List<GameObject> m_CloneCreateRoleLst = new List<GameObject>();
    /// <summary>
    ///清除克隆的角色
    /// </summary>
    private void ClaerCloneCreateRole()
    {
        if (m_CloneCreateRoleLst!=null&& m_CloneCreateRoleLst.Count>0)
        {
            for (int i = 0; i < m_CloneCreateRoleLst.Count; i++)
            {
                Destroy(m_CloneCreateRoleLst[i]);
               
            }
            m_CloneCreateRoleLst.Clear();
        }
    }

    #endregion
    #region 设置拖拽旋转相关
    /// <summary>
    /// 设置创建角色所需场景模型
    /// </summary>
    /// <param name="isShow"></param>
    public void SetCreateRoleModel(bool isShow)
    {
        if (creteRoleSceneModle != null && creteRoleSceneModle.Length > 0)
        {
            for (int i = 0; i < creteRoleSceneModle.Length; i++)
            {
                creteRoleSceneModle[i].gameObject.SetActive(isShow);
            }
        }
    }
    #region 拖拽旋转
    /// <summary>
    /// 是否旋转中
    /// </summary>
    private bool m_IsRotateing = false;
    /// <summary>
    /// 拖拽的目标
    /// </summary>
    public Transform m_DragTarget;
    /// <summary>
    /// 目标角度
    /// </summary>
    private float m_TagetAngle = 0;
    /// <summary>
    /// 每次拖拽后角度
    /// </summary>
    private float m_RotateAngle = 90;
    /// <summary>
    /// 旋转速度
    /// </summary>
    private float m_RotateSpeed=10f;


    /// <summary>
    /// 点击职业项回调
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="rotateAngle"></param>
    private void OnSelectJobCallBack(int jobId, int rotateAngle)
    {
        if (m_IsRotateing) return;
        m_IsRotateing = true;
        m_CurrSelectJobId = jobId;
        m_TagetAngle = rotateAngle;
        SetSelectJob();

    }
    /// <summary>
    /// 设置选择职业
    /// </summary>
    private void SetSelectJob()
    {
        for (int i = 0; i < m_JobLst.Count; i++)
        {
            if (m_JobLst[i].Id== m_CurrSelectJobId)
            {
                m_UISceneSelectRoleView.uISelectRoleJobDescription.SetUI(m_JobLst[i].Name,m_JobLst[i].Desc);

                break;

            }

        }
        for (int i = 0; i < m_UISceneSelectRoleView.JobItem.Length; i++)
        {
            m_UISceneSelectRoleView.JobItem[i].SetSelected(m_CurrSelectJobId);
        }
        

    }


    private void OnSelctRoleDrag(int obj)
    {

        if (m_IsRotateing) return;

        m_RotateAngle = Mathf.Abs((m_RotateAngle) * (obj == 0 ? 1 : -1));

        m_IsRotateing = true;

        m_TagetAngle = m_DragTarget.eulerAngles.y + m_RotateAngle;

        if (obj == 0)
        {
            m_CurrSelectJobId--;

            if (m_CurrSelectJobId <= 0)
            {
                m_CurrSelectJobId = 4;
            }

        }
        else
        {
            m_CurrSelectJobId++;
            if (m_CurrSelectJobId > 4)
            {
                m_CurrSelectJobId = 1;
            } 
        }
        SetSelectJob();


    }
    #endregion
    #endregion

    #region 已有角色相关

    private List<RoleOperation_LogOnGameServerReturnProto.RoleItem> m_RoleList;

    /// <summary>
    /// 根据角色编号获取已有角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    private RoleOperation_LogOnGameServerReturnProto.RoleItem GetRoleItems(int roleId)
    {
        if (m_RoleList != null)
        {
            for (int i = 0; i < m_RoleList.Count; i++)
            {
                if (m_RoleList[i].RoleId== roleId)
                {
                    return m_RoleList[i];
                }
            }
        }


        return default(RoleOperation_LogOnGameServerReturnProto.RoleItem);
    }
    /// <summary>
    /// 设置选择角色
    /// </summary>
    private void SetSelectRole(int jobId)
    {
        if (m_CurrSelectRoleId == jobId) return;
        m_CurrSelectRoleId = jobId;
        if (m_CurrSelectRoleModel != null)
        {
            Destroy(m_CurrSelectRoleModel);
        }

        RoleOperation_LogOnGameServerReturnProto.RoleItem item = GetRoleItems(jobId);

        if (CreateRoleContainers == null || CreateRoleContainers.Length < 4) return;


        m_CurrSelectRoleModel = Instantiate(GlobalInit.Instance.JobObjectDic[item.RoleJob]);

        m_CurrSelectRoleModel.transform.parent = CreateRoleContainers[0];

        m_CurrSelectRoleModel.transform.localPosition = Vector3.zero;
        m_CurrSelectRoleModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        m_CurrSelectRoleModel.transform.localScale = Vector3.one;

        RoleCtrl roleCtrl = m_CurrSelectRoleModel.GetComponent<RoleCtrl>();


    }

    /// <summary>
    /// 设置已有角色回调
    /// </summary>
    /// <param name="roleId"></param>
    private void SelectRoleCallBack(int roleId)
    {

        SetSelectRole(roleId);
        m_UISceneSelectRoleView.SetSelected(roleId);
    }



    /// <summary>
    /// 删除角色按钮回调
    /// </summary>
    private void OnBtnDeleteRoleClick()
    {
        m_UISceneSelectRoleView.DeleteSelectRole(GetRoleItems(m_CurrSelectRoleId).RoleNickName, OnDeleteRoleCallBack);
    }
    /// <summary>
    /// 删除角色回调
    /// </summary>
    private void OnDeleteRoleCallBack()
    {
        //DebugApp.Log("删除成功");
        RoleOperation_DeleteRoleProto proto = new RoleOperation_DeleteRoleProto();
        proto.RoleId = m_CurrSelectRoleId;
        //proto.RoleId = 7;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());

    }

    /// <summary>
    ///已有角色  服务器返回删除角色消息
    /// </summary>
    /// <param name="p"></param>
    private void DeleteRoleReturn(byte[] p)
    {
        RoleOperation_DeleteRoleReturnProto proto = RoleOperation_DeleteRoleReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            DebugApp.Log("删除角色成功");
            DeleteRole(m_CurrSelectRoleId);
            m_UISceneSelectRoleView.CloseDeleteRoleView();
        }
        else
        {
            MessageCtrl.Instance.Show("提示", "删除角色失败");
        }
    }
    /// <summary>
    /// 从本地列表 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    private void DeleteRole(int roleId)
    {
        for (int i = m_RoleList.Count-1; i>=0; i--)
        {
            if (m_RoleList[i].RoleId==roleId)
            {
                m_RoleList.RemoveAt(i);
            }
        }
       
        if (m_RoleList.Count==0)
        {


            ToCreateRoleUI();

        }
        else
        {
            m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
            SetSelectRole(m_RoleList[0].RoleId);
            m_UISceneSelectRoleView.SetSelected(m_RoleList[0].RoleId);
        }
    }
    private void ToCreateRoleUI()
    {
        m_UISceneSelectRoleView.ClearRoleListUI();
        if (m_CurrSelectRoleModel != null)
        {
            Destroy(m_CurrSelectRoleModel);
        }

        //切换新建角色 

        //新建角色
        isCreateRole = true;
        m_UISceneSelectRoleView.SetCreateRoleUI(true);
        m_UISceneSelectRoleView.SetUISelectRoleActive(false);
        SetCreateRoleModel(true);
        CloneCreateRole();
        m_CurrSelectJobId = 1;
        SetSelectJob();

        m_UISceneSelectRoleView.RandomName();
    }

    #endregion
    #region 进入游戏相关
    /// <summary>
    ///已有角色  服务器返回进入游戏消息
    /// </summary>
    /// <param name="p"></param>
    private void EnterGameReturn(byte[] p)
    {

        RoleOperation_EnterGameReturnProto proto = RoleOperation_EnterGameReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            DebugApp.Log("进入游戏成功");
        }
        else
        {
            MessageCtrl.Instance.Show("提示", "进入游戏失败");
        }

    }
    #endregion

    private void Update()
    {
        if (m_IsRotateing)
        {
            float toAgle = Mathf.MoveTowardsAngle(m_DragTarget.eulerAngles.y, m_TagetAngle, m_RotateSpeed);

            m_DragTarget.eulerAngles = toAgle * Vector3.up;

            if (Mathf.RoundToInt(m_TagetAngle) == Mathf.RoundToInt(toAgle)|| Mathf.RoundToInt(m_TagetAngle+360) == Mathf.RoundToInt(toAgle))
            {
                m_IsRotateing = false;

                m_DragTarget.eulerAngles = Vector3.up * m_TagetAngle;
               
            }

        }
    }
    private void OnDestroy()
    {
        //服务器返回登录信息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleOperation_LogOnGameServerReturn, LogOnGameServerReturn);
        //服务器返回创建信息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleOperation_CreateRoleReturn, CreateRoleReturn);
        //服务器返回进入游戏信息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleOperation_EnterGameReturn, EnterGameReturn);
        //服务器返回删除角色信息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleOperation_DeleteRoleReturn, DeleteRoleReturn);
        //服务器返回角色信息
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleOperation_SelectRoleInfoReturn, SelectRoleInfoReturn);
        //服务器返回角色学会的技能
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.RoleData_SkillReturn, RoleData_SkillReturn);
    }
}
