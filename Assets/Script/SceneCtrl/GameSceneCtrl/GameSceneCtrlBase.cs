using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSceneCtrlBase : MonoBehaviour
{

    /// <summary>
    /// ��UI
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
        //���������س�ֵ��Ϣ
        UIDispatcher.Instance.AddEventListener(ConstDefine.RechargeOk, OnRechargeOk);
    }
    /// <summary>
    /// ��ֵ�ɹ��ص�
    /// </summary>
    /// <param name="buffer"></param>
    private void OnRechargeOk(string[] buffer)
    {
        int money = buffer[0].ToInt();

        //����Ԫ��
        int oldMoney = GlobalInit.Instance.MainPlayerInfo.Money;
        int addMoney = money - oldMoney;
        GlobalInit.Instance.MainPlayerInfo.Money = money;
        GlobalInit.Instance.MainPlayerInfo.TotalRechargeMoney += addMoney;

        if (UIMainCityRoleInfoView.Instance!=null)
        {
            UIMainCityRoleInfoView.Instance.SetMoney(money);
        }

    }


    #region OnZoom ���������
    /// <summary>
    /// ���������
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

    #region OnPlayerClickGround ��ҵ��
    /// <summary>
    /// ��ҵ��
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
        //��ֹ��͸
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
                //�����������������
                //ʶ��������һ��Ƕ���

                //������ݳ������ͺ͵�ǰ�����Ƿ����ս����ʶ��
                if (SceneMgr.Instance.IsFightingScene)
                {
                    //����������Ϊ���� ʵ�ʹ�����ʱ�� ���ݾ����߼� ʶ����˻��Ƕ��� ��
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

    #region OnFingerDrag ��ָ����
    /// <summary>
    /// ��ָ����
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
