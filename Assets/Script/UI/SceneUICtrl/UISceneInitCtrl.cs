//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2017-03-25 14:48:54
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISceneInitCtrl : MonoBehaviour 
{
    [SerializeField]
    private Text txt_Load;

    [SerializeField]
    private Slider slider_Load;

    public static UISceneInitCtrl Instance;

    void Awake () 
	{
        Instance = this;
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    public void SetProgress(string text, float value)
    {
        txt_Load.SetText(text);
        slider_Load.SetSliderValue(value);
    }
}