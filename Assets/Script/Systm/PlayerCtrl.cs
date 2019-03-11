using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerCtrl : SystmCtrlBase<PlayerCtrl>, ISystemCtrl
{
    private UIRoleInfoView roleInfoView;
    /// <summary>
    /// ������������ͼid
    /// </summary>
    public int LastInWorldMapId;
    /// <summary>
    /// ������������ͼ����
    /// </summary>
    public string LastInWorldMapPos;

    /// <summary>
    /// ����ͼ
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.RoleInfo:
                OpenRoleInfoView();
                break;
            //case WindowUIType.Reg:
            //    OpenRegView();
            //    break;
        }
    }

    /// <summary>
    /// �򿪽�ɫ��Ϣ��ͼ
    /// </summary>
    public void OpenRoleInfoView()
    {
        UIViewUtil.Instance.LoadWindow(WindowUIType.RoleInfo.ToString(), (GameObject oj) =>
        {
            roleInfoView = oj.GetComponent<UIRoleInfoView>();
            RoleInfoMainPlayer roleinfo = ((RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo);


            TransferData data = new TransferData();

            data.SetValue(ConstDefine.JobId, roleinfo.JobId);
            data.SetValue(ConstDefine.NickName, roleinfo.RoleNickName);
            data.SetValue(ConstDefine.Level, roleinfo.Level);
            data.SetValue(ConstDefine.Fighting, roleinfo.Fighting);

            data.SetValue(ConstDefine.Money, roleinfo.Money);
            data.SetValue(ConstDefine.Gold, roleinfo.Gold);
            data.SetValue(ConstDefine.CurrHP, roleinfo.CurrHP);
            data.SetValue(ConstDefine.CurrMP, roleinfo.CurrMP);
            data.SetValue(ConstDefine.MaxHP, roleinfo.MaxHP);
            data.SetValue(ConstDefine.MaxMP, roleinfo.MaxMP);

            data.SetValue(ConstDefine.MaxExp, 8888);
            data.SetValue(ConstDefine.CurrExp, roleinfo.Exp);
            data.SetValue(ConstDefine.Attack, roleinfo.Attack);
            data.SetValue(ConstDefine.Defense, roleinfo.Defense);
            data.SetValue(ConstDefine.Dodge, roleinfo.Dodge);
            data.SetValue(ConstDefine.Hit, roleinfo.Hit);

            data.SetValue(ConstDefine.Cri, roleinfo.Cri);
            data.SetValue(ConstDefine.Res, roleinfo.Res);
            roleInfoView.SetRoleInfo(data);
        });

    }


    public void SetMainCityRoleData()
    {
        SetMainCityRoleInfo();
        SetMainCityRoleSkillInfo();
    }
    RoleInfoMainPlayer roleInfoMainPlayer;

    /// <summary>
    /// ��������UI�Ͻ�ɫ��Ϣ
    /// </summary>
    private void SetMainCityRoleInfo()
    {
         roleInfoMainPlayer =(RoleInfoMainPlayer) GlobalInit.Instance.CurrPlayer.CurrRoleInfo;

        string headPic = string.Empty;

        JobEntity entity = JobDBModel.instance.Get(roleInfoMainPlayer.JobId);

        if (entity!=null)
        {
            headPic = entity.HeadPic;

        }

        GlobalInit.Instance.CurrPlayer.OnHPChange= OnHPChangeCallBack;
        GlobalInit.Instance.CurrPlayer.OnMPChange = OnMPChangeCallBack;

        UIMainCityRoleInfoView.Instance.SetUI(headPic, roleInfoMainPlayer.RoleNickName,1, roleInfoMainPlayer.Money, roleInfoMainPlayer.Gold, roleInfoMainPlayer.CurrHP, roleInfoMainPlayer.MaxHP, roleInfoMainPlayer.CurrMP, roleInfoMainPlayer.MaxMP);

        
    }

    /// <summary>
    /// MP�仯�ص�
    /// </summary>
    /// <param name="type"></param>
    private void OnMPChangeCallBack(ValueChangeType type)
    {
        if (roleInfoMainPlayer == null) return;
        UIMainCityRoleInfoView.Instance.SetMP( roleInfoMainPlayer.CurrMP, roleInfoMainPlayer.MaxMP);
    }
    /// <summary>
    /// HP�仯�ص�
    /// </summary>
    /// <param name="type"></param>
    private void OnHPChangeCallBack(ValueChangeType type)
    {
        if (roleInfoMainPlayer == null) return;
        UIMainCityRoleInfoView.Instance.SetHP( roleInfoMainPlayer.CurrHP, roleInfoMainPlayer.MaxHP);
    }

    /// <summary>
    /// ��������UI�Ͻ�ɫ������Ϣ
    /// </summary>
    private void SetMainCityRoleSkillInfo()
    {
        RoleInfoMainPlayer roleInfoMainPlayer = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;

        List<TransferData> lst = new List<TransferData>();

        for (int i = 0; i < roleInfoMainPlayer.SkillInfosLst.Count; i++)
        {
            TransferData data = new TransferData();
            data.SetValue(ConstDefine.SkillId, roleInfoMainPlayer.SkillInfosLst[i].SkillId);
            data.SetValue(ConstDefine.SkillSlotsNo, roleInfoMainPlayer.SkillInfosLst[i].SlotsNo);
            data.SetValue(ConstDefine.SkillLevel, roleInfoMainPlayer.SkillInfosLst[i].SkillLevel);

            SkillEntity entity = SkillDBModel.Instance.Get(roleInfoMainPlayer.SkillInfosLst[i].SkillId);
            if (entity!=null)
            {
                data.SetValue(ConstDefine.SkillPic, entity.SkillPic);
            }

            SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(roleInfoMainPlayer.SkillInfosLst[i].SkillId, roleInfoMainPlayer.SkillInfosLst[i].SkillLevel);
            if (skillLevelEntity!=null)
            {
                data.SetValue(ConstDefine.SkillCDTime, skillLevelEntity.SkillCDTime);
            }

            lst.Add(data);
        }
        UIMainCitySkillView.Instance.SetUI(lst,OnSkillClick);
    }

    /// <summary>
    /// ���ܰ�ť����ص�
    /// </summary>
    /// <param name="obj"></param>
    public void OnSkillClick(int skillId)
    {
        bool isSuccess =GlobalInit.Instance.CurrPlayer.ToAttack(RoleAttackType.SkillAttack, skillId);
        if (isSuccess)
        {
            GlobalInit.Instance.CurrPlayer.CurrRoleInfo.SetSkillCDEndTime(skillId);
            //CD
            UIMainCitySkillView.Instance.BeginCD(skillId);
        }
    }
}
