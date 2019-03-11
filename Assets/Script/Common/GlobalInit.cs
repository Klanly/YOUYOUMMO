 using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Xml.Linq;
[XLua.LuaCallCSharp]
public class GlobalInit : MonoBehaviour 
{
    #region 常量
    /// <summary>
    /// 昵称KEY
    /// </summary>
    public const string MMO_NICKNAME = "MMO_NICKNAME";

    /// <summary>
    /// 密码KEY
    /// </summary>
    public const string MMO_PWD = "MMO_PWD";

    /// <summary>
    /// 账户服务器地址
    /// </summary>
    public static string WebAccountUrl;
    /// <summary>
    /// 渠道号
    /// </summary>
    public static int ChannelId;
    /// <summary>
    /// 内部版本号
    /// </summary>
    public static int InnerVersion;

    /// <summary>
    /// 当前渠道
    /// </summary>
    public ChannelInitConfig CurrChannelInitConfig;

 
    #endregion

    public static GlobalInit Instance;

    /// <summary>
    /// 玩家注册时候的昵称
    /// </summary>
    [HideInInspector]
    public string CurrRoleNickName;

    /// <summary>
    /// 当前玩家
    /// </summary>
    [HideInInspector]
    public RoleCtrl CurrPlayer;

    /// <summary>
    /// UI动画曲线
    /// </summary>
    public AnimationCurve UIAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    /// <summary>
    /// 服务器时间
    /// </summary>
    private long ServerTime = 0;
    /// <summary>
    /// 当前选择的服务器
    /// </summary>
    public RetGameServerEntity CurrSelectGameServer;
    /// <summary>
    /// 当前选择的服务器
    /// </summary>
    public RetAccountEntity CurrAccount;

    public Shader T4MShader;
    /// <summary>
    /// 服务器对表时刻
    /// </summary>
    public float CheckServerTime;
    /// <summary>
    /// PING值
    /// </summary>
    public int PingValue;
    /// <summary>
    /// 服务器时间
    /// </summary>
    public long GameServerTime;
    /// <summary>
    /// 主角色镜像
    /// </summary>
    [HideInInspector]
    public Dictionary<int, GameObject> JobObjectDic = new Dictionary<int, GameObject>();

    /// <summary>
    /// 主角信息
    /// </summary>
    [HideInInspector]
    public RoleInfoMainPlayer MainPlayerInfo;

    /// <summary>
    /// 获取当前年月日时分秒毫秒格式
    /// </summary>
    /// <returns></returns>
    public string GetCurrTime()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }

    /// <summary>
    /// 获取当前服务器时间
    /// </summary>
    /// <returns></returns>
    public long GetCurrServerTime()
    {
        return (int)((Time.realtimeSinceStartup - CheckServerTime) * 1000) + GameServerTime;
    }

    /// <summary>
    /// 当前服务器时间
    /// </summary>
    public long CurrServerTime
    {
        get
        {
            return CurrChannelInitConfig.ServerTime + (long)RealTime.time;
        }
    }

    /// <summary>
    /// 设备标识符
    /// </summary>
    public string DeviceUniqueIdentifier
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
            
        
    }

    /// <summary>
    /// 获取设备型号
    /// </summary>
    public string DeviceModel
    {
        get
        {

#if UNITY_IPHONE && !UNITY_EDITOR
            return Device.generation.ToString();
#else
            return SystemInfo.deviceModel;
#endif
        }



    }



    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

	void Start ()
	{
        CurrChannelInitConfig = new ChannelInitConfig();

        InitChannelConfig(ref WebAccountUrl,ref ChannelId,ref InnerVersion);
        DebugApp.Log("WebAccountUrl="+ WebAccountUrl);
        DebugApp.Log("ChannelId=" + ChannelId);
        DebugApp.Log("InnerVersion=" + InnerVersion);

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["ChannelId"] = ChannelId;
        dic["InnerVersion"] = InnerVersion;


        //初始化的时候 请求服务器时间
        NetWorkHttp.Instance.SendData(WebAccountUrl + "api/init", OnGetTimeCallBack,isPost:true,dic: dic);

	}

    /// <summary>
    /// 初始化渠道配置文件
    /// </summary>
    /// <param name="webAccountUrl"></param>
    /// <param name="channelId"></param>
    /// <param name="innerVersion"></param>
    public void InitChannelConfig(ref string webAccountUrl,ref int channelId,ref int innerVersion)
    {
        TextAsset asset = Resources.Load("Config/ChannelConfig") as TextAsset;
        XDocument xDoc = XDocument.Parse(asset.text);
        XElement root = xDoc.Root;
        webAccountUrl = root.Element("WebAccountUrl").Attribute("Value").Value;
        channelId = root.Element("ChannelId").Attribute("Value").Value.ToInt();
        innerVersion = root.Element("InnerVersion").Attribute("Value").Value.ToInt();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(1);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(2);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(3);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(4);
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(5);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(6);
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(7);
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    SceneMgr.Instance.LoadToWorldMap(8);
        //}

    }

    private void OnGetTimeCallBack(CallBackArgs obj)
    {
        if (!obj.HasError)
        {
            //ServerTime =long.Parse(obj.Value);
            string item = obj.Value;

            LitJson.JsonData data = LitJson.JsonMapper.ToObject(obj.Value);

            bool hasError = (bool)data["HasError"];
            if (!hasError)
            {
                LitJson.JsonData config = LitJson.JsonMapper.ToObject(data["Value"].ToString());

                CurrChannelInitConfig.ServerTime = long.Parse(config["ServerTime"].ToString());
                CurrChannelInitConfig.SourceUrl = config["SourceUrl"].ToString();
                CurrChannelInitConfig.RechargeUrl = config["RechargeUrl"].ToString();
                CurrChannelInitConfig.TDAppId = config["TDAppId"].ToString();
                CurrChannelInitConfig.IsOpenTD = int.Parse(config["IsOpenTD"].ToString())==1;
                CurrChannelInitConfig.PayServerNo= config["PayServerNo"].ToString();
                if (DelegateDefine.Instance.OnChannelInitOk != null)
                {
                    DelegateDefine.Instance.OnChannelInitOk();
                }
            }
   


        }
        else
        {

        }
    }
}