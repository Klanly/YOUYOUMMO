using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// 网络通讯 协议观察者
/// </summary>
[LuaCallCSharp]
public class SocketDispatcher : IDisposable 
{
    #region 单列
    private static SocketDispatcher instance;

    public static SocketDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SocketDispatcher();
            }
            return instance;
        }
    }

    public virtual void Dispose()
    {

    }
    #endregion


    /// <summary>
    /// 委托原型
    /// </summary>
    [CSharpCallLua]
    public delegate void OnActionHandle(byte[] buffer);

    private Dictionary<ushort, List<OnActionHandle>> dic = new Dictionary<ushort, List<OnActionHandle>>();


    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void AddEventListener(ushort key, OnActionHandle handler)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(handler);
        }
        else
        {
            List<OnActionHandle> lstHandle = new List<OnActionHandle>();
            lstHandle.Add(handler);
            dic[key] = lstHandle;

        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void RemoveEventListener(ushort key, OnActionHandle handler)
    {
        if (dic.ContainsKey(key))
        {

            List<OnActionHandle> lstHandle = dic[key];
            lstHandle.Remove(handler);
            if (lstHandle.Count == 0)
            {
                dic.Remove(key);
            }

        }
    }

    /// <summary>
    /// 派发
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="buffer"></param>
    public void Dispatch(ushort key, byte[] buffer)
    {
        if (dic.ContainsKey(key))
        {
            List<OnActionHandle> lstHandle = dic[key];

            if (lstHandle != null && lstHandle.Count > 0)
            {
                for (int i = 0; i < lstHandle.Count; i++)
                {
                    if (lstHandle != null)
                    {
                        lstHandle[i](buffer);
                    }
                }
            }
        }
    }

    public void Dispatch(ushort key)
    {
        Dispatch(key, null);
    }

}
