using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下载数据的实体
/// </summary>
public class DownloadDataEntity
{
    /// <summary>
    /// 资源名称
    /// </summary>
    public string FullName;
    /// <summary>
    /// MD5
    /// </summary>
    public string MD5;
    /// <summary>
    /// 资源大小(k)
    /// </summary>
    public int Size;
    /// <summary>
    /// 是否初始资源
    /// </summary>
    public bool IsFirstData;
}
