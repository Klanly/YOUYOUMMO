using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商品实体类
/// </summary>
public class ProductEntity : AbstractEntity
{

    /// <summary>
    /// 商品名称
    /// </summary>
    public string Name
    {
        get;
        set;
    }
    /// <summary>
    /// 价格
    /// </summary>
    public float Price
    {
        get;
        set;
    }
    /// <summary>
    /// 图片名称
    /// </summary>
    public string PicName
    {
        get;
        set;
    }
    /// <summary>
    /// 商品描述
    /// </summary>
    public string Desc
    {
        get;
        set;
    }

}
