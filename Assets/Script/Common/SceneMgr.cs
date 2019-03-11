using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    public SceneMgr()
    {
        ///服务器返回角色进入世界地图场景消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.WorldMap_RoleEnterReturn, OnWorldMapRoleEnterReturn);
    }
    /// <summary>
    /// 玩法类型
    /// </summary>
    public PlayType CurrPlayerType
    {
        get;
        private set;
    }
    /// <summary>
    /// 是否战斗场景 PVP  
    /// </summary>
    public bool IsFightingScene
    {
        get;
      private  set;
    }

    /// <summary>
    /// 当前关卡ID
    /// </summary>
    private int currGameLevelId;
    /// <summary>
    /// 当前场景类型
    /// </summary>
    private GameLevelGrade currGameLevelGrade;

    private int m_CurrWorldMapId;
    /// <summary>
    /// 当前世界地图编号
    /// </summary>
    public int CurrWorldMapId
    {
        get
        {
            return m_CurrWorldMapId;
        }

    }

    public int CurrGameLevelId
    {
        get
        {
            return currGameLevelId;
        }

        set
        {
            currGameLevelId = value;
        }
    }

    public GameLevelGrade CurrGameLevelGrade
    {
        get
        {
            return currGameLevelGrade;
        }

        set
        {
            currGameLevelGrade = value;
        }
    }
    /// <summary>
    /// 目标世界地图传送点id
    /// </summary>
    public int TransWorldMapTransPosId;
    /// <summary>
    /// 当前场景类型
    /// </summary>
    public SceneType CurrentSceneType
    {
        get;
        private set;
    }


    /// <summary>
    /// 选人场景
    /// </summary>
    public void LoadToSelectRole()
    {

        CurrentSceneType = SceneType.SelectRole;
        SceneManager.LoadScene("Scene_Loading");
    }
    /// <summary>
    /// 登录场景
    /// </summary>
    public void LoadToLogOn()
    {
        CurrentSceneType = SceneType.LogOn;
        SceneManager.LoadScene("Scene_Loading");
        //Application.LoadLevel("Scene_Loading");
    }


    /// <summary>
    /// 去世界地图场景（主城+野外场景）
    /// </summary>
    public void LoadToWorldMap(int worldMapId)
    {
        if (m_CurrWorldMapId == worldMapId&& CurrPlayerType==PlayType.PVP)
        {
            MessageCtrl.Instance.Show("提示","你已经在目标场景");
            return;
        }
        WorldMapRoleEnter(worldMapId);
    }
    /// <summary>
    /// 想要去的世界地图场景id
    /// </summary>
    private int m_WillToWorldMapId = 0;
    /// <summary>
    /// 客户端发送进入世界地图消息
    /// </summary>
    /// <param name="worldMapId"></param>
    private void WorldMapRoleEnter(int worldMapId)
    {
        m_WillToWorldMapId = worldMapId;

        WorldMap_RoleEnterProto proto = new WorldMap_RoleEnterProto();
        proto.WorldMapSceneId = m_WillToWorldMapId;

        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }


    /// <summary>
    /// 去游戏关卡场景
    /// </summary>
    public void LoadToGameLevel(int gameLevelId, GameLevelGrade grade)
    {
        currGameLevelId = gameLevelId;
        currGameLevelGrade = grade;
        CurrPlayerType = PlayType.PVE;
        CurrentSceneType = SceneType.GameLevel;
        SceneManager.LoadScene("Scene_Loading");

    }
    /// <summary>
    /// 服务器返回角色进入世界地图场景消息
    /// </summary>
    /// <param name="p"></param>
    private void OnWorldMapRoleEnterReturn(byte[] p)
    {
        WorldMap_RoleEnterReturnProto proto = WorldMap_RoleEnterReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            m_CurrWorldMapId = m_WillToWorldMapId;
            CurrentSceneType = SceneType.WorldMap;
            CurrPlayerType = PlayType.PVP;

            WorldMapEntity entity = WorldMapDBModel.Instance.Get(m_CurrWorldMapId);
            if (entity!=null)
            {
                //不是主城可以战斗
                IsFightingScene = entity.IsCity == 0;
            }

            SceneManager.LoadScene("Scene_Loading");
        }
        
    }
}