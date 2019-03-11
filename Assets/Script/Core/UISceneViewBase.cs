using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 场景UI的基类
/// </summary>
public class UISceneViewBase : UIViewBase
{
    /// <summary>
    /// 当前画布
    /// </summary>
    public Canvas m_CurrCanvas;
    /// <summary>
    /// 容器_居中
    /// </summary>
    [SerializeField]
    public Transform Container_Center;
    public bl_HUDText HUDText;

    /// <summary>
    /// 加载完毕
    /// </summary>
    public Action OnloadComplete;
}