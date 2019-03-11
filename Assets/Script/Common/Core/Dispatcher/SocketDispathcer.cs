using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// ����ͨѶ Э��۲���
/// </summary>
[LuaCallCSharp]
public class SocketDispatcher : IDisposable 
{
    #region ����
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
    /// ί��ԭ��
    /// </summary>
    [CSharpCallLua]
    public delegate void OnActionHandle(byte[] buffer);

    private Dictionary<ushort, List<OnActionHandle>> dic = new Dictionary<ushort, List<OnActionHandle>>();


    /// <summary>
    /// ��Ӽ���
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
    /// �Ƴ�����
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
    /// �ɷ�
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
