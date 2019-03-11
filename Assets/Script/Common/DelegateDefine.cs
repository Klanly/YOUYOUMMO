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
    /// 窗口显示
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageShow();

    /// <summary>
    /// 窗口隐藏或者
    /// </summary>
    [CSharpCallLua]
    public delegate void OnViewHide();

    /// <summary>
    /// 点击OK按钮
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageOK();

    /// <summary>
    /// 点击取消按钮
    /// </summary>
    [CSharpCallLua]
    public delegate void OnMessageCancel();

    /// <summary>
    /// 点击确认选择数量按钮
    /// </summary>
    /// <param name="value"></param>
    [CSharpCallLua]
    public delegate void OnChooseCount(int value);
}
