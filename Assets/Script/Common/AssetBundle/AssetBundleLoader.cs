using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssetBundleLoader:IDisposable
{
    private AssetBundle bundle;

    public AssetBundleLoader(string path,bool isFullPath=false)
    {
        string fullPath = isFullPath? path: LocalFileMgr.Instance.LocalFilePath + path;

        bundle=AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(fullPath));

    }


    public T LoadAsset<T>(string name)where T: UnityEngine.Object
    {
        if (bundle == null) return default(T);
        return bundle.LoadAsset(name) as T;
    }

    public UnityEngine.Object LoadAsset(string name)
    {
        return bundle.LoadAsset(name);
    }
    public UnityEngine.Object[] LoadAllAssets()
    {
        return bundle.LoadAllAssets();
    }

    public void Dispose()
    {
        if (bundle!=null)
        {
            bundle.Unload(false);
        }
    }
}
