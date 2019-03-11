using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelRegionCtrl : MonoBehaviour
{
    public int regionId;
    [SerializeField]
    public Transform RoleBornPos;

    [SerializeField]
    public Transform[] MonsterBornPos;
    [SerializeField]
    public GameLevelDoorCtrl[] AllDoor;
    /// <summary>
    /// 区域遮挡物
    /// </summary>
    public GameObject RegionMask;
    void Start ()
    {
        if (MonsterBornPos != null && MonsterBornPos.Length > 0)
        {
            for (int i = 0; i < MonsterBornPos.Length; i++)
            {
                Renderer render = MonsterBornPos[i].GetComponent<Renderer>();
                if (render!=null)
                {
                    render.enabled = false;

                }
               
            }
        }

        if (AllDoor != null && AllDoor.Length > 0)
        {
            for (int i = 0; i < AllDoor.Length; i++)
            {
                Renderer render = AllDoor[i].GetComponent<Renderer>();
                if (render != null)
                {
                    render.enabled = false;
                    AllDoor[i].OwnerRegionId= regionId;
                }

            }
        }
    }
	
    /// <summary>
    /// 获取通往下一个区域的门
    /// </summary>
    /// <param name="nextRegionId"></param>
    /// <returns></returns>
    public GameLevelDoorCtrl GetToNextRegionDoor(int nextRegionId)
    {
        if (AllDoor != null && AllDoor.Length > 0)
        {
            for (int i = 0; i < AllDoor.Length; i++)
            {
             
                if (AllDoor[i].ConnectToDoor.OwnerRegionId== nextRegionId)
                {
                    return AllDoor[i];
                }

            }
        }
        return null;

    }

    void Update () {
		
	}
}
