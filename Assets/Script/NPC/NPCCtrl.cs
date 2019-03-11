using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour {
    /// <summary>
    /// 昵称挂点
    /// </summary>
    [SerializeField]
    private Transform m_HeadBarPos;

    /// <summary>
    /// 头顶UI条
    /// </summary>
    private GameObject m_HeadBar;

    private NPCEntity m_CurrNPEntity;


    private NPCHeadBarView m_NPCHeadBarView;

    /// <summary>
    /// 说的话
    /// </summary>
    private string[] m_NPCTalk;
    void Start () {
        InitHeadBar();


    }

    public void Init(NPCWorldMapData data)
    {
        m_CurrNPEntity = NPCDBModel.Instance.Get(data.NPCId);

        m_NPCTalk = m_CurrNPEntity.Talk.Split('|');
    }

    /// <summary>
    /// 初始化头顶UI条
    /// </summary>
    private void InitHeadBar()
    {
        if (RoleHeadBarRoot.Instance == null) return;
        if (m_CurrNPEntity == null) return;
        if (m_HeadBarPos == null) return;
        AssetBundleMgr.Instance.LoadOrDownload("Download/Prefab/UIPrefab/UIOther/NPCHeadBar.assetbundle", "NPCHeadBar", (GameObject obj) =>
        {
            m_HeadBar = Instantiate(obj);
            m_HeadBar.transform.parent = RoleHeadBarRoot.Instance.gameObject.transform;
            m_HeadBar.transform.localScale = Vector3.one;
            m_HeadBar.transform.localPosition = Vector3.zero;
           
            m_NPCHeadBarView = m_HeadBar.GetComponent<NPCHeadBarView>();
           
            //给预设赋值
            m_NPCHeadBarView.Init(m_HeadBarPos, m_CurrNPEntity.Name);
        });
    }

    private float nextTalkTime;
    void Update () {

        if (Time.time> nextTalkTime)
        {
            nextTalkTime = Time.time + 10f;

            if (m_NPCHeadBarView != null&& m_NPCTalk.Length>0)
            {
                m_NPCHeadBarView.Talk(m_NPCTalk[Random.Range(0, m_NPCTalk.Length)], 3f);
            }
        }
       
		
	}
}
