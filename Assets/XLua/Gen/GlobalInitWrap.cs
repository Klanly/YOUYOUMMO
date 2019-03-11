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
    public class GlobalInitWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(GlobalInit), L, translator, 0, 3, 15, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrTime", _m_GetCurrTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrServerTime", _m_GetCurrServerTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitChannelConfig", _m_InitChannelConfig);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrServerTime", _g_get_CurrServerTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DeviceUniqueIdentifier", _g_get_DeviceUniqueIdentifier);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DeviceModel", _g_get_DeviceModel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrChannelInitConfig", _g_get_CurrChannelInitConfig);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrRoleNickName", _g_get_CurrRoleNickName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrPlayer", _g_get_CurrPlayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIAnimationCurve", _g_get_UIAnimationCurve);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrSelectGameServer", _g_get_CurrSelectGameServer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrAccount", _g_get_CurrAccount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "T4MShader", _g_get_T4MShader);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CheckServerTime", _g_get_CheckServerTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PingValue", _g_get_PingValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GameServerTime", _g_get_GameServerTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "JobObjectDic", _g_get_JobObjectDic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainPlayerInfo", _g_get_MainPlayerInfo);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrChannelInitConfig", _s_set_CurrChannelInitConfig);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrRoleNickName", _s_set_CurrRoleNickName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrPlayer", _s_set_CurrPlayer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UIAnimationCurve", _s_set_UIAnimationCurve);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrSelectGameServer", _s_set_CurrSelectGameServer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrAccount", _s_set_CurrAccount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "T4MShader", _s_set_T4MShader);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CheckServerTime", _s_set_CheckServerTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PingValue", _s_set_PingValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GameServerTime", _s_set_GameServerTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "JobObjectDic", _s_set_JobObjectDic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MainPlayerInfo", _s_set_MainPlayerInfo);
            
			Utils.EndObjectRegister(typeof(GlobalInit), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(GlobalInit), L, __CreateInstance, 3, 4, 4);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MMO_NICKNAME", GlobalInit.MMO_NICKNAME);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MMO_PWD", GlobalInit.MMO_PWD);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(GlobalInit));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "WebAccountUrl", _g_get_WebAccountUrl);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ChannelId", _g_get_ChannelId);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "InnerVersion", _g_get_InnerVersion);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "WebAccountUrl", _s_set_WebAccountUrl);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ChannelId", _s_set_ChannelId);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "InnerVersion", _s_set_InnerVersion);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Instance", _s_set_Instance);
            
			Utils.EndClassRegister(typeof(GlobalInit), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					GlobalInit __cl_gen_ret = new GlobalInit();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GlobalInit constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrTime(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.GetCurrTime(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrServerTime(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        long __cl_gen_ret = __cl_gen_to_be_invoked.GetCurrServerTime(  );
                        LuaAPI.lua_pushint64(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitChannelConfig(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string webAccountUrl = LuaAPI.lua_tostring(L, 2);
                    int channelId = LuaAPI.xlua_tointeger(L, 3);
                    int innerVersion = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.InitChannelConfig( ref webAccountUrl, ref channelId, ref innerVersion );
                    LuaAPI.lua_pushstring(L, webAccountUrl);
                        
                    LuaAPI.xlua_pushinteger(L, channelId);
                        
                    LuaAPI.xlua_pushinteger(L, innerVersion);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrServerTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, __cl_gen_to_be_invoked.CurrServerTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DeviceUniqueIdentifier(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.DeviceUniqueIdentifier);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DeviceModel(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.DeviceModel);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WebAccountUrl(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, GlobalInit.WebAccountUrl);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChannelId(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushinteger(L, GlobalInit.ChannelId);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InnerVersion(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushinteger(L, GlobalInit.InnerVersion);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrChannelInitConfig(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.CurrChannelInitConfig);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GlobalInit.Instance);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrRoleNickName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.CurrRoleNickName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrPlayer(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.CurrPlayer);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIAnimationCurve(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.UIAnimationCurve);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrSelectGameServer(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.CurrSelectGameServer);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrAccount(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.CurrAccount);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_T4MShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.T4MShader);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CheckServerTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.CheckServerTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PingValue(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.PingValue);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GameServerTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, __cl_gen_to_be_invoked.GameServerTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_JobObjectDic(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.JobObjectDic);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainPlayerInfo(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.MainPlayerInfo);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WebAccountUrl(RealStatePtr L)
        {
            
            try {
			    GlobalInit.WebAccountUrl = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChannelId(RealStatePtr L)
        {
            
            try {
			    GlobalInit.ChannelId = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InnerVersion(RealStatePtr L)
        {
            
            try {
			    GlobalInit.InnerVersion = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrChannelInitConfig(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CurrChannelInitConfig = (ChannelInitConfig)translator.GetObject(L, 2, typeof(ChannelInitConfig));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Instance(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    GlobalInit.Instance = (GlobalInit)translator.GetObject(L, 1, typeof(GlobalInit));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrRoleNickName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CurrRoleNickName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrPlayer(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CurrPlayer = (RoleCtrl)translator.GetObject(L, 2, typeof(RoleCtrl));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UIAnimationCurve(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.UIAnimationCurve = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrSelectGameServer(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CurrSelectGameServer = (RetGameServerEntity)translator.GetObject(L, 2, typeof(RetGameServerEntity));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrAccount(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CurrAccount = (RetAccountEntity)translator.GetObject(L, 2, typeof(RetAccountEntity));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_T4MShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.T4MShader = (UnityEngine.Shader)translator.GetObject(L, 2, typeof(UnityEngine.Shader));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CheckServerTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.CheckServerTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PingValue(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.PingValue = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GameServerTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.GameServerTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_JobObjectDic(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.JobObjectDic = (System.Collections.Generic.Dictionary<int, UnityEngine.GameObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<int, UnityEngine.GameObject>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MainPlayerInfo(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GlobalInit __cl_gen_to_be_invoked = (GlobalInit)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.MainPlayerInfo = (RoleInfoMainPlayer)translator.GetObject(L, 2, typeof(RoleInfoMainPlayer));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
