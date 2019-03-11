using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : SingletonMono<TimeMgr>
{
    /// <summary>
    /// �Ƿ���������
    /// </summary>
    private bool isTimeScale;
    /// <summary>
    /// ������ʱ��
    /// </summary>
    private float timeScaleEndTime = 0;
    /// <summary>
    /// �޸�ʱ������
    /// </summary>
    /// <param name="toTimeScale">���ŵ�ֵ</param>
    /// <param name="delayTime">����ʱ��</param>
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
