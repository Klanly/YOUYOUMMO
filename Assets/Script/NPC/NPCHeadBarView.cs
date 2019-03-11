using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHeadBarView : MonoBehaviour {

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;
    /// <summary>
    /// 对齐的目标点
    /// </summary>
    private Transform m_Target;

    private RectTransform rectTransform;
    [SerializeField]
    private Text lblTalkText;
    [SerializeField]
    private Image imgTalkBG;

    private Tween m_ScaleTween;

    private Tween m_RotaTween;

    private void Awake()
    {
        imgTalkBG.gameObject.SetActive(false);


    }

    void Start () {


        rectTransform = UISceneCtrl.Instance.CurrentUIScene.m_CurrCanvas.GetComponent<RectTransform>();
        imgTalkBG.transform.localScale = Vector3.zero;
        imgTalkBG.transform.localEulerAngles = new Vector3(0, 0, -10);
        m_ScaleTween = imgTalkBG.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(false).Pause().OnComplete(() =>
        {

            lblTalkText.DOText(m_Talk, 1f);

        }).OnRewind(() =>
        {
            imgTalkBG.gameObject.SetActive(false);
        });


        m_RotaTween = imgTalkBG.transform.DOLocalRotate(new Vector3(0, 0, 20), 1f, RotateMode.LocalAxisAdd).SetAutoKill(false).Pause().SetLoops(-1, LoopType.Yoyo);
   


    }
    //停止时间
    private float m_TalkStopTime = 0;
    //是否说话
    private bool m_IsTalk;
    //说话内容
    private string m_Talk;
    /// <summary>
    /// 开始说话
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    public void Talk(string text,float time)
    {

        m_TalkStopTime = Time.time + time;
        m_IsTalk = true;
        m_Talk = text;
        lblTalkText.text = "";

        imgTalkBG.gameObject.SetActive(true);

        m_ScaleTween.PlayForward();

        m_RotaTween.Play();

    }


	void Update () {
        if (rectTransform == null || m_Target == null || UI_Camera.Instance.Camera == null) return;

        //世界左边点 转换成视口坐标
        Vector2 pos = Camera.main.WorldToScreenPoint(m_Target.position);

        //转换成UI摄像机的世界坐标
        Vector3 uiPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, pos, UI_Camera.Instance.Camera, out uiPos))
        {
            transform.position = uiPos;
        }

        if (m_IsTalk&&Time.time>m_TalkStopTime)
        {
            m_IsTalk = false;
            m_ScaleTween.PlayBackwards();
        }

    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="target"></param>
    /// <param name="nickName"></param>
    /// <param name="isShowHPBar"></param>
    public void Init(Transform target, string nickName)
    {
        m_Target = target;
        lblNickName.text = nickName;

    }
}
