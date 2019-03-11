using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[XLua.LuaCallCSharp]
public class UIDispatcher : IDisposable
{
    #region 单列
    private static UIDispatcher instance;

    public static UIDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIDispatcher();
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
    [XLua.CSharpCallLua]
    public delegate void OnActionHandle(string[] buffer);

    private Dictionary<string, List<OnActionHandle>> dic = new Dictionary<string, List<OnActionHandle>>();


    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void AddEventListener(string key, OnActionHandle handler)
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
    public void RemoveEventListener(string key, OnActionHandle handler)
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
    public void Dispatch(string key, string[] buffer)
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

    public void Dispatch(string key)
    {
        Dispatch(key, null);
    }

}
