using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerItemView : MonoBehaviour
{
    /// <summary>
    /// ������״̬
    /// </summary>
     [SerializeField]
    private Sprite[] m_GameServerStatus;
    /// <summary>
    /// ��ǰ������״̬
    /// </summary>
    [SerializeField]
    private Image m_CurrGameServerStatus;
    /// <summary>
    /// ����������
    /// </summary>
    [SerializeField]
    private Text m_GameServerName;

    private RetGameServerEntity m_CurrGameServerData;

    public Action<RetGameServerEntity> OnGameServerClick;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn!=null)
        {
            btn.onClick.AddListener(GameServerClick);
        }
  
    }

    private void GameServerClick()
    {
        if (OnGameServerClick!=null)
        {
            OnGameServerClick(m_CurrGameServerData);
        }
    }

    public void SetUI(RetGameServerEntity entity)
    {
        m_CurrGameServerStatus.overrideSprite = m_GameServerStatus[entity.RunStatus];
        m_GameServerName.text = entity.Name;
        m_CurrGameServerData = entity;

    }

}
