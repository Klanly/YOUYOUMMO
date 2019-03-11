using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ͼ���͵������
/// </summary>
public class WorldMapTransCtrl : MonoBehaviour
{
    /// <summary>
    /// ���͵���
    /// </summary>
    private int m_TransPosId;
    /// <summary>
    /// Ҫ���͵�Ŀ�곡��Id
    /// </summary>
    private int m_TagetTransScennId;
    /// <summary>
    /// Ŀ�곡�����͵�id
    /// </summary>
    private int m_TargetSceneTranId;
    /// <summary>
    /// Ҫ���͵�Ŀ�곡��Id
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
                //����Ŀ�������ͼ���͵�Id
                SceneMgr.Instance.TransWorldMapTransPosId = m_TargetSceneTranId;
                SceneMgr.Instance.LoadToWorldMap(TagetTransScennId);
            }
            
        }
       
    }

}
