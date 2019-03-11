using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 剧情管家控制器
/// </summary>
public class GameLevelCtrl : SystmCtrlBase<GameLevelCtrl>, ISystemCtrl
{
    public GameLevelCtrl()
    {
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.GameLevel_EnterReturn, OnGameLevelEnterReturn);
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.GameLevel_ResurgenceReturn, OnGameLevelResurgenceReturn);
    }


    /// <summary>
    /// 剧情关卡地图视图
    /// </summary>
    private UIGameLevelMapView  uIGameLevelMapView;
    /// <summary>
    /// 剧情关卡详情视图
    /// </summary>
    private UIGameLevelDetailView uIGameLevelDetailView;
    /// <summary>
    /// 胜利视图
    /// </summary>
    private UIGameLevelVictoryView uIGameLevelVictoryView;
    /// <summary>
    /// 失败视图
    /// </summary>
    private UIGameLevelFailView  uIGameLevelFailView;
    /// <summary>
    /// 当前游戏关卡ID
    /// </summary>
    public int CurrGameLevelId;
    /// <summary>
    /// 当前游戏关卡等级
    /// </summary>
    public GameLevelGrade CurrGameLevelGrade;
    /// <summary>
    /// 当前游戏关卡通关时间
    /// </summary>
    public float CurrGameLevelPassTime;
    /// <summary>
    /// 当前关卡获得的经验
    /// </summary>
    public int CurrGameLevleExp;
    /// <summary>
    /// 当前关卡获得的金币
    /// </summary>
    public int CurrGameLevleGold;
    /// <summary>
    /// 当前关卡杀怪的数量
    /// </summary>
    public Dictionary<int, int> CurrGameLevelKillMonsterDic = new Dictionary<int, int>();
    /// <summary>
    /// 当前关卡获得的物品列表
    /// </summary>
    public List<GetGoodsEntity> CurrGameLevelGetGoodsLst = new List<GetGoodsEntity>();

    private int m_GameLevelId;
    private GameLevelGrade m_Grate;
    /// <summary>
    /// 打开视图
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.GameLevelMap:
                OpenGameLevelMapView();
                break;
            case WindowUIType.GameLevelVictory:
                OpenGameLevelVictoryView();
                break;
            case WindowUIType.GameLevelFail:
                OpenGameLevelFailView();
                break;
        }
    }
    /// <summary>
    /// 打开游戏失败视图
    /// </summary>
    public void OpenGameLevelFailView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameLevelFail.ToString().ToString(), (GameObject obj) =>
        {
            uIGameLevelFailView = obj.GetComponent<UIGameLevelFailView>();
        });
        //uIGameLevelFailView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelFail).GetComponent<UIGameLevelFailView>();
        //发送战斗失败消息

        GameLevel_FailProto proto= new GameLevel_FailProto();
        proto.GameLevelId = CurrGameLevelId;
        proto.Grade = (byte)CurrGameLevelGrade;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());

        uIGameLevelFailView.OnResurgence = () =>
        {
            GameLevel_ResurgenceProto mProto = new GameLevel_ResurgenceProto();
            mProto.GameLevelId = CurrGameLevelId;
            mProto.Grade = (byte)CurrGameLevelGrade;
            mProto.Type = 0;
            NetWorkSocket.Instance.SendMsg(mProto.ToArray());

      
        };
    }
    /// <summary>
    /// 服务器返回复活消息
    /// </summary>
    /// <param name="p"></param>
    private void OnGameLevelResurgenceReturn(byte[] p)
    {
        GameLevel_ResurgenceReturnProto proto = GameLevel_ResurgenceReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            //玩家点击复活之后
            uIGameLevelFailView.Close();
            GlobalInit.Instance.CurrPlayer.ToResurgence(RoleIdleState.IdleFight);
        }
        else
        {
            MessageCtrl.Instance.Show("复活失败","复活失败");
        }
    }
    /// <summary>
    /// 打开游戏胜利视图
    /// </summary>
    public void OpenGameLevelVictoryView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameLevelVictory.ToString(), (GameObject obj) =>
        {
            uIGameLevelVictoryView = obj.GetComponent<UIGameLevelVictoryView>();
        });
        //uIGameLevelVictoryView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelVictory).GetComponent<UIGameLevelVictoryView>();
        //游戏关卡表
        GameLevelEntity gameLevelEntity = GameLevelDBModel.Instance.Get(CurrGameLevelId);
        //关卡难度表
        GameLevelGradeEntity gameLevelGradeEntity = GameLevelGradeDBModel.Instance.GetEntityByGameLevelIdAndGrade(CurrGameLevelId, CurrGameLevelGrade);
        if (gameLevelEntity == null || gameLevelGradeEntity == null) return;
        TransferData data = new TransferData();
        data.SetValue(ConstDefine.GameLevelExp, gameLevelGradeEntity.Exp);
        data.SetValue(ConstDefine.GameLevelGold, gameLevelGradeEntity.Gold);
        data.SetValue(ConstDefine.GameLevelDesc, gameLevelGradeEntity.Desc);
        data.SetValue(ConstDefine.GameLevelPassTime, CurrGameLevelPassTime);

        int star = 1;
        if (CurrGameLevelPassTime <= gameLevelGradeEntity.Star2)
        {
            star = 3;
        }
        else if (CurrGameLevelPassTime <= gameLevelGradeEntity.Star1)
        {
            star = 2;
        }
        data.SetValue(ConstDefine.GameLevelStar, star);


        #region 装备
        if (gameLevelGradeEntity.EquipList.Count > 0)
        {
            gameLevelGradeEntity.EquipList.Sort((GoodsEntity entity1, GoodsEntity entity2) => {
                if (entity1.Probability < entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability == entity2.Probability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });
        }
        List<TransferData> lstReward = new List<TransferData>();
        GoodsEntity entity = gameLevelGradeEntity.EquipList[0];

        TransferData EquoReward = new TransferData();
        EquoReward.SetValue(ConstDefine.GoodsId, entity.Id);
        EquoReward.SetValue(ConstDefine.GoodsName, entity.Name);
        EquoReward.SetValue(ConstDefine.GoodsType, GoodsType.Equip);
        lstReward.Add(EquoReward);
        CurrGameLevelGetGoodsLst.Add(new GetGoodsEntity() { GoodsType = 0, GoodsId = entity.Id, GoodsCount = 1 });
        #endregion

        #region 道具
        if (gameLevelGradeEntity.ItemList.Count > 0)
        {
            gameLevelGradeEntity.ItemList.Sort((GoodsEntity entity1, GoodsEntity entity2) => {
                if (entity1.Probability < entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability == entity2.Probability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });
        }
        List<TransferData> itemListReward = new List<TransferData>();
        GoodsEntity itemListentity = gameLevelGradeEntity.ItemList[0];

        TransferData ItemReward = new TransferData();
        ItemReward.SetValue(ConstDefine.GoodsId, entity.Id);
        ItemReward.SetValue(ConstDefine.GoodsName, entity.Name);
        ItemReward.SetValue(ConstDefine.GoodsType, GoodsType.Item);
        lstReward.Add(ItemReward);
        
        #endregion

        #region 材料
        if (gameLevelGradeEntity.MaterialList.Count > 0)
        {
            gameLevelGradeEntity.MaterialList.Sort((GoodsEntity entity1, GoodsEntity entity2) => {
                if (entity1.Probability < entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability == entity2.Probability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });
        }
        List<TransferData> materialReward = new List<TransferData>();
        GoodsEntity materialEntity = gameLevelGradeEntity.MaterialList[0];

        TransferData materialListReward = new TransferData();
        materialListReward.SetValue(ConstDefine.GoodsId, entity.Id);
        materialListReward.SetValue(ConstDefine.GoodsName, entity.Name);
        materialListReward.SetValue(ConstDefine.GoodsType, GoodsType.Material);
        lstReward.Add(materialListReward);

        CurrGameLevelGetGoodsLst.Add(new GetGoodsEntity() { GoodsType = 2, GoodsId = entity.Id, GoodsCount = 1 });
        #endregion

        data.SetValue(ConstDefine.GameLevelReward, lstReward);

        uIGameLevelVictoryView.SetUI(data);

        //==================================================
        CurrGameLevleExp += gameLevelGradeEntity.Exp;
        CurrGameLevleGold += gameLevelGradeEntity.Gold;


        //发送战斗结果给服务器
        GameLevel_VictoryProto proto = new GameLevel_VictoryProto();
        proto.GameLevelId = CurrGameLevelId;
        proto.Grade = (byte)CurrGameLevelGrade;
        proto.Star = (byte)star;
        proto.Exp = CurrGameLevleExp;
        proto.Gold = CurrGameLevleGold;
        proto.KillTotalMonsterCount = CurrGameLevelKillMonsterDic.Count;
        proto.KillMonsterList = new List<GameLevel_VictoryProto.MonsterItem>();
        foreach (var pair in CurrGameLevelKillMonsterDic)
        {
            GameLevel_VictoryProto.MonsterItem item = new GameLevel_VictoryProto.MonsterItem();
            item.MonsterId = pair.Key;
            item.MonsterCount = pair.Value;

            proto.KillMonsterList.Add(item);
        }

        proto.GoodsTotalCount = CurrGameLevelGetGoodsLst.Count;
        proto.GetGoodsList = new List<GameLevel_VictoryProto.GoodsItem>();
        for (int i = 0; i < CurrGameLevelGetGoodsLst.Count; i++)
        {
            GameLevel_VictoryProto.GoodsItem item = new GameLevel_VictoryProto.GoodsItem();
            item.GoodsType = (byte)CurrGameLevelGetGoodsLst[i].GoodsType;
            item.GoodsId = (byte)CurrGameLevelGetGoodsLst[i].GoodsId;
            item.GoodsCount = (byte)CurrGameLevelGetGoodsLst[i].GoodsCount;
            proto.GetGoodsList.Add(item);
        }
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    /// <summary>
    /// 打开关卡地图视图
    /// </summary>
    public void OpenGameLevelMapView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameLevelMap.ToString(), (GameObject obj) =>
        {
            uIGameLevelMapView = obj.GetComponent<UIGameLevelMapView>();
            TransferData data = new TransferData();
            //1章
            ChapterEntity entity = ChapterDBModel.Instance.Get(1);
            data.SetValue(ConstDefine.ChapterId, entity.Id);
            data.SetValue(ConstDefine.ChapterName, entity.ChapterName);
            data.SetValue(ConstDefine.ChapterBG, entity.BG_Pic);
            //2关卡
            List<GameLevelEntity> gameLevelList = GameLevelDBModel.Instance.GetListByChapterId(entity.Id);
            if (gameLevelList != null && gameLevelList.Count > 0)
            {
                List<TransferData> lst = new List<TransferData>();
                for (int i = 0; i < gameLevelList.Count; i++)
                {
                    TransferData childData = new TransferData();
                    childData.SetValue(ConstDefine.GameLevelId, gameLevelList[i].Id);
                    childData.SetValue(ConstDefine.GameLevelName, gameLevelList[i].Name);
                    childData.SetValue(ConstDefine.GameLevelPostion, gameLevelList[i].Postion);
                    childData.SetValue(ConstDefine.GameLevelisBoss, gameLevelList[i].isBoss);
                    childData.SetValue(ConstDefine.GameLevelIco, gameLevelList[i].Ico);
                    lst.Add(childData);
                }

                data.SetValue(ConstDefine.GameLevelList, lst);
            }

            uIGameLevelMapView.SetUI(data, OnGameLevelItemClick);
        });

      
    }

    private void OnGameLevelItemClick(int obj)
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.GameLevelDetail.ToString(), (GameObject oj) =>
        {
            uIGameLevelDetailView = oj.GetComponent<UIGameLevelDetailView>();
            SetGameLevelDetailData(obj, GameLevelGrade.Normal);
            uIGameLevelDetailView.OnBtnGradeClick = OnBtnGradeClick;
            uIGameLevelDetailView.OnBtnEnterClick = OnBtnEnterClick;

        });
       // uIGameLevelDetailView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameLevelDetail).GetComponent<UIGameLevelDetailView>();

    }


    /// <summary>
    /// 进入关卡按钮点击
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    private void OnBtnEnterClick(int gameLevelId, GameLevelGrade grade)
    {
       

        //告诉服务器进入某个关卡
        GameLevel_EnterProto proto = new GameLevel_EnterProto();
        proto.GameLevelId = gameLevelId;
        proto.Grade = (byte)grade;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());

        m_GameLevelId = gameLevelId;
        m_Grate = grade;

    }

    /// <summary>
    /// 服务器返回进入游戏关卡消息
    /// </summary>
    /// <param name="p"></param>
    private void OnGameLevelEnterReturn(byte[] p)
    {
        GameLevel_EnterReturnProto proto = GameLevel_EnterReturnProto.GetProto(p);
        if (proto.IsSuccess)
        {
            //跳转场景
            SceneMgr.Instance.LoadToGameLevel(m_GameLevelId, m_Grate);
        }
        
    }

    /// <summary>
    /// 难度按钮点击
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <param name="grade"></param>
    private void OnBtnGradeClick(int gameLevelId, GameLevelGrade grade)
    {
        SetGameLevelDetailData(gameLevelId, grade);
    }

    private void SetGameLevelDetailData(int gameLevelId,GameLevelGrade gard)
    {
        
        //游戏关卡表
        GameLevelEntity gameLevelEntity = GameLevelDBModel.Instance.Get(gameLevelId);
        //关卡难度表
        GameLevelGradeEntity gameLevelGradeEntity = GameLevelGradeDBModel.Instance.GetEntityByGameLevelIdAndGrade(gameLevelId, gard);
        if (gameLevelEntity == null || gameLevelGradeEntity == null) return;


        TransferData data = new TransferData();
        data.SetValue(ConstDefine.GameLevelId, gameLevelEntity.Id);
        data.SetValue(ConstDefine.GameLevelDlgPic, gameLevelEntity.DlgPic);
        data.SetValue(ConstDefine.GameLevelName, gameLevelEntity.Name);
        data.SetValue(ConstDefine.GameLevelExp, gameLevelGradeEntity.Exp);
        data.SetValue(ConstDefine.GameLevelGold, gameLevelGradeEntity.Gold);
        data.SetValue(ConstDefine.GameLevelDesc, gameLevelGradeEntity.Desc);
        data.SetValue(ConstDefine.GameLevelConditionDesc, gameLevelGradeEntity.ConditionDesc);
        data.SetValue(ConstDefine.GameLevelCommendFighting, gameLevelGradeEntity.CommendFighting);

        #region 装备
        if (gameLevelGradeEntity.EquipList.Count>0)
        {
            gameLevelGradeEntity.EquipList.Sort((GoodsEntity entity1, GoodsEntity entity2)=> {
                if (entity1.Probability<entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability==entity2.Probability)
                {
                    return 0;
                }
                else 
                {
                    return 1;
                }

            });
        }
        List<TransferData> lstReward = new List<TransferData>();
        GoodsEntity entity = gameLevelGradeEntity.EquipList[0];

        TransferData EquoReward = new TransferData();
        EquoReward.SetValue(ConstDefine.GoodsId,entity.Id);
        EquoReward.SetValue(ConstDefine.GoodsName, entity.Name);
        EquoReward.SetValue(ConstDefine.GoodsType, GoodsType.Equip);
        lstReward.Add(EquoReward);
        #endregion

        #region 道具
        if (gameLevelGradeEntity.ItemList.Count > 0)
        {
            gameLevelGradeEntity.ItemList.Sort((GoodsEntity entity1, GoodsEntity entity2) => {
                if (entity1.Probability < entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability == entity2.Probability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });
        }
        List<TransferData> itemListReward = new List<TransferData>();
        GoodsEntity itemListentity = gameLevelGradeEntity.ItemList[0];

        TransferData ItemReward = new TransferData();
        ItemReward.SetValue(ConstDefine.GoodsId, entity.Id);
        ItemReward.SetValue(ConstDefine.GoodsName, entity.Name);
        ItemReward.SetValue(ConstDefine.GoodsType, GoodsType.Item);
        lstReward.Add(ItemReward);
        #endregion

        #region 材料
        if (gameLevelGradeEntity.MaterialList.Count > 0)
        {
            gameLevelGradeEntity.MaterialList.Sort((GoodsEntity entity1, GoodsEntity entity2) => {
                if (entity1.Probability < entity2.Probability)
                {
                    return -1;
                }
                else if (entity1.Probability == entity2.Probability)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            });
        }
        List<TransferData> materialReward = new List<TransferData>();
        GoodsEntity materialEntity = gameLevelGradeEntity.MaterialList[0];

        TransferData materialListReward = new TransferData();
        materialListReward.SetValue(ConstDefine.GoodsId, entity.Id);
        materialListReward.SetValue(ConstDefine.GoodsName, entity.Name);
        materialListReward.SetValue(ConstDefine.GoodsType, GoodsType.Material);
        lstReward.Add(materialListReward);
        #endregion

        data.SetValue(ConstDefine.GameLevelReward, lstReward);

        uIGameLevelDetailView.SetUI(data);


    }





	

}
