using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageView : MonoBehaviour
{
    /// <summary>
    /// 标题
    /// </summary>
    private Text m_Title;
    /// <summary>
    /// 内容
    /// </summary>
    private Text m_Message;
    /// <summary>
    /// 确定按钮
    /// </summary>
    private Button btnOk;
    /// <summary>
    /// 取消按钮
    /// </summary>
    private Button btnCancel;
    /// <summary>
    /// 确定按钮回调
    /// </summary>
    public DelegateDefine.OnMessageOK OnOkClickHandle;
    /// <summary>
    /// 取消按钮回调
    /// </summary>
    public DelegateDefine.OnMessageCancel OnCancelClickHandle;

    void Awake ()
    {
        m_Title = Global.FindChild(transform, "lblTitle").GetComponent<Text>();
        m_Message = Global.FindChild(transform, "lblMessage").GetComponent<Text>();
        btnOk = Global.FindChild(transform, "btnOk").GetComponent<Button>();
        btnCancel = Global.FindChild(transform, "btnCancel").GetComponent<Button>();

        EventTriggerListener.Get(btnOk.gameObject).onClick = BtnOkClickCallBack;
        EventTriggerListener.Get(btnCancel.gameObject).onClick = BtnCancelClickCallBack;


    }

    private void BtnCancelClickCallBack(GameObject go)
    {
        if (OnCancelClickHandle != null) OnCancelClickHandle();

        Close();
    }

    private void BtnOkClickCallBack(GameObject go)
    {
        if (OnOkClickHandle != null) OnOkClickHandle();
        Close();
    }
    /// <summary>
    /// 显示提示消息窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">内容</param>
    /// <param name="uiMessageType">类型</param>
    /// <param name="okAction">确定回调</param>
    /// <param name="cancelAction">取消回到</param>
    public void Show(string title,string message,MessageViewType uiMessageType=MessageViewType.Ok, DelegateDefine.OnMessageShow onShow = null, DelegateDefine.OnMessageOK okAction=null, DelegateDefine.OnMessageCancel cancelAction =null)
    {
        if (onShow!=null)
        {
            onShow();
        }
        gameObject.transform.localPosition = Vector3.zero;
        m_Title.text = title;
        m_Message.text = message;
        OnOkClickHandle = okAction;
        OnCancelClickHandle = cancelAction;
        switch (uiMessageType)
        {
            case MessageViewType.Ok:
                btnOk.transform.localPosition = Vector3.zero;
                btnCancel.gameObject.SetActive(false);
                break;
            case MessageViewType.OkAndCancel:
                btnOk.transform.localPosition = new Vector3(-70, 0, 0);
                btnCancel.gameObject.SetActive(true);
                break;
            default:
                break;
        }

    }


    public void Close()
    {
        gameObject.transform.localPosition = new Vector3(0, 5000, 0);
    }
	


}
