//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-06-21 22:07:53
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

public class T4MGround : MonoBehaviour
{
	void Start () 
	{
        if (GlobalInit.Instance == null) return;
        Renderer[] arr = GetComponentsInChildren<Renderer>(true);

        if (arr != null && arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].material.shader = GlobalInit.Instance.T4MShader;
            }
        }
        Destroy(this);
	}
}