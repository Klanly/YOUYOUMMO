using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<LuaMgr>();
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        //������һ���ű�
        LuaMgr.Instance.DOString("require'Download/xLuaLogic/Main'");

    }
	


}
