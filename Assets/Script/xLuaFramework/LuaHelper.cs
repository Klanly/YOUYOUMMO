using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaHelper:Singleton<LuaHelper>
{
    #region LoadLuaView
    [CSharpCallLua]
    public delegate void delLuaLoadView(string ctrlName);
    LuaHelper.delLuaLoadView luaLoadView;

    private LuaTable scriptEnv;
    private LuaEnv luaEnv;

    public void LoadLuaView(string ctrlName)
    {
        luaEnv = LuaMgr.luaEnv;

        if (luaEnv == null) return;

        scriptEnv = luaEnv.NewTable();

        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        luaLoadView = scriptEnv.GetInPath<LuaHelper.delLuaLoadView>("GameInit.LoadView");
        if (luaLoadView!=null)
        {
            luaLoadView(ctrlName);
        }
        scriptEnv = null;
    }
    #endregion


    #region SendHttpData 发送Http数据
    /// <summary>
    /// 发送Http数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callBack"></param>
    /// <param name="isPost"></param>
    /// <param name="param"></param>
    public void SendHttpData(string url, XLuaCustomExport.NetWorkSendDataCallBack callBack, bool isPost, string[][] param)
    {
        Dictionary<string, object> dic = null;
        if (param != null)
        {
            dic = new Dictionary<string, object>();

            for (int i = 0; i < param.Length; i++)
            {
                if (param[i].Length >= 2)
                {
                    string key = param[i][0];
                    object value = param[i][1];

                    dic[key] = value;
                }
            }
        }

        NetWorkHttp.Instance.SendData(url, callBack, isPost, dic);
    }

    public RetValue GetNetWorkRetValue(CallBackArgs args)
    {
        RetValue retValue = JsonMapper.ToObject<RetValue>(args.Value);
        return retValue;
    }
    #endregion

    /// <summary>
    /// 根据编号获取文本提示
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetLanguageText(int id)
    {
        return LanguageDBModel.Instance.GetText(id);
    }

    /// <summary>
    /// 自动加载图片
    /// </summary>
    /// <param name="go"></param>
    /// <param name="imgPath"></param>
    /// <param name="imgName"></param>
    public void AutoLoadTexture(GameObject go,string imgPath,string imgName)
    {
        AutoLoadTexture component = go.GetOrCreatComponent<AutoLoadTexture>();
        if (component!=null)
        {
            component.ImgPath = imgPath;
            component.ImgName = imgName;
        }
    }

    public SocketDispatcher SocketDispatcher
    {
        get { return global::SocketDispatcher.Instance; }
    }
    public UIDispatcher UIDispatcher
    {
        get { return UIDispatcher.Instance; }
    }
    public MessageCtrl MessageCtrl
    {
        get { return MessageCtrl.Instance; }
    }

    public UISceneCtrl UISceneCtrl
    {
        get { return UISceneCtrl.Instance; }
    }

    public UIViewUtil UIViewUtil
    {
        get { return UIViewUtil.Instance; }
    }

    public AssetBundleMgr AssetBundleMgr
    {
        get { return AssetBundleMgr.Instance; }
    }
    /// <summary>
    /// 获取表格数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameDataTableToLua GetData(string path)
    {
        GameDataTableToLua data = new GameDataTableToLua();
#if DISABLE_ASSETBUNDLE
        path = Application.dataPath + "/Download/DataTable/" + path;

#else
         path = Application.persistentDataPath + "/Download/DataTable/" + path;
#endif
        using (GameDataTableParser parse = new GameDataTableParser(path))
        {
            data.Row = parse.Row;
            data.Column = parse.Column;

            data.Data = new string[data.Row][];

            //转换交叉数组

            for (int i = 0; i < data.Row; i++)
            {
                string[] arr = new string[data.Column];

                for (int j = 0; j < data.Column; j++)
                {
                    arr[j] = parse.GameData[i, j];
                }
                data.Data[i] = arr;
            }
        }
        return data;

    }

    /// <summary>
    /// 创建一个MMO_MemoryStream 发送
    /// </summary>
    /// <returns></returns>
    public MMO_MemoryStream CreateMemoryStream()
    {
        return new MMO_MemoryStream();
    }

    /// <summary>
    /// 创建一个带Buffer MMO_MemoryStream 接受解析
    /// </summary>
    /// <returns></returns>
    public MMO_MemoryStream CreateMemoryStream(byte[] buffer)
    {
        return new MMO_MemoryStream(buffer);
    }

    /// <summary>
    /// 发送协议
    /// </summary>
    /// <param name="buffer"></param>
    public void SendProto(byte[] buffer)
    {
        NetWorkSocket.Instance.SendMsg(buffer);
    }
    /// <summary>
    /// 添加Lua监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="callBack"></param>
    public void AddEventListener(ushort protoCode,SocketDispatcher.OnActionHandle callBack)
    {

        global::SocketDispatcher.Instance.AddEventListener(protoCode, callBack);
    }
    /// <summary>
    /// 移除Lua监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="callBack"></param>
    public void RemoveEventListener(ushort protoCode, SocketDispatcher.OnActionHandle callBack)
    {

        global::SocketDispatcher.Instance.RemoveEventListener(protoCode, callBack);
        
    }
  
}
