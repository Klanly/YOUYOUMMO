using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 
/// </summary>
public class CameraCtrl : MonoBehaviour 
{
    public static CameraCtrl Instance;

    /// <summary>
    /// 控制摄像机上下
    /// </summary>
    [SerializeField]
    private Transform m_CameraUpAndDown;

    /// <summary>
    /// 摄像机缩放父物体
    /// </summary>
    [SerializeField]
    private Transform m_CameraZoomContainer;

    /// <summary>
    /// 摄像机容器
    /// </summary>
    [SerializeField]
    private Transform m_CameraContainer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        m_CameraUpAndDown.transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(m_CameraUpAndDown.transform.localEulerAngles.z, 35f, 80f));
    }

    /// <summary>
    /// 设置摄像机旋转
    /// </summary>
    /// <param name="type">0=左 1=右</param>
    public void SetCameraRotate(int type)
    {
        transform.Rotate(0, 80 * Time.deltaTime * (type == 0 ? -1 : 1), 0);
    }

    /// <summary>
    /// 设置摄像机上下 0=上 1=下
    /// </summary>
    /// <param name="type"></param>
    public void SetCameraUpAndDown(int type)
    {
        m_CameraUpAndDown.transform.Rotate(0, 0, 60 * Time.deltaTime * (type == 1 ? -1 : 1));
        m_CameraUpAndDown.transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(m_CameraUpAndDown.transform.localEulerAngles.z, 35f, 80f));
    }

    /// <summary>
    /// 设置摄像机 缩放
    /// </summary>
    /// <param name="type">0=拉近 1=拉远</param>
    public void SetCameraZoom(int type)
    {
        m_CameraContainer.Translate(Vector3.forward * 10 * Time.deltaTime * ((type == 1 ? -1 : 1)));
        m_CameraContainer.localPosition = new Vector3(0, 0, Mathf.Clamp(m_CameraContainer.localPosition.z, -5f,5f));
    }

    /// <summary>
    /// 实时看着主角
    /// </summary>
    /// <param name="pos"></param>
    public void AutoLookAt(Vector3 pos)
    {
        m_CameraZoomContainer.LookAt(pos);
    }

    /// <summary>
    /// 摄像机震动
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="duration"></param>
    /// <param name="strength"></param>
    /// <param name="vibrato"></param>
    public void CameraShake(float delay = 0, float duration = 0.5f, float strength = 1, int vibrato = 10)
    {

        StartCoroutine(DoCameraShake(delay, duration, strength, vibrato));
    }

    private IEnumerator DoCameraShake(float delay=0,float duration=0.5f,float strength=1,int vibrato=10)
    {
        yield return new WaitForSeconds(delay);

        m_CameraContainer.DOShakePosition(0.3f,1,100);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 15f);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, 14f);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, 12f);
    //}
}