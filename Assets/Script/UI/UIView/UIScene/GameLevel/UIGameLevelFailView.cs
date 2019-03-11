using UnityEngine;
using System.Collections;

public class UIGameLevelFailView : UIWindowViewBase
{
    /// <summary>
    /// 复活
    /// </summary>
    public System.Action OnResurgence;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnReturn":

                GlobalInit.Instance.CurrPlayer.ToResurgence(RoleIdleState.IdleFight);
                PlayerCtrl.Instance.LastInWorldMapPos = string.Empty; //把最后进入的世界地图坐标清空 因为不是通过传送点传输的 所以为了防止坐标错位 就清空坐标
                SceneMgr.Instance.LoadToWorldMap(2);
                break;
            case "btnResurgence":
                if (OnResurgence != null) OnResurgence();
                break;
        }
    }
}
