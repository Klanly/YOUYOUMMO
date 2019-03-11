using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerPageItemView : MonoBehaviour
{
    private int m_PageIndex;

    private Text m_ServerPageName;


    public Action<int> OnGameServerPageClick;

    private void Awake()
    {
        m_ServerPageName = transform.Find("lblServerPageName").GetComponent<Text>();
        GetComponent<Button>().onClick.AddListener(GameServerPageClcik);
    }
    private void Start()
    {
       
    }

    private void GameServerPageClcik()
    {
        if (OnGameServerPageClick!=null)
        {
            OnGameServerPageClick(m_PageIndex);
        }
    }

    public void SetUI(RetGameServerPageEntity entity)
    {
        m_PageIndex = entity.PageIndex;
        m_ServerPageName.text = entity.Name;
    }


}
