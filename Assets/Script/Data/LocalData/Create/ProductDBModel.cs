using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 商品管理类
/// </summary>
public class ProductDBModel : AbstractDBModel<ProductDBModel, ProductEntity>
{
    protected override string FileName
    {
        get
        {
            return "Product.data";
        }
    }

    protected override ProductEntity MakeEntity(GameDataTableParser parse)
    {
        ProductEntity entity = new ProductEntity();

        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.Name = parse.GetFieldValue("Name");
        entity.Price = parse.GetFieldValue("Price").ToFloat();
        entity.PicName = parse.GetFieldValue("PicName");
        entity.Desc = parse.GetFieldValue("Desc");

        return entity;
    }
}
