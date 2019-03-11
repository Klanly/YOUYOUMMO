using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewMgr : Singleton<UIViewMgr>
{
    private Dictionary<WindowUIType, ISystemCtrl> m_SystemCtrlDic = new Dictionary<WindowUIType, ISystemCtrl>();


    public UIViewMgr()
    {
        m_SystemCtrlDic.Add(WindowUIType.LogOn, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.Reg, AccountCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameServerEnter, GameServerCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameServerSelect, GameServerCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.RoleInfo, PlayerCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameLevelMap, GameLevelCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.GameLevelDetail, GameLevelCtrl.Instance);
        m_SystemCtrlDic.Add(WindowUIType.WorldMap, WorldMapCtrl.Instance);
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <returns></returns>
    public void OpenWindow(WindowUIType type)
    {
        m_SystemCtrlDic[type].OpenView(type);
    }
}
