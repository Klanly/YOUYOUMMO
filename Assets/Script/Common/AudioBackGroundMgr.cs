//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2017-05-07 16:15:46
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 背景音乐管理器（支持音乐的淡入淡出）
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioBackGroundMgr : MonoBehaviour
{
    private AudioSource m_AudioSource; //音源
    private AudioClip m_PrevAudioClip; //上一个播放的背景音乐
    private string m_AudioName; //播放的背景音乐名称
    private float m_MaxVolume = 0.2f;//最大音量

    public static AudioBackGroundMgr Instance;

    void Awake()
    {
        Instance = this;

        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.volume = 0f; //音量
        m_AudioSource.loop = true; //是否循环
        m_AudioSource.spatialBlend = 0f; //2D音乐

        DontDestroyOnLoad(gameObject); //不销毁
    }

    void Start()
    {

    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        m_AudioName = name;

        StartCoroutine(DoPlay());
    }

    private IEnumerator DoPlay()
    {
        float fadeOut = 1;//淡出需要多少秒
        float fadeIn = 1; //淡入需要多少秒
        float delay = 0; //延迟时间

        AudioClip audioClip = null;

        audioClip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(string.Format("Assets/Download/Audio/BackGround/{0}.mp3", m_AudioName));

        if (m_AudioSource.isPlaying && m_AudioSource.clip == audioClip)
        {
            yield return 0;
        }
        else
        {
            float time1 = Time.time;
            //开始播放
            //检查是否有未完成的要播放的音乐
            if (m_PrevAudioClip != null)
            {
                //先把上一个音乐 淡出
                yield return StartCoroutine(StartFadeOut(fadeOut));
            }

            //设置延迟
            float time2 = Time.time - time1;
            if (delay > time2)
            {
                yield return new WaitForSeconds(delay - time2);
            }

            //播放音乐
            m_AudioSource.clip = audioClip;
            m_PrevAudioClip = audioClip;
            m_AudioSource.Play();

            //声音淡入
            yield return StartCoroutine(StartFadeIn(fadeIn));
        }
    }


    /// <summary>
    /// 声音淡出
    /// </summary>
    /// <param name="fadeOut"></param>
    /// <returns></returns>
    private IEnumerator StartFadeOut(float fadeOut)
    {
        float time = 0f;
        while (time <= fadeOut)
        {
            if (time != 0)
            {
                m_AudioSource.volume = Mathf.Lerp(m_MaxVolume, 0f, time / fadeOut);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        m_AudioSource.volume = 0;
    }

    /// <summary>
    /// 声音淡入
    /// </summary>
    /// <param name="fadeIn"></param>
    /// <returns></returns>
    private IEnumerator StartFadeIn(float fadeIn)
    {
        float time = 0f;
        while (time <= fadeIn)
        {
            if (time != 0)
            {
                m_AudioSource.volume = Mathf.Lerp(0f, m_MaxVolume, time / fadeIn);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        m_AudioSource.volume = m_MaxVolume;
    }
}