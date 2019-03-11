using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// HttpͨѶ����
/// </summary>
public class NetWorkHttp :SingletonMono<NetWorkHttp>
{
    #region ����
    /// <summary>
    /// Web����ص�
    /// </summary>
    private XLuaCustomExport.NetWorkSendDataCallBack m_CalBack;
    /// <summary>
    /// Web����ص�����
    /// </summary>
    private CallBackArgs m_CallBackArgs;
    /// <summary>
    /// �Ƿ�æ
    /// </summary>
    private bool m_IsBusy=false;
    /// <summary>
    /// �Ƿ�æ
    /// </summary>
    public bool IsBusy
    {
        get { return m_IsBusy; }
    }

    #endregion

    protected override void OnStart()
    {
        base.OnStart();
        m_CallBackArgs = new CallBackArgs();
    }




    #region SendData ����WEB����

    /// <summary>
    /// ����Web����
    /// </summary>
    /// <param name="url"></param>
    /// <param name="calBack"></param>
    /// <param name="isPost"></param>
    /// <param name="json"></param>
    public void SendData(string url, XLuaCustomExport.NetWorkSendDataCallBack calBack, bool isPost=false, Dictionary<string,object>dic=null)
    {
        if (m_IsBusy)
            return;
        m_IsBusy = true;

        m_CalBack = calBack;

        if(!isPost)
        {
            GetUrl(url);

        }
        else
        {

            //web����
            if (dic!=null)
            {
                //�ͻ��˱�ʶ��
                dic["deviceUniqueIdentifier"] = GlobalInit.Instance.DeviceUniqueIdentifier;
                //�ͻ��˱�ʶ��
                dic["deviceModel"] = GlobalInit.Instance.DeviceModel;
                long t = GlobalInit.Instance.CurrServerTime;
                //ǩ��
                dic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, GlobalInit.Instance.DeviceUniqueIdentifier));
                //ʱ���
                dic["t"] = t;
            }

            PostUrl(url,dic==null ? "" : JsonMapper.ToJson(dic));
        }



    }
    #endregion


     #region Get����
    private void GetUrl(string url)
    {
        WWW data = new WWW(url);
        StartCoroutine(Get(data));
        
    }
    
    private IEnumerator Get(WWW data)
    {
        yield return data;
        m_IsBusy = false;

        if (string.IsNullOrEmpty(data.error))
        {
            if(data.text=="null")
            {
                if (m_CalBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.ErrorMsg = "δ��������";
                    m_CalBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CalBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.text;
                    m_CalBack(m_CallBackArgs);
                }
            }

           


        }
        else
        {
            if(m_CalBack!=null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorMsg = data.error;
                m_CalBack(m_CallBackArgs);
            }

         
        }
        




    }



    #endregion

    #region PostUrl Post����
    private void PostUrl(string url,string json)
    {
        //����һ����
        WWWForm form = new WWWForm();
        //�������ֵ
        form.AddField("", json);

        WWW data = new WWW(url,form);

        StartCoroutine(Request(data));


    }
    private IEnumerator Request(WWW data)
    {
        yield return data;
        m_IsBusy = false;

        if (string.IsNullOrEmpty(data.error))
        {
            if (data.text == "null")
            {
                if (m_CalBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.ErrorMsg = "δ��������";
                    m_CalBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CalBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.text;
                    m_CalBack(m_CallBackArgs);
                }
            }




        }
        else
        {
            if (m_CalBack != null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorMsg = data.error;
                m_CalBack(m_CallBackArgs);
            }


        }





    }
    #endregion




}
/// <summary>
/// Web����ص�
/// </summary>
public  class CallBackArgs:EventArgs
{
    /// <summary>
    /// �Ƿ񱨴�
    /// </summary>
    public bool HasError;
    /// <summary>
    /// ����ԭ��
    /// </summary>
    public string ErrorMsg;
    /// <summary>
    /// ��������
    /// </summary>
    public string Value;
}
