using UnityEngine;
using System.Collections;
/// <summary>
/// ͷ������Root
/// </summary>
public class RoleHeadBarRoot : MonoBehaviour 
{
    public static RoleHeadBarRoot Instance;

    void Awake ()
	{
        Instance = this;
    }
}