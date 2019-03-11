//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2017-01-25 14:46:55
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIWorldMapFailView : UIWindowViewBase
{
    /// <summary>
    /// 复活委托
    /// </summary>
    public System.Action OnResurgence;

    [SerializeField]
    private Text lblTip;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnReturn":
                //返回主城
                PlayerCtrl.Instance.LastInWorldMapPos = string.Empty; //把最后进入的世界地图坐标清空 因为不是通过传送点传输的 所以为了防止坐标错位 就清空坐标
                GlobalInit.Instance.CurrPlayer.ToResurgence(RoleIdleState.IdleNormal);
                SceneMgr.Instance.LoadToWorldMap(2); //默认让角色 进入杭州
                break;
            case "btnResurgence":
                if (OnResurgence != null) OnResurgence();
                break;
        }
    }

    public void SetUI(string enemyNickName)
    {
        lblTip.SetText(string.Format("您已经被<color=#ff5400>{0}</color>杀死了\n继续修炼 加强装备 然后去报仇吧！", enemyNickName));
    }
}