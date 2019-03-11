//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-06-28 22:38:20
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMainCityRoleInfoView : MonoBehaviour
{
    /// <summary>
    /// 头像
    /// </summary>
    [SerializeField]
    private Image imgHeadPic;

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

    /// <summary>
    /// 等级
    /// </summary>
    [SerializeField]
    private Text lblLV;

    /// <summary>
    /// 元宝
    /// </summary>
    [SerializeField]
    private Text lblMoney;

    /// <summary>
    /// 金币
    /// </summary>
    [SerializeField]
    private Text lblGold;

    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    private Slider sliderHP;

    /// <summary>
    /// MP
    /// </summary>
    [SerializeField]
    private Slider sliderMP;

    public static UIMainCityRoleInfoView Instance;
    private void Awake()
    {
        Instance = this;
    }


    public void SetUI(string headPic, string nickName, int level, int money, int gold, int currHP, int maxHP, int currMP, int maxMP)
    {
        lblNickName.text = nickName;
        lblLV.text = string.Format("LV.{0}", level);

        lblNickName.text = (nickName);
        lblLV.text = string.Format("LV.{0}", level);
        lblMoney.text = money.ToString();
        lblGold.text = gold.ToString();
        sliderHP.value = (float)currHP / maxHP;
        sliderMP.value = (float)currMP / maxMP;

        AssetBundleMgr.Instance.LoadOrDownload<Texture2D>(string.Format("Download/Source/UISource/UI/HeadImg/{0}.assetbundle", headPic), headPic, (Texture2D obj) =>
        {
            if (obj == null) return;
            var iconRect = new Rect(0, 0, obj.width, obj.height);
            var iconSprite = Sprite.Create(obj, iconRect, new Vector2(.5f, .5f));

            imgHeadPic.overrideSprite = iconSprite;

          

        }, type: 1);

       // imgHeadPic.overrideSprite = RoleMgr.Instance.LoadHeadPic(headPic);

       
    }

    /// <summary>
    /// 设置HP
    /// </summary>
    /// <param name="currHP"></param>
    /// <param name="maxHP"></param>
    public void SetHP(int currHP, int maxHP)
    {
        sliderHP.value = (float)currHP / maxHP;

    }
    /// <summary>
    /// 设置MP
    /// </summary>
    /// <param name="currHP"></param>
    /// <param name="maxHP"></param>
    public void SetMP(int currMP, int maxMP)
    {
        sliderMP.value = (float)currMP / maxMP;

    }
    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="currHP"></param>
    /// <param name="maxHP"></param>
    public void SetMoney(int money)
    {
        GameUtil.AutoNumberAnimation(lblMoney.gameObject,money);

    }



    //protected override void OnAwake()
    //{
    //    base.OnAwake();
    //    Instance = this;
    //}

    //public void SetUI(string headPic, string nickName, int level, int money, int gold, int currHP, int maxHP, int currMP, int maxMP)
    //{

    //    AssetBundleMgr.Instance.LoadOrDownload<Texture2D>(string.Format("Download/Source/UISource/HeadImg/{0}.assetbundle", headPic), headPic, (Texture2D obj) =>
    //    {
    //        if (obj == null) return;
    //        var iconRect = new Rect(0, 0, obj.width, obj.height);
    //        var iconSprite = Sprite.Create(obj, iconRect, new Vector2(.5f, .5f));

    //        imgHeadPic.overrideSprite = iconSprite;
    //    }, type: 1);

    //    lblNickName.SetText(nickName);
    //    lblLV.SetText(string.Format("LV.{0}", level));

    //    lblNickName.SetText(nickName);
    //    lblLV.SetText(string.Format("LV.{0}", level));
    //    lblMoney.SetText(money.ToString());
    //    lblGold.SetText(gold.ToString());
    //    sliderHP.SetSliderValue((float)currHP / maxHP);
    //    sliderMP.SetSliderValue((float)currMP / maxMP);
    //}

    //public void SetHP(int currHP, int maxHP)
    //{
    //    sliderHP.SetSliderValue((float)currHP / maxHP);
    //}

    //public void SetMP(int currMP, int maxMP)
    //{
    //    sliderMP.SetSliderValue((float)currMP / maxMP);
    //}

    //protected override void BeforeOnDestroy()
    //{
    //    base.BeforeOnDestroy();

    //    imgHeadPic = null;
    //    lblNickName = null;
    //    lblLV = null;
    //    lblMoney = null;
    //    lblGold = null;
    //    sliderHP = null;
    //    sliderMP = null;

    //    Instance = null;
    //}
}