using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/// <summary>
/// Lua����������
/// </summary>
public class LuaMgr : SingletonMono<LuaMgr>
{
    /// <summary>
    /// ȫ��LUA����
    /// </summary>
    public static LuaEnv luaEnv;

	void Awake ()
    {
        //1 ʵ����xlua����
        luaEnv = new LuaEnv();

        //2����xlua�Ľű�·��
        luaEnv.DoString(string.Format("package.path='{0}/?.lua'",Application.dataPath));

    }
    /// <summary>
    /// ִ��lua�ű�
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
