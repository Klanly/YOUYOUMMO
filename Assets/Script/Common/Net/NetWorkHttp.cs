using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Http通讯管理
/// </summary>
public class NetWorkHttp :SingletonMono<NetWorkHttp>
{
    #region 属性
    /// <summary>
    /// Web请求回调
    /// </summary>
    private XLuaCustomExport.NetWorkSendDataCallBack m_CalBack;
    /// <summary>
    /// Web请求回调数据
    /// </summary>
    private CallBackArgs m_CallBackArgs;
    /// <summary>
    /// 是否繁忙
    /// </summary>
    private bool m_IsBusy=false;
    /// <summary>
    /// 是否繁忙
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




    #region SendData 发送WEB数据

    /// <summary>
    /// 发送Web数据
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

            //web加密
            if (dic!=null)
            {
                //客户端标识符
                dic["deviceUniqueIdentifier"] = GlobalInit.Instance.DeviceUniqueIdentifier;
                //客户端标识符
                dic["deviceModel"] = GlobalInit.Instance.DeviceModel;
                long t = GlobalInit.Instance.CurrServerTime;
                //签名
                dic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, GlobalInit.Instance.DeviceUniqueIdentifier));
                //时间戳
                dic["t"] = t;
            }

            PostUrl(url,dic==null ? "" : JsonMapper.ToJson(dic));
        }



    }
    #endregion


     #region Get请求
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
                    m_CallBackArgs.ErrorMsg = "未请求到数据";
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

    #region PostUrl Post请求
    private void PostUrl(string url,string json)
    {
        //定义一个表单
        WWWForm form = new WWWForm();
        //给表单添加值
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
                    m_CallBackArgs.ErrorMsg = "未请求到数据";
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
/// Web请求回调
/// </summary>
public  class CallBackArgs:EventArgs
{
    /// <summary>
    /// 是否报错
    /// </summary>
    public bool HasError;
    /// <summary>
    /// 错误原因
    /// </summary>
    public string ErrorMsg;
    /// <summary>
    /// 返回数据
    /// </summary>
    public string Value;
}
