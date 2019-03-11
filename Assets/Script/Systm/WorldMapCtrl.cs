using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldMapCtrl : SystmCtrlBase<WorldMapCtrl>, ISystemCtrl
{
    private UIWorldMapView uIWorldMapView;
    private UIWorldMapFailView uIWorldMapFailView;
    /// <summary>
    /// 启始场景id
    /// </summary>
    private int m_BegingSceneId;
    /// <summary>
    /// 目标场景Id
    /// </summary>
    private int m_TagetScnenId;
    /// <summary>
    /// 
    /// </summary>
    public Queue<int> SceneIdQueue;

    private Dictionary<int, WorldMapSceneEntity> m_WorldMapSceneDic;
    /// <summary>
    /// 是否查找结束
    /// </summary>
    private bool m_IsFindOver = false;
    /// <summary>
    /// 目标场景
    /// </summary>
    private WorldMapSceneEntity m_TargetScene;
    /// <summary>
    /// 临时存储数据
    /// </summary>
    private List<int> m_WorldMapSceneList;

    /// <summary>
    /// 当前场景id
    /// </summary>
    public int currSceneId;
    /// <summary>
    /// 要前往的场景id
    /// </summary>
    public int ToSceneId;
    /// <summary>
    /// 是否自动移动
    /// </summary>
    public bool isAutoMove;

    public Vector3 ToScenePos=Vector3.zero;

    public string EnemyNickName;

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.WorldMap:
                OpenWorldMapView();
                break;
            case WindowUIType.WorldMapFail:
                OpenWorldMapFailView();
                break;
            default:
                break;
        }
    }


    public void OpenWorldMapFailView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.WorldMapFail.ToString(), (GameObject oj) =>
        {
            uIWorldMapFailView = oj.GetComponent<UIWorldMapFailView>();

            uIWorldMapFailView.SetUI(EnemyNickName);

            //发复活消息给服务器
            uIWorldMapFailView.OnResurgence = () =>
            {
                WorldMap_CurrRoleResurgenceProto proto = new WorldMap_CurrRoleResurgenceProto();
                proto.Type = 0;//0==免费 1=元宝复活

                NetWorkSocket.Instance.SendMsg(proto.ToArray());

                uIWorldMapFailView.Close();

            };
        });
       // uIWorldMapFailView = UIViewUtil.Instance.OpenWindow(WindowUIType.WorldMapFail).GetComponent<UIWorldMapFailView>();


    }

    public void OpenWorldMapView()
    {
        //从本地 世界地图表 获取数据
        List<WorldMapEntity> worldMapLst = WorldMapDBModel.Instance.GetList();
        if (worldMapLst == null || worldMapLst.Count == 0) return;

        UIViewUtil.Instance.LoadWindow(WindowUIType.WorldMap.ToString(), (GameObject oj) =>
        {
            uIWorldMapView = oj.GetComponent<UIWorldMapView>();
            TransferData data = new TransferData();

            List<TransferData> lst = new List<TransferData>();

            for (int i = 0; i < worldMapLst.Count; i++)
            {
                WorldMapEntity worldMapEntity = worldMapLst[i];
                //如果不在地图上显示 则跳过
                if (worldMapEntity.IsShowInMap == 0) continue;

                TransferData chilData = new TransferData();
                chilData.SetValue(ConstDefine.WorldMapId, worldMapEntity.Id);
                chilData.SetValue(ConstDefine.WorldMapName, worldMapEntity.Name);
                chilData.SetValue(ConstDefine.WorldMapIco, worldMapEntity.IcoInMap);

                string[] arr = worldMapEntity.PosInMap.Split('_');
                Vector2 pos = new Vector2();
                if (arr.Length == 2)
                {
                    pos.x = float.Parse(arr[0]);
                    pos.y = float.Parse(arr[1]);
                }
                chilData.SetValue(ConstDefine.WorldMapPostion, pos);
                lst.Add(chilData);

            }
            data.SetValue(ConstDefine.WorldMapList, lst);

            uIWorldMapView.SetUI(data, OnWorldMapItemClick);
        });


      

    }

    private void OnWorldMapItemClick(int obj)
    {
        //1直接传送
        //SceneMgr.Instance.LoadToWorldMap(obj);
        //逐步传送
        CalculateTargetScenePath(SceneMgr.Instance.CurrWorldMapId,obj);
    }

    /// <summary>
    /// 计算到达目标场景的路径
    /// </summary>
    private void Calculate(int currSceneId)
    {
        if (!m_WorldMapSceneDic.ContainsKey(currSceneId)) return;

        //拿到当前场景实体
        WorldMapSceneEntity entity = m_WorldMapSceneDic[currSceneId];

        string[] arr = entity.NearScene.Split('_');
        for (int i = 0; i < arr.Length; i++)
        {
            if (m_IsFindOver) continue;

            int sceneId = int.Parse(arr[i]);//拿到关联场景ID
            if (sceneId == m_BegingSceneId) continue;
            WorldMapSceneEntity findScene= m_WorldMapSceneDic[sceneId];
            //如果访问过 就跳出
            if (findScene.IsVisit) continue;
             findScene.IsVisit = true;
            //设置父节点时候一定要在IsVisit之后
            findScene.Parent = entity;

            if (findScene.Id==m_TagetScnenId)
            {
                m_IsFindOver = true;
                m_TargetScene = findScene;
                break;
            }
            else
            {
                //从当前场景为出发点开始找
                Calculate(findScene.Id);
            }

        }
    }
    /// <summary>
    /// 计算到达目标场景的路径
    /// </summary>
    /// <param name="beginScnenid"></param>
    /// <param name="endScencId"></param>
    private void CalculateTargetScenePath(int beginScnenid,int endScencId)
    {
        List<WorldMapEntity> worldMapLst = WorldMapDBModel.Instance.GetList();

        if (m_WorldMapSceneDic==null)
        {
             m_WorldMapSceneDic = new Dictionary<int, WorldMapSceneEntity>();
            m_WorldMapSceneList = new List<int>();
            for (int i = 0; i < worldMapLst.Count; i++)
            {
                m_WorldMapSceneDic[worldMapLst[i].Id] = new WorldMapSceneEntity() { Id = worldMapLst[i].Id, NearScene = worldMapLst[i].NearScene, IsVisit = false };
            }
            SceneIdQueue = new Queue<int>();
        }
        m_BegingSceneId = beginScnenid;
        m_TagetScnenId = endScencId;
        SceneIdQueue.Clear();
        m_IsFindOver = false;

        //重置字典中的场景没有访问过
        for (int i = 0; i < worldMapLst.Count; i++)
        {
            m_WorldMapSceneDic[worldMapLst[i].Id].IsVisit = false;
            m_WorldMapSceneDic[worldMapLst[i].Id].Parent = null;
        }
        //计算地图寻路
        Calculate(m_BegingSceneId);

        if (m_TargetScene!=null)
        {
            m_WorldMapSceneList.Clear();
            GetParaentScene(m_TargetScene);
        }

 
        for (int i = m_WorldMapSceneList.Count-1; i >= 0; i--)
        {
            SceneIdQueue.Enqueue(m_WorldMapSceneList[i]);
        }

        //int count = SceneIdQueue.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    Debug.Log(SceneIdQueue.Dequeue());
        //}


        //计算完毕 开始移动
        if (SceneIdQueue.Count>=2)
        {
            isAutoMove = true;
            currSceneId = SceneIdQueue.Dequeue();
            ToSceneId= SceneIdQueue.Dequeue();
           

            uIWorldMapView.Close();

            if (WorldMapSceneCtrl.Instance!=null)
            {
                WorldMapSceneCtrl.Instance.AutoMove();
            }

        }
   

    }

    public void GetParaentScene(WorldMapSceneEntity entity)
    {
        m_WorldMapSceneList.Add(entity.Id);
        if (entity.Parent!=null)
        {
            GetParaentScene(entity.Parent);
        }
    }

    /// <summary>
    /// 进行世界地图寻路的实体
    /// </summary>
    public class WorldMapSceneEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id;
        /// <summary>
        /// 关联的场景
        /// </summary>
        public string NearScene;
        /// <summary>
        /// 是否访问过
        /// </summary>
        public bool IsVisit;
        /// <summary>
        /// 父节点
        /// </summary>
        public WorldMapSceneEntity Parent;

    }
}
