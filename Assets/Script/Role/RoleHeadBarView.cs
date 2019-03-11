using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleHeadBarView : MonoBehaviour
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;
    /// <summary>
    /// 血条
    /// </summary>
    [SerializeField]
    private Slider sliderHp;
    /// <summary>
    /// 对齐的目标点
    /// </summary>
    private Transform m_Target;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = UISceneCtrl.Instance.CurrentUIScene.m_CurrCanvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rectTransform == null || m_Target == null|| UI_Camera.Instance.Camera==null) return;

        //世界左边点 转换成视口坐标
        Vector2 pos = Camera.main.WorldToScreenPoint(m_Target.position);

        //转换成UI摄像机的世界坐标
        Vector3 uiPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, pos,UI_Camera.Instance.Camera,out uiPos))
        {
           transform.position = uiPos;
        }

     
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="nickName"></param>
    /// <param name="isShowHPBar">是否显示血条</param>
    public void Init(Transform target, string nickName, bool isShowHPBar = false,float sliderHpvalue=1)
    {
        m_Target = target;
        lblNickName.text = nickName;
        sliderHp.gameObject.SetActive(isShowHPBar);
        sliderHp.value = sliderHpvalue;


    }

    /// <summary>
    /// 设置血条
    /// </summary>
    /// <param name="sliderHpvalue"></param>
    public void SetSliderHP(float sliderHpvalue)
    {
        sliderHp.value = sliderHpvalue;
    }

    /// <summary>
    /// 上弹伤害文字
    /// </summary>
    /// <param name="hurtValue"></param>
    //public void Hurt(int hurtValue, float pbHPValue = 0)
    //{
    //    hudText.Add(string.Format("-{0}", hurtValue), Color.red, 0.1f);
    //    pbHP.value = pbHPValue;
    //}
}