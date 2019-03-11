using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : SingletonMono<TimeMgr>
{
    /// <summary>
    /// 是否在缩放中
    /// </summary>
    private bool isTimeScale;
    /// <summary>
    /// 结束的时间
    /// </summary>
    private float timeScaleEndTime = 0;
    /// <summary>
    /// 修改时间缩放
    /// </summary>
    /// <param name="toTimeScale">缩放的值</param>
    /// <param name="delayTime">持续时间</param>
    public void ChangeTimeScale(float toTimeScale,float delayTime)
    {
        isTimeScale = true;
        Time.timeScale = toTimeScale;
        timeScaleEndTime = Time.realtimeSinceStartup+ delayTime;
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (isTimeScale)
        {
            if (Time.realtimeSinceStartup>timeScaleEndTime)
            {
                Time.timeScale = 1;
            }

        }
    }
}
