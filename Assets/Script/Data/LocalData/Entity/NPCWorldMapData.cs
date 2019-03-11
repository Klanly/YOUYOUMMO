//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-07-05 21:38:02
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

public class NPCWorldMapData
{
    /// <summary>
    /// NPC编号
    /// </summary>
    public int NPCId { get; set; }

    /// <summary>
    /// NPC的坐标
    /// </summary>
    public Vector3 NPCPostion { get; set; }

    /// <summary>
    /// Y轴的旋转角度
    /// </summary>
    public float EulerAnglesY { get; set; }

    /// <summary>
    /// 开场白
    /// </summary>
    public string Prologue { get; set; }
}