using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelSceneCtrl : GameSceneCtrlBase
{
    [SerializeField]
    private GameLevelRegionCtrl[] allRegion;
    /// <summary>
    /// 表格中的区域列表
    /// </summary>
    private List<GameLevelRegionEntity> regionLst = new List<GameLevelRegionEntity>();
    /// <summary>
    /// 当前关卡ID
    /// </summary>
    private int currGameLevelId;
    /// <summary>
    /// 当前进入区域的索引号
    /// </summary>
    private int currRegionIndex;
    /// <summary>
    /// 当前难度等级
    /// </summary>
    private GameLevelGrade currGrade;
    /// <summary>
    /// 本关卡怪的总数
    /// </summary>
    private int allMonsterCount;
    /// <summary>
    /// 怪的种类
    /// </summary>
    private int[] m_MonsterId;
    /// <summary>
    /// 当前区域怪的总数量
    /// </summary>
    private int currRegionMonsterCount;
    /// <summary>
    /// 当前区域控制器
    /// </summary>
    private GameLevelRegionCtrl currRegionCtrl;
    /// <summary>
    /// 当前创建区域怪的数量
    /// </summary>
    private int currCreateRegionMonsterCount;
    /// <summary>
    /// 当前杀死怪的数量
    /// </summary>
    private int currKillRegionMonsterCount;
    /// <summary>
    /// 当前区域编号
    /// </summary>
    private int m_CurrRegionId;
    /// <summary>
    /// 当前怪物实体
    /// </summary>
    //private List< GameLevelMonsterEntity> regionMonsterEntitylst=new List<GameLevelMonsterEntity>();
    /// <summary>
    /// 当前怪的详情 每种怪有多少
    /// </summary>
    private Dictionary<int, int> m_RegionMonsterDic;
    /// <summary>
    /// 怪物池
    /// </summary>
    private SpawnPool monsterPool;
    /// <summary>
    /// 战斗的时间
    /// </summary>
    private float m_FightTime=0;
    /// <summary>
    /// 是否在战斗中
    /// </summary>
    private bool m_IsFighing;
    private int tempMonsterId;
    private int m_Index = 0;
    List<int> spriteList;

    public static GameLevelSceneCtrl Instance;
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        currGameLevelId = SceneMgr.Instance.CurrGameLevelId;
        currGrade= SceneMgr.Instance.CurrGameLevelGrade;

        GameLevelCtrl.Instance.CurrGameLevelGrade = currGrade;
        GameLevelCtrl.Instance.CurrGameLevelId = currGameLevelId;

    }
    protected override void OnStart()
    {
        base.OnStart();
        GameLevelEntity entity = GameLevelDBModel.Instance.Get(currGameLevelId);
        if (entity != null)
        {
            AudioBackGroundMgr.Instance.Play(entity.Audio_BG);
        }
        
        m_IsFighing = true;
        m_RegionMonsterDic=new Dictionary<int, int>();
        spriteList = new List<int>();
        GameLevelCtrl.Instance.CurrGameLevleGold = 0;
        GameLevelCtrl.Instance.CurrGameLevleExp = 0;
        GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic.Clear();
        GameLevelCtrl.Instance.CurrGameLevelGetGoodsLst.Clear();
    }
    /// <summary>
    /// 下次刷怪时间
    /// </summary>
    private float nextCreateMonsterTime = 0;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (m_IsFighing)
        {
            m_FightTime += Time.deltaTime;


            if (currCreateRegionMonsterCount < currRegionMonsterCount)
            {

                if (Time.time > nextCreateMonsterTime)
                {
                    nextCreateMonsterTime = Time.deltaTime + 1f;
                    CreateMonster();
                }

            }
        }
    }
    /// <summary>
    /// 创建怪
    /// </summary>
    private void  CreateMonster()
    {
        //if (currCreateRegionMonsterCount >= 1) return;
        spriteList.Clear();
        foreach (var item in m_RegionMonsterDic)
        {
            spriteList.Add(item.Key);
        }

        m_Index = UnityEngine.Random.Range(0, m_RegionMonsterDic.Count);

        int monsterId = spriteList[m_Index];

        //GameObject obj = Instantiate(monsterGameObjectDic[monsterId]);

        ///缓存池取怪
        Transform tran = monsterPool.Spawn(SpriteDBModel.Instance.Get(monsterId).PrefabName);
       
        Transform monsterBornPos =currRegionCtrl.MonsterBornPos[UnityEngine.Random.Range(0, currRegionCtrl.MonsterBornPos.Length)];        

        RoleCtrl monsterCtl = tran.GetComponent<RoleCtrl>();

        RoleInfoMonster roleInfoMonster = new RoleInfoMonster();

        SpriteEntity entity = SpriteDBModel.Instance.Get(monsterId);
        if (entity!=null)
        {
            roleInfoMonster.SpriteEntity = entity;
            roleInfoMonster.RoleId = ++tempMonsterId;
            roleInfoMonster.RoleNickName = entity.Name;
            roleInfoMonster.MaxHP = roleInfoMonster.CurrHP=entity.HP+100;
            roleInfoMonster.MaxMP = roleInfoMonster.MaxMP = entity.MP;
            roleInfoMonster.Level = entity.Level;
            roleInfoMonster.Attack = entity.Attack;
            roleInfoMonster.Defense = entity.Defense;
            roleInfoMonster.Hit = entity.Hit;
            roleInfoMonster.Dodge = entity.Dodge;
            roleInfoMonster.Cri = entity.Cri;
            roleInfoMonster.Res = entity.Res;
            roleInfoMonster.Fighting = entity.Fighting;

            monsterCtl.ViewRange = entity.Range_View+30;//怪的视野
            monsterCtl.Speed = entity.MoveSpeed;//怪的移动速度
        }

        monsterCtl.Init(RoleType.Monster, roleInfoMonster, new RoleMonsterAI(monsterCtl, roleInfoMonster));
        monsterCtl.OnRoleDestroy = OnRoleDestroy;
        monsterCtl.OnRoleDie = OnRoleDieCallBack;
        //怪出生
        monsterCtl.Born(monsterBornPos.TransformPoint(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f)));

        m_RegionMonsterDic[monsterId]--;

        if (m_RegionMonsterDic[monsterId]<=0)
        {
            m_RegionMonsterDic.Remove(monsterId);
        }
        currCreateRegionMonsterCount++;
    }

    /// <summary>
    /// 角色死亡回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoleDieCallBack(RoleCtrl ctr)
    {
        currKillRegionMonsterCount++;
    
        //处理金币 经验
        RoleInfoMonster roleInfoMonster = (RoleInfoMonster)ctr.CurrRoleInfo;

        GameLevelMonsterEntity entiy = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterEntity(currGameLevelId,currGrade,m_CurrRegionId, roleInfoMonster.SpriteEntity.Id);
        if (entiy!=null)
        {
            if (entiy.Exp>0)
            {
                UITipView.Instance.ShowTip(0, string.Format("+{0}", entiy.Exp));
                GameLevelCtrl.Instance.CurrGameLevleExp += entiy.Exp;
            }
            if (entiy.Gold > 0)
            {
                UITipView.Instance.ShowTip(1, string.Format("+{0}", entiy.Gold));
                GameLevelCtrl.Instance.CurrGameLevleGold += entiy.Gold;
            }
        }
        //添加杀怪详情
        if (GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic.ContainsKey(roleInfoMonster.SpriteEntity.Id))
        {
            GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic[roleInfoMonster.SpriteEntity.Id] += 1;
        }
        else
        {
            GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic[roleInfoMonster.SpriteEntity.Id] = 1;
        }

        //杀死怪数量大于 区域怪数量
        if (currKillRegionMonsterCount>=currRegionMonsterCount)
        {
            currRegionIndex++;
            //是否最后一个区域
            if (currRegionIndex>regionLst.Count-1)
            {
                m_IsFighing = false;
                GameLevelCtrl.Instance.CurrGameLevelPassTime = m_FightTime;

                TimeMgr.Instance.ChangeTimeScale(0.3f,3);
                StartCoroutine(ShowVictory());

                return;
            }
            //进入下一个区域
            EnterRegion(currRegionIndex);
        }
    }

    private IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(3f);
        GameLevelCtrl.Instance.OpenView(WindowUIType.GameLevelVictory);
    }

    /// <summary>
    /// 角色销毁委托
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoleDestroy(Transform obj)
    {
        //回池
        monsterPool.Despawn(obj);
    }

    protected override void OnLoadCityViewCompelete(GameObject obj)
    {
        base.OnLoadCityViewCompelete(obj);
        //设置左上角UI
        PlayerCtrl.Instance.SetMainCityRoleData();
        //根据关卡编号返回全部区域
        regionLst = GameLevelRegionDBModel.Instance.GetListByGameLevelId(currGameLevelId);
        allMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(currGameLevelId, currGrade);
        m_MonsterId = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterId(currGameLevelId, currGrade);
        ///创建怪物池
        monsterPool = PoolManager.Pools.Create("Monster");
        monsterPool.group.parent = null;
        monsterPool.group.localPosition = Vector3.zero;

        LoadMonster(0,OnLoadMonsterCallBack);

        ////创建预制池
        //for (int i = 0; i < m_MonsterId.Length; i++)
        //{
        //    //monsterGameObjectDic[m_MonsterId[i]] =RoleMgr.Instance.LoadSprite(m_MonsterId[i]);
        //    PrefabPool prefabPool= new PrefabPool(RoleMgr.Instance.LoadSprite(m_MonsterId[i]).transform);            
        //    prefabPool.preloadAmount = 5;//预加载数量
        //    prefabPool.cullDespawned = true;//开启自动清理
        //    prefabPool.cullAbove = 5;//不清除数量
        //    prefabPool.cullDelay = 2;//清除间隔
        //    prefabPool.cullMaxPerPass = 2;//每次清除数量

        //    monsterPool.CreatePrefabPool(prefabPool);
        //}
        
    }
    /// <summary>
    /// 怪加载完毕
    /// </summary>
    private void OnLoadMonsterCallBack()
    {
        //区索引  怪加载完毕后进入区域
        currRegionIndex = 0;
        EnterRegion(currRegionIndex);
    }

    /// <summary>
    /// 加载怪物
    /// </summary>
    /// <param name="index"></param>
    /// <param name="onComplete"></param>
    private void LoadMonster(int index,Action onComplete)
    {
        int monsterId = m_MonsterId[index];
        RoleMgr.Instance.LoadSprite(monsterId,
            (GameObject obj)=>
            {
                PrefabPool prefabPool = new PrefabPool(obj.transform);
                prefabPool.preloadAmount = 5;//预加载数量
                monsterPool.CreatePrefabPool(prefabPool);
                index++;
                if (index== m_MonsterId.Length)
                {
                    if (onComplete!=null)
                    {
                        onComplete();
                    }
                }
                else
                {
                    LoadMonster(index, onComplete);
                }   

            }

            );

    }


    /// <summary>
    /// 当前区域是否有怪
    /// </summary>
    /// <returns></returns>
    public bool CurrRegionHasMonster
    {
        get
        {
          return  currKillRegionMonsterCount < currRegionMonsterCount;
        }
         
    }
    /// <summary>
    /// 当前区域是否最后一个区域
    /// </summary>
    /// <returns></returns>
    public bool CurrRegionIsLast
    {
        get
        {
            return currRegionIndex > regionLst.Count-1;
        }

    }

    /// <summary>
    /// 下区域出生点
    /// </summary>
    public Vector3 NetxRegionPlayerBornPos
    {
        get
        {
            GameLevelRegionEntity entity = GetGameLevelRegionEntityByIndex(currRegionIndex);
            if (entity == null) return Vector3.zero;
            //区域Id
            int regionId = entity.RegionId;

            return GetRegionCtrlByRegionId(regionId).RoleBornPos.position;
        }
    }

    /// <summary>
    /// 根据索引号取得实体
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameLevelRegionEntity GetGameLevelRegionEntityByIndex(int index)
    {
        for (int i = 0; i < regionLst.Count; i++)
        {
            if (i==index)
            {
                return regionLst[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 根据区域编号获取区域控制器
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameLevelRegionCtrl GetRegionCtrlByRegionId(int regionId)
    {
        for (int i = 0; i < allRegion.Length; i++)
        {
            if (allRegion[i].regionId == regionId)
            {
                return allRegion[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 进入区域
    /// </summary>
    /// <param name="regionIndex"></param>
    private void EnterRegion(int regionIndex)
    {

       GameLevelRegionEntity  entity = GetGameLevelRegionEntityByIndex(regionIndex);
        if (entity == null) return;
        //区域Id
        int regionId = entity.RegionId;
  
        currRegionCtrl = GetRegionCtrlByRegionId(regionId);

        if (currRegionCtrl == null) return;

        currCreateRegionMonsterCount = 0;
        currKillRegionMonsterCount = 0;

        //销毁当前区域遮挡物
        if (currRegionCtrl.RegionMask!=null)
        {
            Destroy(currRegionCtrl.RegionMask);
        }

        //通往下一个区域的门

        if (regionIndex!=0)
        {
            //使用上次的区域编号
            GameLevelDoorCtrl toNextRegionDoor = currRegionCtrl.GetToNextRegionDoor(m_CurrRegionId);
            if (toNextRegionDoor!=null)
            {
                toNextRegionDoor.gameObject.SetActive(false);
                if (toNextRegionDoor.ConnectToDoor!=null)
                {
                    toNextRegionDoor.ConnectToDoor.gameObject.SetActive(false);
                }
            }
        }
        m_CurrRegionId = regionId;

        if (regionIndex==0)
        {
            
            if (GlobalInit.Instance.CurrPlayer != null)
            {
                GlobalInit.Instance.CurrPlayer.Born(currRegionCtrl.RoleBornPos.position);
                GlobalInit.Instance.CurrPlayer.ToIdle(RoleIdleState.IdleFight);

                GlobalInit.Instance.CurrPlayer.OnRoleDie = (RoleCtrl ctrl) =>
                  {
                      StartCoroutine(ShowFailView());
                  };
            }
            //  加载完毕
            if (DelegateDefine.Instance.OnSenceLoadOk != null)
            {
                DelegateDefine.Instance.OnSenceLoadOk();
            }
        }
        //刷怪
        currRegionMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(currGameLevelId, currGrade, regionId);


        List<GameLevelMonsterEntity> regionMonsterEntitylst = GameLevelMonsterDBModel.Instance.GetGameLevelMonster(currGameLevelId, currGrade, regionId);
        for (int i = 0; i < regionMonsterEntitylst.Count; i++)
        {
            if (m_RegionMonsterDic.ContainsKey(regionMonsterEntitylst[i].SpriteId))
            {
                m_RegionMonsterDic[regionMonsterEntitylst[i].SpriteId] += regionMonsterEntitylst[i].SpriteCount;
            }
            else
            {
                m_RegionMonsterDic[regionMonsterEntitylst[i].SpriteId] = regionMonsterEntitylst[i].SpriteCount;
            }
           
        }

    }

    private IEnumerator ShowFailView()
    {
        yield return new WaitForSeconds(3);

        GameLevelCtrl.Instance.OpenView(WindowUIType.GameLevelFail);
    }
}
