using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class OtherRoleAI : IRoleAI
{
    /// <summary>
    /// 目标点
    /// </summary>
    private Vector3 m_TagetPos;
    /// <summary>
    /// 服务器时间
    /// </summary>
    private long m_ServerTime;
    /// <summary>
    /// 所需时间
    /// </summary>
    private int m_NeedTime;

    public RoleCtrl CurrRole
    {
        get;      
        set;
        
    }
    public OtherRoleAI(RoleCtrl roleCtrl)
    {
        CurrRole = roleCtrl;
    }

    public void DoAI()
    {
       
    }
    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="tagetPos"></param>
    /// <param name="serverTime"></param>
    /// <param name="needTime"></param>
    public void MoveTo(Vector3 tagetPos, long serverTime, int needTime )
    {
        m_TagetPos = tagetPos;
        m_ServerTime = serverTime;
        m_NeedTime = needTime;

        CurrRole.Seeker.StartPath(CurrRole.transform.position, tagetPos, p => OnAStarFinish(p));
    }

    private void OnAStarFinish(Path p)
    {
        //路径长度
        float pathLen = GameUtil.GetPathLen(p.vectorPath);
        //这个包的延迟时间=当前服务器时间-协议发过来的服务器时间
        long delayTime = GlobalInit.Instance.GetCurrServerTime() - m_ServerTime;
        //实际给其他玩家的移动时间=移动所需时间-延迟时间
        long realMoveTime = m_NeedTime - delayTime;

        if (realMoveTime <= 0) realMoveTime = 100;

        CurrRole.ModifySpeed = Mathf.Clamp(pathLen / (realMoveTime * 0.001f), 0, 20f);
        Debug.Log("其他玩家要移动的目标点" + m_TagetPos);
        CurrRole.MoveTo(m_TagetPos);
    }
}
