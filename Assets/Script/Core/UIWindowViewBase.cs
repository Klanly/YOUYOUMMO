using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 所有窗口UI的基类
/// </summary>
public class UIWindowViewBase : UIViewBase
{
    /// <summary>
    /// 挂点类型
    /// </summary>
    [SerializeField]
    public WindowUIContainerType containerType = WindowUIContainerType.Center;

    /// <summary>
    /// 打开方式
    /// </summary>
    [SerializeField]
    public WindowShowStyle showStyle = WindowShowStyle.Normal;

    /// <summary>
    /// 打开或关闭动画效果持续时间
    /// </summary>
    [SerializeField]
    public float duration = 0.2f;

    /// <summary>
    /// 视图的名称
    /// </summary>
    [HideInInspector]
    public string ViewName;

    /// <summary>
    /// 下一个要打开的窗口
    /// </summary>
    public WindowUIType m_NextOpenWindow = WindowUIType.None;




  
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name.Equals("btnClose",StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public virtual void Close()
    {
        AudioEffectMgr.Instance.PlayUIAudioEffect(UIAudioEffectType.UIClose);
        UIViewUtil.Instance.CloseWindow(ViewName);
    }

    /// <summary>
    /// 关闭窗口打开下一个
    /// </summary>
    public virtual void CloseAndOpenNext(WindowUIType next)
    {
        this.Close();

        m_NextOpenWindow = next;
   

    }



    /// <summary>
    /// 销毁之前执行
    /// </summary>
    protected override void BeforeOnDestroy()
    {
        LayerUIMgr.Instance.CheckOpenWindow();
        if (m_NextOpenWindow!=WindowUIType.None)
        {
            UIViewMgr.Instance.OpenWindow(m_NextOpenWindow);
   
        }



        //if (NextOpenWindow == WindowUIType.None) return;
        //WindowUIMgr.Instance.OpenWindow(NextOpenWindow);
    }
}