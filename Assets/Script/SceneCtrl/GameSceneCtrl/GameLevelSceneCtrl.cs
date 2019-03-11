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
    /// ����е������б�
    /// </summary>
    private List<GameLevelRegionEntity> regionLst = new List<GameLevelRegionEntity>();
    /// <summary>
    /// ��ǰ�ؿ�ID
    /// </summary>
    private int currGameLevelId;
    /// <summary>
    /// ��ǰ���������������
    /// </summary>
    private int currRegionIndex;
    /// <summary>
    /// ��ǰ�Ѷȵȼ�
    /// </summary>
    private GameLevelGrade currGrade;
    /// <summary>
    /// ���ؿ��ֵ�����
    /// </summary>
    private int allMonsterCount;
    /// <summary>
    /// �ֵ�����
    /// </summary>
    private int[] m_MonsterId;
    /// <summary>
    /// ��ǰ����ֵ�������
    /// </summary>
    private int currRegionMonsterCount;
    /// <summary>
    /// ��ǰ���������
    /// </summary>
    private GameLevelRegionCtrl currRegionCtrl;
    /// <summary>
    /// ��ǰ��������ֵ�����
    /// </summary>
    private int currCreateRegionMonsterCount;
    /// <summary>
    /// ��ǰɱ���ֵ�����
    /// </summary>
    private int currKillRegionMonsterCount;
    /// <summary>
    /// ��ǰ������
    /// </summary>
    private int m_CurrRegionId;
    /// <summary>
    /// ��ǰ����ʵ��
    /// </summary>
    //private List< GameLevelMonsterEntity> regionMonsterEntitylst=new List<GameLevelMonsterEntity>();
    /// <summary>
    /// ��ǰ�ֵ����� ÿ�ֹ��ж���
    /// </summary>
    private Dictionary<int, int> m_RegionMonsterDic;
    /// <summary>
    /// �����
    /// </summary>
    private SpawnPool monsterPool;
    /// <summary>
    /// ս����ʱ��
    /// </summary>
    private float m_FightTime=0;
    /// <summary>
    /// �Ƿ���ս����
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
    /// �´�ˢ��ʱ��
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
    /// ������
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

        ///�����ȡ��
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

            monsterCtl.ViewRange = entity.Range_View+30;//�ֵ���Ұ
            monsterCtl.Speed = entity.MoveSpeed;//�ֵ��ƶ��ٶ�
        }

        monsterCtl.Init(RoleType.Monster, roleInfoMonster, new RoleMonsterAI(monsterCtl, roleInfoMonster));
        monsterCtl.OnRoleDestroy = OnRoleDestroy;
        monsterCtl.OnRoleDie = OnRoleDieCallBack;
        //�ֳ���
        monsterCtl.Born(monsterBornPos.TransformPoint(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f)));

        m_RegionMonsterDic[monsterId]--;

        if (m_RegionMonsterDic[monsterId]<=0)
        {
            m_RegionMonsterDic.Remove(monsterId);
        }
        currCreateRegionMonsterCount++;
    }

    /// <summary>
    /// ��ɫ�����ص�
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoleDieCallBack(RoleCtrl ctr)
    {
        currKillRegionMonsterCount++;
    
        //������ ����
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
        //���ɱ������
        if (GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic.ContainsKey(roleInfoMonster.SpriteEntity.Id))
        {
            GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic[roleInfoMonster.SpriteEntity.Id] += 1;
        }
        else
        {
            GameLevelCtrl.Instance.CurrGameLevelKillMonsterDic[roleInfoMonster.SpriteEntity.Id] = 1;
        }

        //ɱ������������ ���������
        if (currKillRegionMonsterCount>=currRegionMonsterCount)
        {
            currRegionIndex++;
            //�Ƿ����һ������
            if (currRegionIndex>regionLst.Count-1)
            {
                m_IsFighing = false;
                GameLevelCtrl.Instance.CurrGameLevelPassTime = m_FightTime;

                TimeMgr.Instance.ChangeTimeScale(0.3f,3);
                StartCoroutine(ShowVictory());

                return;
            }
            //������һ������
            EnterRegion(currRegionIndex);
        }
    }

    private IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(3f);
        GameLevelCtrl.Instance.OpenView(WindowUIType.GameLevelVictory);
    }

    /// <summary>
    /// ��ɫ����ί��
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoleDestroy(Transform obj)
    {
        //�س�
        monsterPool.Despawn(obj);
    }

    protected override void OnLoadCityViewCompelete(GameObject obj)
    {
        base.OnLoadCityViewCompelete(obj);
        //�������Ͻ�UI
        PlayerCtrl.Instance.SetMainCityRoleData();
        //���ݹؿ���ŷ���ȫ������
        regionLst = GameLevelRegionDBModel.Instance.GetListByGameLevelId(currGameLevelId);
        allMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(currGameLevelId, currGrade);
        m_MonsterId = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterId(currGameLevelId, currGrade);
        ///���������
        monsterPool = PoolManager.Pools.Create("Monster");
        monsterPool.group.parent = null;
        monsterPool.group.localPosition = Vector3.zero;

        LoadMonster(0,OnLoadMonsterCallBack);

        ////����Ԥ�Ƴ�
        //for (int i = 0; i < m_MonsterId.Length; i++)
        //{
        //    //monsterGameObjectDic[m_MonsterId[i]] =RoleMgr.Instance.LoadSprite(m_MonsterId[i]);
        //    PrefabPool prefabPool= new PrefabPool(RoleMgr.Instance.LoadSprite(m_MonsterId[i]).transform);            
        //    prefabPool.preloadAmount = 5;//Ԥ��������
        //    prefabPool.cullDespawned = true;//�����Զ�����
        //    prefabPool.cullAbove = 5;//���������
        //    prefabPool.cullDelay = 2;//������
        //    prefabPool.cullMaxPerPass = 2;//ÿ���������

        //    monsterPool.CreatePrefabPool(prefabPool);
        //}
        
    }
    /// <summary>
    /// �ּ������
    /// </summary>
    private void OnLoadMonsterCallBack()
    {
        //������  �ּ�����Ϻ��������
        currRegionIndex = 0;
        EnterRegion(currRegionIndex);
    }

    /// <summary>
    /// ���ع���
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
                prefabPool.preloadAmount = 5;//Ԥ��������
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
    /// ��ǰ�����Ƿ��й�
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
    /// ��ǰ�����Ƿ����һ������
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
    /// �����������
    /// </summary>
    public Vector3 NetxRegionPlayerBornPos
    {
        get
        {
            GameLevelRegionEntity entity = GetGameLevelRegionEntityByIndex(currRegionIndex);
            if (entity == null) return Vector3.zero;
            //����Id
            int regionId = entity.RegionId;

            return GetRegionCtrlByRegionId(regionId).RoleBornPos.position;
        }
    }

    /// <summary>
    /// ����������ȡ��ʵ��
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
    /// ���������Ż�ȡ���������
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
    /// ��������
    /// </summary>
    /// <param name="regionIndex"></param>
    private void EnterRegion(int regionIndex)
    {

       GameLevelRegionEntity  entity = GetGameLevelRegionEntityByIndex(regionIndex);
        if (entity == null) return;
        //����Id
        int regionId = entity.RegionId;
  
        currRegionCtrl = GetRegionCtrlByRegionId(regionId);

        if (currRegionCtrl == null) return;

        currCreateRegionMonsterCount = 0;
        currKillRegionMonsterCount = 0;

        //���ٵ�ǰ�����ڵ���
        if (currRegionCtrl.RegionMask!=null)
        {
            Destroy(currRegionCtrl.RegionMask);
        }

        //ͨ����һ���������

        if (regionIndex!=0)
        {
            //ʹ���ϴε�������
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
            //  �������
            if (DelegateDefine.Instance.OnSenceLoadOk != null)
            {
                DelegateDefine.Instance.OnSenceLoadOk();
            }
        }
        //ˢ��
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
