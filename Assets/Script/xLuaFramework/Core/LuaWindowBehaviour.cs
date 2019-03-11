using System;
using UnityEngine;
using System.Collections;
using XLua;

[LuaCallCSharp]
public class LuaWindowBehaviour : UIWindowViewBase
{
    [CSharpCallLua]
    public delegate void delLuaAwake(GameObject obj);
    LuaViewBehaviour.delLuaAwake luaAwake;

    [CSharpCallLua]
    public delegate void delLuaStart();
    LuaViewBehaviour.delLuaStart luaStart;

    [CSharpCallLua]
    public delegate void delLuaUpdate();
    LuaViewBehaviour.delLuaUpdate luaUpdate;

    [CSharpCallLua]
    public delegate void delLuaOnDestroy();
    LuaViewBehaviour.delLuaOnDestroy luaOnDestroy;

    private LuaTable scriptEnv;
    private LuaEnv luaEnv;

    public string Tag;

   protected override void OnAwake()
    {
        luaEnv = LuaMgr.luaEnv; //此处要从LuaManager上获取 全局只有一个

        scriptEnv = luaEnv.NewTable();

        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        string prefabName = name;
        if (prefabName.Contains("(Clone)"))
        {
            prefabName = prefabName.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        prefabName = prefabName.Replace("pan_", "");

        luaAwake = scriptEnv.GetInPath<LuaViewBehaviour.delLuaAwake>(prefabName + ".awake");
        luaStart = scriptEnv.GetInPath<LuaViewBehaviour.delLuaStart>(prefabName + ".start");
        luaUpdate = scriptEnv.GetInPath<LuaViewBehaviour.delLuaUpdate>(prefabName + ".update");
        luaOnDestroy = scriptEnv.GetInPath<LuaViewBehaviour.delLuaOnDestroy>(prefabName + ".ondestroy");

        scriptEnv.Set("self", this);
        if (luaAwake != null)
        {
            luaAwake(gameObject);
        }
    }



    protected override void OnStart()
    {
        base.OnStart();

        Debug.Log("c#的 Start");
        if (luaStart != null)
        {
            luaStart();
        }
    }


    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        //备注 调用销毁的话，经常会造成Unity崩溃
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
    }
}