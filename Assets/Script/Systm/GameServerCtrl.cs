using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 区服控制器
/// </summary>
public class GameServerCtrl : SystmCtrlBase<GameServerCtrl>, ISystemCtrl
{
    private UIGameServerEnterView m_GameServerEnterView;

    private UIGameServerSelectView m_GameServerSelectView;

    private Dictionary<int, List<RetGameServerEntity>> m_GamerServerDic = new Dictionary<int, List<RetGameServerEntity>>();

    private int m_CurrClickPageIndex=0;

    private bool isBusy;

    public string EnemyNickName;

    public GameServerCtrl()
    {
        AddEventListener(ConstDefine.UIGameServerEnterView_btnEnterGameServer, UIGameServerEnterViewBtnEnterOnClick);
        AddEventListener(ConstDefine.UIGameServerEnterView_btnSelectServer, UIGameServerEnterViewBtnSelectOnClick);
        NetWorkSocket.Instance.OnConectOk = OnConectOkCallBack;
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.System_ServerTimeReturn, OnSystemServerTimeReturn);
    }

    /// <summary>
    /// 服务器返回服务器时间
    /// </summary>
    /// <param name="p"></param>
    private void OnSystemServerTimeReturn(byte[] p)
    {
        System_ServerTimeReturnProto proto = System_ServerTimeReturnProto.GetProto(p);
        float localTime = proto.LocalTime;
        long serverTime = proto.ServerTime;

        Log("收到服务器时间="+serverTime.ToString());

        GlobalInit.Instance.PingValue = (int)((Time.realtimeSinceStartup * 1000 - localTime) * 0.5f);//ping值
        GlobalInit.Instance.GameServerTime = serverTime - GlobalInit.Instance.PingValue;//客户端计算出的服务器时间
        Log("PINGValue=" + GlobalInit.Instance.PingValue.ToString());
        Log("GameServerTime=" + GlobalInit.Instance.GameServerTime.ToString());

        //更新最后登录服务器
        UpdateLastLogOnServer(GlobalInit.Instance.CurrAccount, GlobalInit.Instance.CurrSelectGameServer);    
        SceneMgr.Instance.LoadToSelectRole();
        Log("连接服务器成功");
    }



    /// <summary>
    /// 连接服务器成功回调
    /// </summary>
    private void OnConectOkCallBack()
    {
        //和服务器对表
        //客户端发送本地时间
        System_SendLocalTimeProto proto = new System_SendLocalTimeProto();
        proto.LocalTime = Time.realtimeSinceStartup * 1000;
        GlobalInit.Instance.CheckServerTime = Time.realtimeSinceStartup;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());   
    }

    /// <summary>
    /// 进入游戏按钮事件
    /// </summary>
    /// <param name="p"></param>
    private void UIGameServerEnterViewBtnEnterOnClick(object[] p)
    {
        //开始连接服务器
        NetWorkSocket.Instance.Connect(GlobalInit.Instance.CurrSelectGameServer.Ip, GlobalInit.Instance.CurrSelectGameServer.Port);


        
    }
    /// <summary>
    /// 选择区服按钮事件
    /// </summary>
    /// <param name="p"></param>
    private void UIGameServerEnterViewBtnSelectOnClick(object[] p)
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameServerSelect.ToString(), (GameObject obj) =>
        {
            m_GameServerSelectView = obj.GetComponent<UIGameServerSelectView>();
            m_GameServerSelectView.SetSelectUI(GlobalInit.Instance.CurrSelectGameServer);
            //当视图打开 ，获取数据
            GetGameServerPage();
           
        });

        //m_GameServerSelectView = UIViewUtil.Instance.LoadWindow(WindowUIType.GameServerSelect,()=>{
        //    m_GameServerSelectView.SetSelectUI(GlobalInit.Instance.CurrSelectGameServer);

        //    //当视图打开 ，获取数据
        //    GetGameServerPage();
           

        //}).GetComponent<UIGameServerSelectView>();

        m_GameServerSelectView.OnPageClick = OnPageClick;
        m_GameServerSelectView.OnGameServerClick = OnGameServerClick;

    }
    /// <summary>
    /// 服务器点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnGameServerClick(RetGameServerEntity obj)
    {
        m_GameServerSelectView.Close();

        GlobalInit.Instance.CurrSelectGameServer = obj;
        if (m_GameServerSelectView!=null)
        {
            m_GameServerEnterView.SetUI(GlobalInit.Instance.CurrSelectGameServer.Name);
        }


        DebugApp.Log("已选择"+obj.Name);
    }
    /// <summary>
    /// 页签点击
    /// </summary>
    /// <param name="pageIndex"></param>
    private void OnPageClick(int pageIndex)
    {
        GetGameServer(pageIndex);
    }

    /// <summary>
    /// 获取页签
    /// </summary>
    public void GetGameServerPage()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 0;
        dic["ChannelId"] = GlobalInit.ChannelId;
        dic["InnerVersion"] = GlobalInit.InnerVersion;
        NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/GameServer", OnGetServerPageCallBack, isPost: true, dic: dic);

    }
    /// <summary>
    /// 获取区服
    /// </summary>
    public void GetGameServer(int pageIndex)
    {
        if (m_GamerServerDic.ContainsKey(pageIndex))
        {
            if (m_GameServerSelectView != null)
            {
                m_GameServerSelectView.SetGameServerUI(m_GamerServerDic[pageIndex]);
            }
            return;
        }

        m_CurrClickPageIndex = pageIndex;

        if (isBusy) return;
        isBusy = true;

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 1;
        dic["ChannelId"] = GlobalInit.ChannelId;
        dic["InnerVersion"] = GlobalInit.InnerVersion;
        dic["pageIndex"] = pageIndex;
        NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/GameServer", OnGetServerCallBack, isPost: true, dic: dic);

    }
    /// <summary>
    /// 获取区服回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetServerCallBack(CallBackArgs obj)
    {
        isBusy = false;
        if (obj.HasError)
        {
            LogError(obj.ErrorMsg);
        }
        else
        {


            List<RetGameServerEntity> lst = JsonMapper.ToObject<List<RetGameServerEntity>>(obj.Value);
            m_GamerServerDic[m_CurrClickPageIndex] = lst;
              if (m_GameServerSelectView != null)
                {
                    m_GameServerSelectView.SetGameServerUI(lst);
                }
           


        }
    }


    /// <summary>
    /// 获取区服页签回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetServerPageCallBack(CallBackArgs obj)
    {
        if (obj.HasError)
        {
            LogError(obj.ErrorMsg);
        }
        else
        {


            List<RetGameServerPageEntity> lst = JsonMapper.ToObject<List<RetGameServerPageEntity>>(obj.Value);
            if (m_GameServerSelectView != null)
            {
                lst.Insert(0, new RetGameServerPageEntity() { Name = "推荐区服", PageIndex = 0 });
                m_GameServerSelectView.SetGameServerPageUI(lst);
                GetGameServer(0);
            }
       


        }
    }
    /// <summary>
    /// 更新登录最后服务器
    /// </summary>
    /// <param name="currAccount"></param>
    /// <param name="currGameServer"></param>
    private void UpdateLastLogOnServer(RetAccountEntity currAccount,RetGameServerEntity currGameServer)
    {

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 2;
        dic["userId"] = currAccount.Id;
        dic["lastServerId"] = currGameServer.Id;
        dic["lastServerName"] = currGameServer.Name;
        dic["lastServerPort"] = currGameServer.Port;

        NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/GameServer", OnUpdateLastLogOnServerCallBack, isPost: true, dic: dic);

    }

    private void OnUpdateLastLogOnServerCallBack(CallBackArgs obj)
    {
        if (obj.HasError)
        {
            LogError(obj.ErrorMsg);
        }
        else
        {
            Log("更新最后登录完毕");
        }
    }

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.GameServerEnter:
                OpenGameServerEnterView();
                break;
            case WindowUIType.GameServerSelect:
                break;
            default:
                break;
        }
    }

    #region 打开视图
    /// <summary>
    /// 打开游戏进入视图
    /// </summary>
    public void OpenGameServerEnterView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameServerEnter.ToString(), (GameObject oj) =>
        {
            m_GameServerEnterView = oj.GetComponent<UIGameServerEnterView>();
            m_GameServerEnterView.SetUI(GlobalInit.Instance.CurrSelectGameServer.Name);
        });

        //m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameServerEnter, () => {
        //    m_GameServerEnterView.SetUI(GlobalInit.Instance.CurrSelectGameServer.Name);

        //}
        //    ).GetComponent<UIGameServerEnterView>();
        //m_GameServerEnterView.OnViewClose = () =>
        //{

        //   // OpenRegView();
        //};



    }


    #endregion
    #region 释放
    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UIGameServerEnterView_btnEnterGameServer, UIGameServerEnterViewBtnEnterOnClick);
        RemoveEventListener(ConstDefine.UIGameServerEnterView_btnSelectServer, UIGameServerEnterViewBtnSelectOnClick);
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.System_ServerTimeReturn, OnSystemServerTimeReturn);

    }
    #endregion
}
