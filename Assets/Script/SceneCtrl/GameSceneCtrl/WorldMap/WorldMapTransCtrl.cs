using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 世界地图传送点控制器
/// </summary>
public class WorldMapTransCtrl : MonoBehaviour
{
    /// <summary>
    /// 传送点编号
    /// </summary>
    private int m_TransPosId;
    /// <summary>
    /// 要传送的目标场景Id
    /// </summary>
    private int m_TagetTransScennId;
    /// <summary>
    /// 目标场景传送点id
    /// </summary>
    private int m_TargetSceneTranId;
    /// <summary>
    /// 要传送的目标场景Id
    /// </summary>
    public int TagetTransScennId
    {
        get
        {
            return m_TagetTransScennId;
        }

        set
        {
            m_TagetTransScennId = value;
        }
    }

    void Start ()
    {
		
	}
	

    public void SetParam(int transPosId,int tragetTranSceneId, int targetSceneTranId)
    {
        m_TransPosId = transPosId;
        TagetTransScennId = tragetTranSceneId;
        m_TargetSceneTranId = targetSceneTranId;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            RoleCtrl ctrl = collider.gameObject.GetComponent<RoleCtrl>();
            if (ctrl!=null&& ctrl.CurrRoleType==RoleType.MainPlayer)
            {
                //设置目标世界地图传送点Id
                SceneMgr.Instance.TransWorldMapTransPosId = m_TargetSceneTranId;
                SceneMgr.Instance.LoadToWorldMap(TagetTransScennId);
            }
            
        }
       
    }

}
