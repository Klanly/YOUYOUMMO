using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 场景UI管理器
/// </summary>
public class UISceneCtrl : Singleton<UISceneCtrl>
{

    /// <summary>
    /// 场景UI类型
    /// </summary>
    public enum SceneUIType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        None,
        /// <summary>
        /// 登录
        /// </summary>
        LogOn,
        /// <summary>
        /// 加载
        /// </summary>
        Loading,
        /// <summary>
        /// 主城
        /// </summary>
        MainCity,
        /// <summary>
        /// 选人场景
        /// </summary>
        SelctRole,
    }

    /// <summary>
    /// 当前场景UI
    /// </summary>
    public UISceneViewBase CurrentUIScene;

    #region LoadSceneUI 加载场景UI

    public void LoadSceneUI(string path, XLuaCustomExport.OnCreate  OnCreate)
    {
        LoadSceneUI(SceneUIType.None,null, OnCreate,path);
    }
    /// <summary>
    /// 加载场景UI
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void LoadSceneUI(SceneUIType type,Action<GameObject> OnLoadCompelete, XLuaCustomExport.OnCreate OnCreate=null
        ,string path=null)
    {
        string strUIName = string.Empty;
        string NewPath = string.Empty;
        if (type!=SceneUIType.None)
        {
            switch (type)
            {
                case SceneUIType.LogOn:
                    strUIName = "UI_Root_LogOn";
                    break;
                case SceneUIType.Loading:
                    break;
                case SceneUIType.MainCity:
                    strUIName = "UI_Root_MainCity";
                    break;
                case SceneUIType.SelctRole:
                    strUIName = "UI_Root_SelectRole";
                    break;
            }
            NewPath = string.Format("Download/Prefab/UIPrefab/UIScene/{0}.assetbundle", strUIName);
        }
        else
        {
            NewPath = path;
        }
       
        AssetBundleMgr.Instance.LoadOrDownload(NewPath, strUIName,(GameObject obj)=> 
        {
            obj = UnityEngine.Object.Instantiate(obj);

            CurrentUIScene = obj.GetComponent<UISceneViewBase>();
            if (OnLoadCompelete!=null)
            {
                OnLoadCompelete(obj);
            }
            if (OnCreate!=null)
            {
                obj.GetOrCreatComponent<LuaViewBehaviour>();
               OnCreate(obj);
            }
        });

    }
    #endregion
}