using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/// <summary>
/// Lua环境管理器
/// </summary>
public class LuaMgr : SingletonMono<LuaMgr>
{
    /// <summary>
    /// 全局LUA引擎
    /// </summary>
    public static LuaEnv luaEnv;

	void Awake ()
    {
        //1 实例化xlua引擎
        luaEnv = new LuaEnv();

        //2设置xlua的脚本路径
        luaEnv.DoString(string.Format("package.path='{0}/?.lua'",Application.dataPath));

    }
    /// <summary>
    /// 执行lua脚本
    /// </summary>
    /// <param name="path"></param>
    public void DOString(string path)
    {
        luaEnv.DoString(path);
    }

	void Start ()
    {
		
	}
}
