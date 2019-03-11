//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-08-14 18:26:18
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameLevelRegionDBModel
{
    private List<GameLevelRegionEntity> lst = new List<GameLevelRegionEntity>();

    /// <summary>
    /// 根据游戏关卡编号 返回集合
    /// </summary>
    /// <param name="gameLevelId"></param>
    /// <returns></returns>
    public List<GameLevelRegionEntity> GetListByGameLevelId(int gameLevelId)
    {
        lst.Clear();

        for (int i = 0; i < m_List.Count; i++)
        {
            if (m_List[i].GameLevelId == gameLevelId)
            {
                lst.Add(m_List[i]);
            }
        }

        return lst;
    }
}