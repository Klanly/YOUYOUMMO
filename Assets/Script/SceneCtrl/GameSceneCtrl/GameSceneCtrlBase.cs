using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSceneCtrlBase : MonoBehaviour
{

    /// <summary>
    /// 主UI
    /// </summary>
    protected UISceneMainCityView m_MainCityView;
    void Awake()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
            FingerEvent.Instance.OnZoom += OnZoom;
            FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
        }
        OnAwake();
    }

    void Start()
    {
       
        UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.MainCity, OnLoadCityViewCompelete);
      //  m_MainCityView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.MainCity, OnLoadCityViewCompelete).GetComponent<UISceneMainCityView>();
      
    }
    protected virtual void OnLoadCityViewCompelete(GameObject obj)
    {
        m_MainCityView = obj.GetComponent<UISceneMainCityView>();
        OnStart();
        EffectMgr.Instance.Init();
        //服务器返回充值消息
        UIDispatcher.Instance.AddEventListener(ConstDefine.RechargeOk, OnRechargeOk);
    }
    /// <summary>
    /// 充值成功回调
    /// </summary>
    /// <param name="buffer"></param>
    private void OnRechargeOk(string[] buffer)
    {
        int money = buffer[0].ToInt();

        //更新元宝
        int oldMoney = GlobalInit.Instance.MainPlayerInfo.Money;
        int addMoney = money - oldMoney;
        GlobalInit.Instance.MainPlayerInfo.Money = money;
        GlobalInit.Instance.MainPlayerInfo.TotalRechargeMoney += addMoney;

        if (UIMainCityRoleInfoView.Instance!=null)
        {
            UIMainCityRoleInfoView.Instance.SetMoney(money);
        }

    }


    #region OnZoom 摄像机缩放
    /// <summary>
    /// 摄像机缩放
    /// </summary>
    /// <param name="obj"></param>
    private void OnZoom(FingerEvent.ZoomType obj)
    {
        switch (obj)
        {
            case FingerEvent.ZoomType.In:
                CameraCtrl.Instance.SetCameraZoom(0);
                break;
            case FingerEvent.ZoomType.Out:
                CameraCtrl.Instance.SetCameraZoom(1);
                break;
        }
    }
    #endregion

    #region OnPlayerClickGround 玩家点击
    /// <summary>
    /// 玩家点击
    /// </summary>
    private void OnPlayerClick()
    {
        if (SceneMgr.Instance.CurrentSceneType==SceneType.GameLevel)
        {
            if (GlobalInit.Instance.CurrPlayer.roleAttack.IsAutoFight)
            {
                return;
            }
        }
        //防止穿透
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        WorldMapCtrl.Instance.isAutoMove = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));
        if (hitArr.Length > 0)
        {
            RoleCtrl hitRole = hitArr[0].collider.gameObject.GetComponent<RoleCtrl>();
            if (hitRole.CurrRoleType == RoleType.Monster)
            {
                GlobalInit.Instance.CurrPlayer.LockEnemy = hitRole;
                return;
            }
            else if (hitRole.CurrRoleType == RoleType.OtherPlayer)
            {
                //如果点击到了其他玩家
                //识别其他玩家还是队友

                //这里根据场景类型和当前场景是否可以战斗来识别
                if (SceneMgr.Instance.IsFightingScene)
                {
                    //这两款设置为敌人 实际工作的时候， 根据具体逻辑 识别敌人还是队友 等
                    GlobalInit.Instance.CurrPlayer.LockEnemy = hitRole;
                    return;
                }

            

            }
        }
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000, 1 << LayerMask.NameToLayer("Ground")))
        {

            if (GlobalInit.Instance.CurrPlayer != null)
            {
                if (SceneMgr.Instance.CurrentSceneType == SceneType.GameLevel)
                {
                    Vector3 point = new Vector3(hitInfo.point.x, hitInfo.point.y + 50, hitInfo.point.z);
                    if (Physics.Raycast(point,new Vector3(0,-200,0), out hitInfo, 1000, 1 << LayerMask.NameToLayer("RegionMask")))
                    {
                        return;
                    }
                    //hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("RegionMask"));
                    //if (hitArr.Length > 0)
                    //{
                    //    return;
                    //}
                }

                GlobalInit.Instance.CurrPlayer.LockEnemy = null;
                GlobalInit.Instance.CurrPlayer.MoveTo(hitInfo.point);
            }


        }
    }
    #endregion

    #region OnFingerDrag 手指滑动
    /// <summary>
    /// 手指滑动
    /// </summary>
    /// <param name="obj"></param>
    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        switch (obj)
        {
            case FingerEvent.FingerDir.Left:
                CameraCtrl.Instance.SetCameraRotate(0);
                break;
            case FingerEvent.FingerDir.Right:
                CameraCtrl.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Up:
                CameraCtrl.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                CameraCtrl.Instance.SetCameraUpAndDown(0);
                break;
        }
    }
    #endregion

    void OnDestroy()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
            FingerEvent.Instance.OnZoom -= OnZoom;
            FingerEvent.Instance.OnPlayerClick -= OnPlayerClick;
        }
        BeforeOnDestroy();
        EffectMgr.Instance.Clear();
        UIDispatcher.Instance.RemoveEventListener(ConstDefine.RechargeOk, OnRechargeOk);
    }
    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void BeforeOnDestroy() { }


}
