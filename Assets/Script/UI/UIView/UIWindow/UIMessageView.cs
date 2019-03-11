using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageView : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private Text m_Title;
    /// <summary>
    /// ����
    /// </summary>
    private Text m_Message;
    /// <summary>
    /// ȷ����ť
    /// </summary>
    private Button btnOk;
    /// <summary>
    /// ȡ����ť
    /// </summary>
    private Button btnCancel;
    /// <summary>
    /// ȷ����ť�ص�
    /// </summary>
    public DelegateDefine.OnMessageOK OnOkClickHandle;
    /// <summary>
    /// ȡ����ť�ص�
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
    /// ��ʾ��ʾ��Ϣ����
    /// </summary>
    /// <param name="title">����</param>
    /// <param name="message">����</param>
    /// <param name="uiMessageType">����</param>
    /// <param name="okAction">ȷ���ص�</param>
    /// <param name="cancelAction">ȡ���ص�</param>
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
