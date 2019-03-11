using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : Singleton<EffectMgr>
{

    /// <summary>
    /// 特效池
    /// </summary>
    private SpawnPool effectPool;
    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, Transform> effectDic = new Dictionary<string, Transform>();

    /// <summary>
    /// 加载技能
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public GameObject LoadEffect(string effectPath,string effectName)
    {
        return AssetBundleMgr.Instance.Load(string.Format("{0}.assetbundle", effectPath), effectName);      
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        ///创建池
        effectPool = PoolManager.Pools.Create("Effect");
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="effectName"></param>
    public void PlayEffect(string effectPath,string effectName,System.Action<Transform>onComplete)
    {
        if (effectPool == null) return;
        if (!effectDic.ContainsKey(effectName))
        {
            AssetBundleMgr.Instance.LoadOrDownload(effectPath+".assetbundle", effectName,
                (GameObject obj) =>
                {
                if (!effectDic.ContainsKey(effectName))
                {

                    //如果没播放过
                    effectDic[effectName] = obj.transform;

                    PrefabPool prefabPool = new PrefabPool(effectDic[effectName]);
                    prefabPool.preloadAmount = 0;//预加载数量
                    prefabPool.cullDespawned = true;//开启自动清理
                    prefabPool.cullAbove = 5;//不清除数量
                    prefabPool.cullDelay = 2;//清除间隔
                    prefabPool.cullMaxPerPass = 2;//每次清除数量

                    effectPool.CreatePrefabPool(prefabPool);
                    if (onComplete != null)
                    {
                        onComplete(effectPool.Spawn(effectDic[effectName]));
                    }
                    }
                    else
                    {
                        if (onComplete != null)
                        {
                            onComplete(effectPool.Spawn(effectDic[effectName]));
                        }
                    }

                }
                );

        }
        else
        {
            if (onComplete != null)
            {
                onComplete(effectPool.Spawn(effectDic[effectName]));
            }
        }

  
    }
    /// <summary>
    /// 销毁特效
    /// </summary>
    public void DestroyEffect(Transform effect,float delay)
    {
        effectPool.Despawn(effect, delay);
    }
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        effectDic.Clear();
        effectPool = null;
    }
}
