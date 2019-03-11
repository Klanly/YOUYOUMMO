using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatcherBase<T,P,X> : IDisposable 
    where T : new()
    where P: class
{
    #region 单列
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
    #endregion


    /// <summary>
    /// 委托原型
    /// </summary>
    public delegate void OnActionHandle(P p);

    private Dictionary<X, List<OnActionHandle>> dic = new Dictionary<X, List<OnActionHandle>>();


    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void AddEventListener(X key, OnActionHandle handler)
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
    public void RemoveEventListener(X key, OnActionHandle handler)
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
    public void Dispatch(X key, P p)
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
                        lstHandle[i](p);
                    }
                }
            }
        }
    }

    public void Dispatch(X key)
    {
        Dispatch(key, null);
    }

}
