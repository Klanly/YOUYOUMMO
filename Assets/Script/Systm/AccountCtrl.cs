using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 账号系统控制器
/// </summary>
public class AccountCtrl : SystmCtrlBase<AccountCtrl>, ISystemCtrl
{
    #region 属性
    /// <summary>
    /// 登录窗口视图
    /// </summary>
    private UILogOnView m_LogOnView;

    /// <summary>
    /// 注册窗口视图
    /// </summary>
    private UIRegView m_RegView;
    #endregion

    #region 构造函数
    /// <summary>
    /// 构造
    /// </summary>
    public AccountCtrl()
    {
        AddEventListener(ConstDefine.UILogOnView_btnLogOn, UILogOnViewBtnLogOnClick);
        AddEventListener(ConstDefine.UILogOnView_btnToReg, UILogOnViewBtnRegOnClick);
        
        AddEventListener(ConstDefine.UIRegView_btnToLogOn, UIRegViewBtnToLogOn);
        AddEventListener(ConstDefine.UIRegView_btnReg, UIRegViewBtnReg);
    }
    #endregion
    private bool m_IsAutoLogOn;
    /// <summary>
    /// 快速登录
    /// </summary>
    public void QuickLogOn()
    {
        //1.首先判断本地账号
        //2.如果本地没有账号，则进入注册View
        //3.如果本地有，则自动登录 登录成功后进入游戏的view

        if (!PlayerPrefs.HasKey(ConstDefine.LogOn_AccountID))
        {
            this.OpenView(WindowUIType.Reg);
        }

        else
        {
            m_IsAutoLogOn = true;
            //自动登录
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Type"] = 1;
            dic["UserName"] = PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName);
            dic["Pwd"] = PlayerPrefs.GetString(ConstDefine.LogOn_AccountPwd);

            NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", OnLogOnCallBack, isPost: true, dic: dic);
        }
        


    }


    #region  UIRegView 注册窗口视图按钮
    /// <summary>
    /// 注册点击
    /// </summary>
    /// <param name="param"></param>
    private void UIRegViewBtnReg(object[] param)
    {
        //Debug.Log("注册点击");
        if (string.IsNullOrEmpty(m_RegView.txtAccount.text))
        {

            ShowMessage("注册提示", "请输入账号");

            return;
            
        }
        if (string.IsNullOrEmpty(m_RegView.txtPwd.text))
        {
            ShowMessage("注册提示", "请输入密码");
            return;

        }
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 0;
        dic["UserName"] = m_RegView.txtAccount.text;
        dic["Pwd"] = m_RegView.txtPwd.text;
        dic["ChannelId"] = 1;

        NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", OnRegCallBack, isPost: true, dic:dic);

    }
    /// <summary>
    /// 注册回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnRegCallBack(CallBackArgs obj)
    {
        if (obj.HasError)
        {
            LogError(obj.ErrorMsg);
        }
        else
        {
            RetValue ret = JsonMapper.ToObject<RetValue>(obj.Value);
            if (ret.HasError)
            {
                LogError(ret.ErrorMsg);
            }
            else
            {
                Log("注册成功" + ret.Value);
                RetAccountEntity entity = JsonMapper.ToObject<RetAccountEntity>(ret.Value.ToString());
                GlobalInit.Instance.CurrAccount = entity;

                SerCurrSelectGameServer(entity);

                Stat.LogOn(entity.Id, m_RegView.txtAccount.text);
                //本地存储
                PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, entity.Id);
                PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_RegView.txtAccount.text);
                PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_RegView.txtPwd.text);

                m_RegView.CloseAndOpenNext(WindowUIType.GameServerEnter);
               
            }

            
        }
    }

    /// <summary>
    /// 返回登录点击事件
    /// </summary>
    /// <param name="param"></param>
    private void UIRegViewBtnToLogOn(object[] param)
    {
        Log("返回登录点击");


        m_RegView.CloseAndOpenNext(WindowUIType.LogOn);
    }
    #endregion

    #region  UILogOnView 登录窗口视图按钮
    /// <summary>
    /// 去注册按钮点击事件
    /// </summary>
    /// <param name="param"></param>
    private void UILogOnViewBtnRegOnClick(object[] param)
    {
        Log("现在注册点击");
        m_LogOnView.CloseAndOpenNext(WindowUIType.Reg);


       
    }
    /// <summary>
    /// 登录按钮点击事件
    /// </summary>
    /// <param name="param"></param>
    private void UILogOnViewBtnLogOnClick(object[] param)
    {
        Log("登录点击");

        if (string.IsNullOrEmpty(m_LogOnView.txtUserName.text))
        {
            Log("请输入账号");
            return;

        }
        if (string.IsNullOrEmpty(m_LogOnView.txtPwd.text))
        {
           Log("请输入密码");
            return;

        }
        m_IsAutoLogOn = false;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 1;
        dic["UserName"] = m_LogOnView.txtUserName.text;
        dic["Pwd"] = m_LogOnView.txtPwd.text;

        NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", OnLogOnCallBack, isPost: true, dic:dic);

    }

    /// <summary>
    /// 设置当前服务器
    /// </summary>
    /// <param name="entity"></param>
    private void SerCurrSelectGameServer(RetAccountEntity entity)
    {
        RetGameServerEntity currGameServerEntity = new RetGameServerEntity();
        currGameServerEntity.Id = entity.LastServerId;
        currGameServerEntity.Name = entity.LastServerName;
        currGameServerEntity.Ip = entity.LastServerIp;
        currGameServerEntity.Port = entity.LastServerPort;

        GlobalInit.Instance.CurrSelectGameServer = currGameServerEntity;
    }


    /// <summary>
    /// 登录成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnLogOnCallBack(CallBackArgs obj)
    {
        if (obj.HasError)
        {
           LogError(obj.ErrorMsg);
        }
        else
        {
            RetValue ret = JsonMapper.ToObject<RetValue>(obj.Value);
            if (ret.HasError)
            {
               LogError(ret.ErrorMsg);
            }
            else
            {
                Log("登录成功" + ret.Value);

                RetAccountEntity entity = JsonMapper.ToObject<RetAccountEntity>(ret.Value.ToString());
                GlobalInit.Instance.CurrAccount = entity;
                SerCurrSelectGameServer(entity);

                string userName = string.Empty;
                if (m_IsAutoLogOn)
                {
                    userName =PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName);
                    UIViewMgr.Instance.OpenWindow(WindowUIType.GameServerEnter);
                }
                else
                {
                    //本地存储
                    PlayerPrefs.SetInt(ConstDefine.LogOn_AccountID, entity.Id);
                    PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_LogOnView.txtUserName.text);
                    PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_LogOnView.txtPwd.text);

                    userName = m_LogOnView.txtUserName.text;
                    m_LogOnView.CloseAndOpenNext(WindowUIType.GameServerEnter);
                }


                Stat.LogOn(entity.Id, userName);

              
            }


        }
    }
    #endregion

    #region 打开视图
    /// <summary>
    /// 打开登录视图
    /// </summary>
    public void OpenLogOnView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.LogOn.ToString(),(GameObject obj)=>
        {
            m_LogOnView= obj.GetComponent<UILogOnView>();
        });
       // m_LogOnView =UIViewUtil.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
    }

    /// <summary>
    /// 打开注册视图 
    /// </summary>
    public void OpenRegView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.Reg.ToString(), (GameObject obj) =>
        {
            m_RegView = obj.GetComponent<UIRegView>();
        });
        //m_RegView = UIViewUtil.Instance.OpenWindow(WindowUIType.Reg).GetComponent<UIRegView>();
    }
    #endregion

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.LogOn:
                OpenLogOnView();
                break;
            case WindowUIType.Reg:
                OpenRegView();
                break;
        }
    }

    #region 释放
    public override void Dispose()
    {
        base.Dispose();

       RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, UILogOnViewBtnLogOnClick);
       RemoveEventListener(ConstDefine.UILogOnView_btnToReg, UILogOnViewBtnRegOnClick);
       RemoveEventListener(ConstDefine.UIRegView_btnToLogOn, UIRegViewBtnToLogOn);
       RemoveEventListener(ConstDefine.UIRegView_btnReg, UIRegViewBtnReg);
    }
#endregion
}
