
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIGameLevelMapView : UIWindowViewBase
{
    /// <summary>
    /// 章名称
    /// </summary>
    [SerializeField]
    private Text txtChapterName;

    /// <summary>
    /// 背景图
    /// </summary>
    [SerializeField]
    private RawImage imgMap;

    /// <summary>
    /// 章编号
    /// </summary>
    [HideInInspector]
    private int m_ChapterId;

    /// <summary>
    /// 连线点容器
    /// </summary>
    [SerializeField]
    private Transform pointContainer;

    private List<Transform> m_GameLevelItems = new List<Transform>();

    private List<TransferData> m_lstData;

    public Action<int> OnGameLevelItemClick;

    protected override void OnStart()
    {
        base.OnStart();

       StartCoroutine(CreateItem());
    }

    private IEnumerator CreateItem()
    {
        if (m_lstData == null) yield break;

        if (m_lstData != null && m_lstData.Count > 0)
        {
            m_GameLevelItems.Clear();

            AssetBundleMgr.Instance.LoadOrDownload("Download/Prefab/UIPrefab/UIWindowsChild/GameLevel/GameLevelMapItem.assetbundle", "GameLevelMapItem", (GameObject obj) =>
            {
                StartCoroutine(LoadItem(obj));
            });
        }
    }

    private IEnumerator LoadItem(GameObject obj)
    {
        for (int i = 0; i < m_lstData.Count; i++)
        {
            obj = Instantiate(obj);

            obj.SetParent(imgMap.transform);
            Vector2 pos = m_lstData[i].GetValue<Vector2>(ConstDefine.GameLevelPostion);
            obj.transform.localPosition = new Vector3(pos.x, pos.y, 0);

            UIGameLevelMapItemView view = obj.GetComponent<UIGameLevelMapItemView>();
            view.SetUI(m_lstData[i], OnGameLevelItemClick);

            m_GameLevelItems.Add(obj.transform);

            yield return null;
        }

        AssetBundleMgr.Instance.LoadOrDownload("Download/Prefab/UIPrefab/UIWindowsChild/GameLevel/GameLevelMapPoint.assetbundle", "GameLevelMapPoint", (GameObject gameObject1) =>
        {
            StartCoroutine(LoadPoint(gameObject1));
        });
    }

    private IEnumerator LoadPoint(GameObject obj)
    {
        for (int i = 0; i < m_GameLevelItems.Count; i++)
        {
            if (i == m_GameLevelItems.Count - 1) break;

            //起始点
            Transform transBegin = m_GameLevelItems[i];

            //结束点
            Transform transEnd = m_GameLevelItems[i + 1];

            //计算两点的距离
            float distance = Vector2.Distance(transBegin.localPosition, transEnd.localPosition);

            //计算生成多少个连线点
            int createCount = Mathf.FloorToInt(distance / 20f);

            float xLen = transEnd.localPosition.x - transBegin.localPosition.x;
            float yLen = transEnd.localPosition.y - transBegin.localPosition.y;

            //xy的递增
            float stepX = xLen / createCount;
            float stepY = yLen / createCount;

            //创建点
            for (int j = 0; j < createCount; j++)
            {
                if (j < 1 || j > createCount - 1) continue;

                //克隆点
                GameObject objPoint = Instantiate(obj);

                objPoint.SetParent(pointContainer);
                objPoint.transform.localPosition = new Vector3(transBegin.transform.localPosition.x + (stepX * j), transBegin.transform.localPosition.y + (stepY * j), 0f);

                UIGameLevelMapPointView view = objPoint.GetComponent<UIGameLevelMapPointView>();
                if (view != null)
                {
                    //todu:和服务器交互
                    view.SetUI(false);
                }

                yield return null;
            }
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        txtChapterName = null;
        imgMap = null;
    }
    //private IEnumerator CreateItem()
    //{
    //    if (m_lstData == null) yield break;
    //    if (m_lstData != null && m_lstData.Count > 0)
    //    {
    //        m_GameLevelItems.Clear();
    //        for (int i = 0; i < m_lstData.Count; i++)
    //        {
    //            //关卡项
    //            GameObject obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindowsChild, "GameLevel/GameLevelMapItem");
    //            obj.SetParent(imgMap.transform);
    //            Vector2 pos = m_lstData[i].GetValue<Vector2>(ConstDefine.GameLevelPostion);
    //            obj.transform.localPosition = new Vector3(pos.x, pos.y, 0);

    //            UIGameLevelMapItemView view = obj.GetComponent<UIGameLevelMapItemView>();

    //            view.SetUI(m_lstData[i], OnGameLevelItemClick);
    //            m_GameLevelItems.Add(obj.transform);
    //            yield return null;
    //        }
    //    }

    //    //===========生成连线点=================

    //    for (int i = 0; i < m_GameLevelItems.Count; i++)
    //    {
    //        if (i == m_GameLevelItems.Count - 1) break;

    //        Transform transBegin = m_GameLevelItems[i];

    //        Transform transEnd = m_GameLevelItems[i + 1];
    //        //计算两点的距离
    //        float distance = Vector2.Distance(transBegin.localPosition, transEnd.localPosition);

    //        //计算生成多少个连线点
    //        int createCount = Mathf.FloorToInt(distance / 20f);

    //        float xLen = transEnd.localPosition.x - transBegin.localPosition.x;
    //        float yLen = transEnd.localPosition.y - transBegin.localPosition.y;

    //        //xy的递增
    //        float stepX = xLen / createCount;
    //        float stepY = yLen / createCount;

    //        //创建点
    //        for (int j = 0; j < createCount; j++)
    //        {
    //            if (j < 1 || j > createCount - 1) continue;

    //            //克隆点
    //            GameObject objPoint = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindowsChild, "GameLevel/GameLevelMapPoint");

    //            objPoint.SetParent(pointContainer);
    //            objPoint.transform.localPosition = new Vector3(transBegin.transform.localPosition.x + (stepX * j), transBegin.transform.localPosition.y + (stepY * j), 0f);

    //            UIGameLevelMapPointView view = objPoint.GetComponent<UIGameLevelMapPointView>();
    //            if (view != null)
    //            {
    //                //todu:和服务器交互
    //                view.SetUI(false);
    //            }

    //             yield return null;
    //        }

    //    }

    //}

    public void SetUI(TransferData data, Action<int> onGameLevelItemClick)
    {
        OnGameLevelItemClick = onGameLevelItemClick;

        m_ChapterId = data.GetValue<int>(ConstDefine.ChapterId);
       txtChapterName.SetText(data.GetValue<string>(ConstDefine.ChapterName));
     //  imgMap.texture = GameUtil.LoadGameLevelMapPic(data.GetValue<string>(ConstDefine.ChapterBG));

        AssetBundleMgr.Instance.LoadOrDownload<Texture>(string.Format("Download/Source/UISource/UI/GameLevel/GameLevelMap/{0}.assetbundle", data.GetValue<string>(ConstDefine.ChapterBG)), data.GetValue<string>(ConstDefine.ChapterBG), (Texture obj) =>
        {
            imgMap.texture = obj;
        }, type: 1);

        //1.取到集合
        m_lstData = data.GetValue<List<TransferData>>(ConstDefine.GameLevelList);

      
    }
}