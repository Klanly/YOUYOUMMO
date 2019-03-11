using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class RoleAttackInfo
{
    /// <summary>
    /// ������
    /// </summary>
    public int Index;
    /// <summary>
    /// ����ID
    /// </summary>
    public int SkillId;
    /// <summary>
    /// ��Ч����(��ɫ������ս������ ʹ��)
    /// </summary>
    public string EffectName;

#if DEBUG_ROLESTATE
    /// <summary>
    /// ��ЧԤ��(�ڲ��Ի���ʹ��)
    /// </summary>
    public GameObject EffectObj;

#endif
    /// <summary>
    /// ��Ч���ʱ��
    /// </summary>
    public float EffectLifeTime=0;
    /// <summary>
    /// ������Χ
    /// </summary>
    public float AttackRange = 0f;
    /// <summary>
    /// �����ӳ�ʱ��
    /// </summary>
    public float HurtDelayTime = 0;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsShake = false;
    /// <summary>
    /// �����ӳ�ʱ��
    /// </summary>
    public float ShakeDelay=0;
    /// <summary>
    /// ��������
    /// </summary>
    public DelayAudioClip FireAudio;
    /// <summary>
    /// �����ߵĺ���
    /// </summary>
    public DelayAudioClip AttackRoleAudio;
}
