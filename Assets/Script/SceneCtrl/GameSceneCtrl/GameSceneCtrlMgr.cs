using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCtrlMgr : MonoBehaviour
{
    /// <summary>
    /// 游戏关卡场景主控制器
    /// </summary>
    [SerializeField]
    private GameLevelSceneCtrl gameLevelSceneCtrl;

    /// <summary>
    /// 世界地图主控制器
    /// </summary>
    [SerializeField]
    private WorldMapSceneCtrl worldMapSceneCtrl;

    private Dictionary<SceneType, GameObject> m_Dic = new Dictionary<SceneType, GameObject>();
    [SerializeField]
    private Transform Ground;

    void Awake ()
    {
        if (gameLevelSceneCtrl!=null)
        {
            m_Dic[SceneType.GameLevel] = gameLevelSceneCtrl.gameObject;
        }
        if (worldMapSceneCtrl != null)
        {
            m_Dic[SceneType.WorldMap] = worldMapSceneCtrl.gameObject;
        }

        GameObject obj = m_Dic[SceneMgr.Instance.CurrentSceneType];
        if (obj!=null)
        {
            obj.SetActive(true);
        }

        foreach (var item in m_Dic)
        {
            if (item.Key!= SceneMgr.Instance.CurrentSceneType)
            {
                Destroy(item.Value);
            }
        }

        Renderer[] groundRender = Ground.GetComponentsInChildren<Renderer>();
        if (groundRender!=null&& groundRender.Length>0)
        {
            for (int i = 0; i < groundRender.Length; i++)
            {
                groundRender[i].enabled = false;
            }
        }

    }
	

}
