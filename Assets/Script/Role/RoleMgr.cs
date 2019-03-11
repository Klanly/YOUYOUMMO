using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleMgr:Singleton<RoleMgr> 
{
    private bool m_IsMainPlayerInit = false;

    /// <summary>
    /// 加载角色头像
    /// </summary>
    /// <returns></returns>
    public Sprite LoadHeadPic(string headPic)
    {
        return Resources.Load(string.Format("UI/HeadImg/{0}", headPic), typeof(Sprite)) as Sprite;
    }


    /// <summary>
    /// 加载技能图片
    /// </summary>
    /// <returns></returns>
    public Sprite LoadSkillPic(string skilPic)
    {
        return Resources.Load(string.Format("UI/Skill/{0}", skilPic), typeof(Sprite)) as Sprite;
    }

    /// <summary>
    /// 初始化主角
    /// </summary>
    public void InitMainPlayer()
    {
        if (m_IsMainPlayerInit) return;
        if (GlobalInit.Instance.MainPlayerInfo!=null)
        {
            GameObject mainPlayerObj =UnityEngine.Object.Instantiate(GlobalInit.Instance.JobObjectDic[GlobalInit.Instance.MainPlayerInfo.JobId]);
            UnityEngine.Object.DontDestroyOnLoad(mainPlayerObj);

            GlobalInit.Instance.MainPlayerInfo.SetPhySkillId(JobDBModel.Instance.Get(GlobalInit.Instance.MainPlayerInfo.JobId).UsedPhyAttackIds);
            GlobalInit.Instance.CurrPlayer = mainPlayerObj.GetComponent<RoleCtrl>();
            GlobalInit.Instance.CurrPlayer.Init(RoleType.MainPlayer,GlobalInit.Instance.MainPlayerInfo,new GameLevel_RoleMonsterAI(GlobalInit.Instance.CurrPlayer));
        }
        m_IsMainPlayerInit = true;
    }


    #region LoadRole 根据角色预设名称 加载角色
    /// <summary>
    /// 根据角色预设名称 加载角色
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadRole(string name, RoleType type)
    {
        string path = string.Empty;

        switch (type)
        {
            case RoleType.MainPlayer:
                path = "Player";
                break;
            case RoleType.Monster:
                path = "Monster";
                break;
        }

        return ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.Role, string.Format("{0}/{1}", path, name), cache: true);
    }

    /// <summary>
    /// 根据玩家编号 加载角色
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public GameObject LoadPlayer(int jobid)
    {
        GameObject obj = GlobalInit.Instance.JobObjectDic[jobid];

        return Object.Instantiate(obj);
    }

    public RoleCtrl LoadOtherRole(int roleId, string roleNickName, int roleLevel, int roleJobId, int maxHp, int currHp, int maxMp, int currMp)
    {
        GameObject roleObj = Object.Instantiate(GlobalInit.Instance.JobObjectDic[roleJobId]);
        RoleCtrl roleCtrl = roleObj.GetComponent<RoleCtrl>();

        //角色初始化 信息
        RoleInfoMainPlayer roleInfo = new RoleInfoMainPlayer();

        roleInfo.RoleId = roleId;
        roleInfo.RoleNickName = roleNickName;
        roleInfo.Level = roleLevel;
        roleInfo.JobId = (byte)roleJobId;
        roleInfo.MaxHP = maxHp;
        roleInfo.CurrHP = currHp;
        roleInfo.MaxMP = maxMp;
        roleInfo.CurrMP = currMp;
        roleCtrl.Init(RoleType.OtherPlayer,roleInfo,new OtherRoleAI(roleCtrl));

        return roleCtrl;
    }

    /// <summary>
    /// 根据精灵编号返回 精灵预设
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public void LoadSprite(int sriteId,System.Action<GameObject>onComplete)
    {
        SpriteEntity entity = SpriteDBModel.Instance.Get(sriteId);
        if (entity == null) return;
         AssetBundleMgr.Instance.LoadOrDownload(string.Format("Download/Prefab/RolePrefab/Monster/{0}.assetbundle", entity.PrefabName), entity.PrefabName, onComplete);      

    }


    /// <summary>
    /// 加载NPC
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public void LoadNPC(string prefabName,System.Action<GameObject>onCompelete)
    {
             AssetBundleMgr.Instance.LoadOrDownload(string.Format("Download/Prefab/RolePrefab/NPC/{0}.assetbundle", prefabName), prefabName,
            (GameObject obj)=>
            {
                if (onCompelete!=null)
                {
                    onCompelete(UnityEngine.Object.Instantiate(obj));
                }

            }

            );

        
    }
    #endregion

    public override void Dispose()
    {
        base.Dispose();
    }
}