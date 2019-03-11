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
    public class MessageCtrlWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(MessageCtrl), L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Show", _m_Show);
			
			
			
			
			Utils.EndObjectRegister(typeof(MessageCtrl), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(MessageCtrl), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(MessageCtrl));
			
			
			Utils.EndClassRegister(typeof(MessageCtrl), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					MessageCtrl __cl_gen_ret = new MessageCtrl();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MessageCtrl constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Show(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            MessageCtrl __cl_gen_to_be_invoked = (MessageCtrl)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<MessageViewType>(L, 4)&& translator.Assignable<DelegateDefine.OnMessageShow>(L, 5)&& translator.Assignable<DelegateDefine.OnMessageOK>(L, 6)&& translator.Assignable<DelegateDefine.OnMessageCancel>(L, 7)) 
                {
                    string title = LuaAPI.lua_tostring(L, 2);
                    string message = LuaAPI.lua_tostring(L, 3);
                    MessageViewType uiMessageType;translator.Get(L, 4, out uiMessageType);
                    DelegateDefine.OnMessageShow onShow = translator.GetDelegate<DelegateDefine.OnMessageShow>(L, 5);
                    DelegateDefine.OnMessageOK onOk = translator.GetDelegate<DelegateDefine.OnMessageOK>(L, 6);
                    DelegateDefine.OnMessageCancel onCancel = translator.GetDelegate<DelegateDefine.OnMessageCancel>(L, 7);
                    
                    __cl_gen_to_be_invoked.Show( title, message, uiMessageType, onShow, onOk, onCancel );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<MessageViewType>(L, 4)&& translator.Assignable<DelegateDefine.OnMessageShow>(L, 5)&& translator.Assignable<DelegateDefine.OnMessageOK>(L, 6)) 
                {
                    string title = LuaAPI.lua_tostring(L, 2);
                    string message = LuaAPI.lua_tostring(L, 3);
                    MessageViewType uiMessageType;translator.Get(L, 4, out uiMessageType);
                    DelegateDefine.OnMessageShow onShow = translator.GetDelegate<DelegateDefine.OnMessageShow>(L, 5);
                    DelegateDefine.OnMessageOK onOk = translator.GetDelegate<DelegateDefine.OnMessageOK>(L, 6);
                    
                    __cl_gen_to_be_invoked.Show( title, message, uiMessageType, onShow, onOk );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<MessageViewType>(L, 4)&& translator.Assignable<DelegateDefine.OnMessageShow>(L, 5)) 
                {
                    string title = LuaAPI.lua_tostring(L, 2);
                    string message = LuaAPI.lua_tostring(L, 3);
                    MessageViewType uiMessageType;translator.Get(L, 4, out uiMessageType);
                    DelegateDefine.OnMessageShow onShow = translator.GetDelegate<DelegateDefine.OnMessageShow>(L, 5);
                    
                    __cl_gen_to_be_invoked.Show( title, message, uiMessageType, onShow );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<MessageViewType>(L, 4)) 
                {
                    string title = LuaAPI.lua_tostring(L, 2);
                    string message = LuaAPI.lua_tostring(L, 3);
                    MessageViewType uiMessageType;translator.Get(L, 4, out uiMessageType);
                    
                    __cl_gen_to_be_invoked.Show( title, message, uiMessageType );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string title = LuaAPI.lua_tostring(L, 2);
                    string message = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.Show( title, message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to MessageCtrl.Show!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
