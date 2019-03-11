#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UIViewUtilWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(UIViewUtil), L, translator, 0, 4, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllWindows", _m_CloseAllWindows);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadWindowForLua", _m_LoadWindowForLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadWindow", _m_LoadWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseWindow", _m_CloseWindow);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OpenWindowCount", _g_get_OpenWindowCount);
            
			
			Utils.EndObjectRegister(typeof(UIViewUtil), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(UIViewUtil), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(UIViewUtil));
			
			
			Utils.EndClassRegister(typeof(UIViewUtil), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UIViewUtil __cl_gen_ret = new UIViewUtil();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewUtil constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllWindows(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UIViewUtil __cl_gen_to_be_invoked = (UIViewUtil)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.CloseAllWindows(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadWindowForLua(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UIViewUtil __cl_gen_to_be_invoked = (UIViewUtil)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<XLuaCustomExport.OnCreate>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    XLuaCustomExport.OnCreate OnCreate = translator.GetDelegate<XLuaCustomExport.OnCreate>(L, 3);
                    string path = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.LoadWindowForLua( viewName, OnCreate, path );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<XLuaCustomExport.OnCreate>(L, 3)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    XLuaCustomExport.OnCreate OnCreate = translator.GetDelegate<XLuaCustomExport.OnCreate>(L, 3);
                    
                    __cl_gen_to_be_invoked.LoadWindowForLua( viewName, OnCreate );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.LoadWindowForLua( viewName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewUtil.LoadWindowForLua!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadWindow(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UIViewUtil __cl_gen_to_be_invoked = (UIViewUtil)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& translator.Assignable<XLuaCustomExport.OnCreate>(L, 5)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.GameObject> onComplete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
                    System.Action OnShow = translator.GetDelegate<System.Action>(L, 4);
                    XLuaCustomExport.OnCreate OnCreate = translator.GetDelegate<XLuaCustomExport.OnCreate>(L, 5);
                    string path = LuaAPI.lua_tostring(L, 6);
                    
                    __cl_gen_to_be_invoked.LoadWindow( viewName, onComplete, OnShow, OnCreate, path );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& translator.Assignable<XLuaCustomExport.OnCreate>(L, 5)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.GameObject> onComplete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
                    System.Action OnShow = translator.GetDelegate<System.Action>(L, 4);
                    XLuaCustomExport.OnCreate OnCreate = translator.GetDelegate<XLuaCustomExport.OnCreate>(L, 5);
                    
                    __cl_gen_to_be_invoked.LoadWindow( viewName, onComplete, OnShow, OnCreate );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.GameObject> onComplete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
                    System.Action OnShow = translator.GetDelegate<System.Action>(L, 4);
                    
                    __cl_gen_to_be_invoked.LoadWindow( viewName, onComplete, OnShow );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 3)) 
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.GameObject> onComplete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
                    
                    __cl_gen_to_be_invoked.LoadWindow( viewName, onComplete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewUtil.LoadWindow!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseWindow(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UIViewUtil __cl_gen_to_be_invoked = (UIViewUtil)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string viewName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.CloseWindow( viewName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OpenWindowCount(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UIViewUtil __cl_gen_to_be_invoked = (UIViewUtil)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.OpenWindowCount);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
