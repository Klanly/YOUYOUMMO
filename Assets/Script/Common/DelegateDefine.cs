using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class DelegateDefine : Singleton<DelegateDefine>
{
    public Action OnSenceLoadOk;
    public Action OnChannelInitOk;

    /// <summary>
    /// ������ʾ
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageShow();

    /// <summary>
    /// �������ػ���
    /// </summary>
    [CSharpCallLua]
    public delegate void OnViewHide();

    /// <summary>
    /// ���OK��ť
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageOK();

    /// <summary>
    /// ���ȡ����ť
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageCancel();

    /// <summary>
    /// ���ȷ��ѡ��������ť
    /// </summary>
    /// <param name="value"></param>
    [CSharpCallLua]
    public delegate void OnChooseCount(int value);
}
