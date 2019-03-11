using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestMMOMemory : MonoBehaviour {


	void Start ()
    {

        //  AssetBundleMgr.Instance.LoadClone(@"Role/role_mainplayer.assetbundle", "Role_MainPlayer");
      AssetBundleLoaderAsync async= AssetBundleMgr.Instance.LoadAsync(@"Role/role_mainplayer.assetbundle", "Role_MainPlayer");

        async.OnLoadComplete = OnLoadCompete;


        //List<JobEntity> lst = JobDBModel.Instance.GetList();

        //for (int i = 0; i < lst.Count; i++)
        //{
        //    Debug.Log("name" + lst[i].Name);
        //}
        //JobEntity entity = JobDBModel.Instance.GetById(1);
        //if(entity!=null)
        //{
        //    Debug.Log(entity.Name);
        //}
        //if(!NetWorkHttp.Instance.IsBusy)
        //{
        //    JsonData jsonData = new JsonData();
        //    jsonData["type"] = 0;//0=注册，1=登陆
        //    jsonData["UserName"] = "xxx";
        //    jsonData["Pwd"] = "123";

        //   //NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account/3", CallBack);
        //    NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", PostCallBack,isPost:true,json:jsonData.ToJson());
        //}

        //NetWorkSocket.Instance.Connect("192.168.1.34", 1011);


        //GameLevel_EnterProto ge = new GameLevel_EnterProto();
        //ge.GameLevelId = 199;

        //NetWorkSocket.Instance.SendMsg(ge.ToArray());


        //EventDispatcher.Instance.AddEventListener(ProtoCodeDef.GameLevel_EnterReturn, GameLevel_EnterReturnCallBack);

        // EventDispatcher.Instance.RemoveEventListener(ProtoCodeDef.GameLevel_EnterReturn, GameLevel_EnterReturnCallBack);

    }

    private void OnLoadCompete(UnityEngine.Object obj)
    {
        Instantiate((GameObject)obj);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SceneMgr.Instance.LoadToCity();
        //}
    }

    private void GameLevel_EnterReturnCallBack(byte[] buffer)
    {
        GameLevel_EnterReturnProto ge = GameLevel_EnterReturnProto.GetProto(buffer);

        print("IsSuccess" + ge.IsSuccess);
           print("MessageId" + ge.MessageId);
    }

    private void PostCallBack(CallBackArgs obj)
    {
        if (obj.HasError)
        {
            Debug.Log(obj.ErrorMsg);
        }
        else
        {
           
            RetValue ret = JsonMapper.ToObject<RetValue>(obj.Value);
            Debug.Log(ret.Value);
        }

    }

    private void CallBack(CallBackArgs obj)
    {
        if(obj.HasError)
        {
            Debug.Log(obj.ErrorMsg);
        }
        else
        {
            RetAccountEntity entity = JsonMapper.ToObject<RetAccountEntity>(obj.Value);
            Debug.Log(entity.Pwd);
        }

      
      


    }
}
