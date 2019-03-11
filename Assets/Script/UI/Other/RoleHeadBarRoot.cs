using UnityEngine;
using System.Collections;
/// <summary>
/// Í·¶¥¸úËæRoot
/// </summary>
public class RoleHeadBarRoot : MonoBehaviour 
{
    public static RoleHeadBarRoot Instance;

    void Awake ()
	{
        Instance = this;
    }
}