using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerSelectView : UIWindowViewBase
{
    /// <summary>
    /// 服务器状态
    /// </summary>
    [SerializeField]
    private Sprite[] m_GameServerStatus;
    /// <summary>
    /// 当前服务器状态
    /// </summary>
    [SerializeField]
    private Image m_CurrGameServerStatus;
    /// <summary>
    /// 服务器名称
    /// </summary>
    [SerializeField]
    private Text m_GameServerName;

    public void SetSelectUI(RetGameServerEntity entity)
    {
        m_CurrGameServerStatus.overrideSprite = m_GameServerStatus[entity.RunStatus];
        m_GameServerName.text = entity.Name;
       

    }

    #region 页签
    /// <summary>
    /// 页签预制体
    /// </summary>
    [SerializeField]
    private GameObject gameServerPageItemPrefab;
    /// <summary>
    /// 页签列表
    /// </summary>
    private GridLayoutGroup gameServerPageGrid;

    public Action<int> OnPageClick;
    private List<GameObject> m_GameServerObjLst = new List<GameObject>();
    public Action<RetGameServerEntity> OnGameServerClick;

    protected override void OnStart()
    {
        base.OnStart();

        gameServerPageGrid = Global.FindChild<GridLayoutGroup>(transform, "GameServerPageGrid");

        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(gameServerItemPrefab) as GameObject;
            obj.transform.parent = gameServerGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            m_GameServerObjLst.Add(obj);
       

        }


        }
    #region 页签 SetGameServerPageUI
    public void SetGameServerPageUI(List<RetGameServerPageEntity>lst)
    {
        if (lst == null||gameServerPageItemPrefab==null) return;

        for (int i = 0; i < lst.Count; i++)
        {
            GameObject obj = Instantiate(gameServerPageItemPrefab) as GameObject;
            obj.transform.parent = gameServerPageGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            
            UIGameServerPageItemView view= obj.GetComponent<UIGameServerPageItemView>();
            if (view!=null)
            {
                view.SetUI(lst[i]);
                view.OnGameServerPageClick = OnGameServerPageClick;


            }

        }
       
    }

    private void OnGameServerPageClick(int pageIndex)
    {
        if (OnPageClick!=null)
        {
            OnPageClick(pageIndex);
        }
        DebugApp.Log("点击了" + pageIndex + "页");
    }
    #endregion
    #endregion

    #region 服务器列表

    /// <summary>
    /// 服务器列表预制体
    /// </summary>
    [SerializeField]
    private GameObject gameServerItemPrefab;
    /// <summary>
    /// 服务器列表列表
    /// </summary>
     [SerializeField]
    private GridLayoutGroup gameServerGrid;
    /// <summary>
    /// 设置服务器列表
    /// </summary>
    /// <param name="lst"></param>
    public void SetGameServerUI(List<RetGameServerEntity> lst)
    {
        if (lst == null || gameServerItemPrefab == null) return;

        for (int i = 0; i < m_GameServerObjLst.Count; i++)
        {
            if (i> lst.Count-1)
            {
                m_GameServerObjLst[i].SetActive(false);
            }
        }
            
       
        for (int i = 0; i < lst.Count; i++)
        {

            GameObject obj = m_GameServerObjLst[i];
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
            
            UIGameServerItemView view = obj.GetComponent<UIGameServerItemView>();
            if (view != null)
            {
                view.SetUI(lst[i]);
                view.OnGameServerClick = OnGameServerClickCallBack;


            }

        }

    }

    private void OnGameServerClickCallBack(RetGameServerEntity obj)
    {
        if (OnGameServerClick!=null)
        {
            OnGameServerClick(obj);
        }
    }

    //private void OnGameServerClick(int obj)
    //{
    //    DebugApp.DebugLog("点击了" + obj + "页");
    //}



    #endregion
}
