using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystmCtrlBase<T> : IDisposable where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public virtual void Dispose()
    {

    }
    /// <summary>
    /// 显示提示消息窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="uiMessageType">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回到</param>

    protected void ShowMessage(string title, string message, MessageViewType uiMessageType = MessageViewType.Ok, DelegateDefine.OnMessageShow onShow = null, DelegateDefine.OnMessageOK okAction = null, DelegateDefine.OnMessageCancel cancelAction = null)
    {
        MessageCtrl.Instance.Show(title, message, uiMessageType, onShow, okAction, cancelAction);
    }
    protected void AddEventListener(string key, UIDispatcher.OnActionHandle handle)
    {
        UIDispatcher.Instance.AddEventListener(key, handle);
    }
    protected void RemoveEventListener(string key, UIDispatcher.OnActionHandle handle)
    {
        UIDispatcher.Instance.RemoveEventListener(key, handle);
    }

    protected void Log(object message)
    {
        DebugApp.Log(message);
    }
    protected void LogError(object message)
    {
        DebugApp.LogError(message);
    }
}
