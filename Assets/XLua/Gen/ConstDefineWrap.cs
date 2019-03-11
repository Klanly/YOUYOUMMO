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
    public class ConstDefineWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(ConstDefine), L, translator, 0, 0, 0, 0);
			
			
			
			
			
			Utils.EndObjectRegister(typeof(ConstDefine), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(ConstDefine), L, __CreateInstance, 60, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LogOn_AccountID", ConstDefine.LogOn_AccountID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LogOn_AccountUserName", ConstDefine.LogOn_AccountUserName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LogOn_AccountPwd", ConstDefine.LogOn_AccountPwd);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UILogOnView_btnLogOn", ConstDefine.UILogOnView_btnLogOn);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UILogOnView_btnToReg", ConstDefine.UILogOnView_btnToReg);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIRegView_btnToLogOn", ConstDefine.UIRegView_btnToLogOn);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIRegView_btnReg", ConstDefine.UIRegView_btnReg);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIGameServerEnterView_btnEnterGameServer", ConstDefine.UIGameServerEnterView_btnEnterGameServer);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UIGameServerEnterView_btnSelectServer", ConstDefine.UIGameServerEnterView_btnSelectServer);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RechargeOk", ConstDefine.RechargeOk);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JobId", ConstDefine.JobId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NickName", ConstDefine.NickName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Level", ConstDefine.Level);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fighting", ConstDefine.Fighting);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Money", ConstDefine.Money);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Gold", ConstDefine.Gold);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CurrHP", ConstDefine.CurrHP);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxHP", ConstDefine.MaxHP);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CurrMP", ConstDefine.CurrMP);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxMP", ConstDefine.MaxMP);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CurrExp", ConstDefine.CurrExp);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxExp", ConstDefine.MaxExp);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Attack", ConstDefine.Attack);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Defense", ConstDefine.Defense);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hit", ConstDefine.Hit);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Dodge", ConstDefine.Dodge);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Cri", ConstDefine.Cri);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Res", ConstDefine.Res);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ChapterId", ConstDefine.ChapterId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ChapterName", ConstDefine.ChapterName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ChapterBG", ConstDefine.ChapterBG);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelList", ConstDefine.GameLevelList);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelId", ConstDefine.GameLevelId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelName", ConstDefine.GameLevelName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelPostion", ConstDefine.GameLevelPostion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelisBoss", ConstDefine.GameLevelisBoss);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelIco", ConstDefine.GameLevelIco);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelDlgPic", ConstDefine.GameLevelDlgPic);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelExp", ConstDefine.GameLevelExp);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelGold", ConstDefine.GameLevelGold);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelDesc", ConstDefine.GameLevelDesc);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelConditionDesc", ConstDefine.GameLevelConditionDesc);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelCommendFighting", ConstDefine.GameLevelCommendFighting);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelReward", ConstDefine.GameLevelReward);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelPassTime", ConstDefine.GameLevelPassTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameLevelStar", ConstDefine.GameLevelStar);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GoodsId", ConstDefine.GoodsId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GoodsName", ConstDefine.GoodsName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GoodsType", ConstDefine.GoodsType);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkillSlotsNo", ConstDefine.SkillSlotsNo);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkillId", ConstDefine.SkillId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkillLevel", ConstDefine.SkillLevel);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkillPic", ConstDefine.SkillPic);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkillCDTime", ConstDefine.SkillCDTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldMapList", ConstDefine.WorldMapList);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldMapId", ConstDefine.WorldMapId);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldMapName", ConstDefine.WorldMapName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldMapPostion", ConstDefine.WorldMapPostion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldMapIco", ConstDefine.WorldMapIco);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(ConstDefine));
			
			
			Utils.EndClassRegister(typeof(ConstDefine), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ConstDefine __cl_gen_ret = new ConstDefine();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ConstDefine constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
