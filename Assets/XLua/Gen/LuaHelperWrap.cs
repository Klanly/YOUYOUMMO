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
    public class LuaHelperWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(LuaHelper), L, translator, 0, 10, 6, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadLuaView", _m_LoadLuaView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHttpData", _m_SendHttpData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNetWorkRetValue", _m_GetNetWorkRetValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLanguageText", _m_GetLanguageText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoLoadTexture", _m_AutoLoadTexture);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetData", _m_GetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateMemoryStream", _m_CreateMemoryStream);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendProto", _m_SendProto);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventListener", _m_AddEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventListener", _m_RemoveEventListener);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SocketDispatcher", _g_get_SocketDispatcher);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIDispatcher", _g_get_UIDispatcher);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MessageCtrl", _g_get_MessageCtrl);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISceneCtrl", _g_get_UISceneCtrl);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIViewUtil", _g_get_UIViewUtil);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundleMgr", _g_get_AssetBundleMgr);
            
			
			Utils.EndObjectRegister(typeof(LuaHelper), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(LuaHelper), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(LuaHelper));
			
			
			Utils.EndClassRegister(typeof(LuaHelper), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					LuaHelper __cl_gen_ret = new LuaHelper();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadLuaView(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string ctrlName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.LoadLuaView( ctrlName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHttpData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    XLuaCustomExport.NetWorkSendDataCallBack callBack = translator.GetDelegate<XLuaCustomExport.NetWorkSendDataCallBack>(L, 3);
                    bool isPost = LuaAPI.lua_toboolean(L, 4);
                    string[][] param = (string[][])translator.GetObject(L, 5, typeof(string[][]));
                    
                    __cl_gen_to_be_invoked.SendHttpData( url, callBack, isPost, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNetWorkRetValue(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    CallBackArgs args = (CallBackArgs)translator.GetObject(L, 2, typeof(CallBackArgs));
                    
                        RetValue __cl_gen_ret = __cl_gen_to_be_invoked.GetNetWorkRetValue( args );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLanguageText(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int id = LuaAPI.xlua_tointeger(L, 2);
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.GetLanguageText( id );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoLoadTexture(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    string imgPath = LuaAPI.lua_tostring(L, 3);
                    string imgName = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.AutoLoadTexture( go, imgPath, imgName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    
                        GameDataTableToLua __cl_gen_ret = __cl_gen_to_be_invoked.GetData( path );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateMemoryStream(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 1) 
                {
                    
                        MMO_MemoryStream __cl_gen_ret = __cl_gen_to_be_invoked.CreateMemoryStream(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] buffer = LuaAPI.lua_tobytes(L, 2);
                    
                        MMO_MemoryStream __cl_gen_ret = __cl_gen_to_be_invoked.CreateMemoryStream( buffer );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.CreateMemoryStream!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendProto(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    byte[] buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    __cl_gen_to_be_invoked.SendProto( buffer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    ushort protoCode = (ushort)LuaAPI.xlua_tointeger(L, 2);
                    SocketDispatcher.OnActionHandle callBack = translator.GetDelegate<SocketDispatcher.OnActionHandle>(L, 3);
                    
                    __cl_gen_to_be_invoked.AddEventListener( protoCode, callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEventListener(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    ushort protoCode = (ushort)LuaAPI.xlua_tointeger(L, 2);
                    SocketDispatcher.OnActionHandle callBack = translator.GetDelegate<SocketDispatcher.OnActionHandle>(L, 3);
                    
                    __cl_gen_to_be_invoked.RemoveEventListener( protoCode, callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SocketDispatcher(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SocketDispatcher);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIDispatcher(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.UIDispatcher);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MessageCtrl(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.MessageCtrl);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISceneCtrl(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.UISceneCtrl);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIViewUtil(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.UIViewUtil);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundleMgr(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                LuaHelper __cl_gen_to_be_invoked = (LuaHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.AssetBundleMgr);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
