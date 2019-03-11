using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[XLua.LuaCallCSharp]
public class MessageCtrl : Singleton<MessageCtrl>
{
    private UIMessageView m_UIMessageView;


    public void Show(string title, string message, MessageViewType uiMessageType = MessageViewType.Ok, DelegateDefine.OnMessageShow onShow = null, DelegateDefine.OnMessageOK onOk= null, DelegateDefine.OnMessageCancel onCancel = null)
    {
        if (m_UIMessageView == null)
        {
            m_UIMessageView.Show(title, message, uiMessageType,onShow, onOk, onCancel);
            //MessageObj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindow, "pan_Message", cache: true);
        }
        else
        {

            AssetBundleMgr.Instance.LoadOrDownload(string.Format("Download/Prefab/UIPrefab/UIWindow/{0}.assetbundle", "pan_Message"), "pan_Message",(GameObject obj)=> 
            {
                GameObject MessageObj = UnityEngine.Object.Instantiate(obj);
                MessageObj.transform.parent = UISceneCtrl.Instance.CurrentUIScene.Container_Center;
                MessageObj.transform.localPosition = Vector3.zero;
                MessageObj.transform.localScale = Vector3.one;
                MessageObj.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                UIMessageView view = MessageObj.GetComponent<UIMessageView>();
                if (view != null)
                {
                    view.Show(title, message, uiMessageType, onShow, onOk, onCancel);
                }
            });

        }
                    
        

        

    }

}
