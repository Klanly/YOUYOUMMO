using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFight : MonoBehaviour {

    public RoleCtrl roleCtrl;
	void Start () {
		
	}
	

	void Update () {

    }

    private void OnGUI()
    {
        int posY = 0;

        if (GUI.Button(new Rect(1,posY,80,30),"普通待机"))
        {
            roleCtrl.ToIdle();
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "战斗待机"))
        {
            roleCtrl.ToIdle(RoleIdleState.IdleFight);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "跑"))
        {
            roleCtrl.ToRun();
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "受伤"))
        {
           // roleCtrl.ToHurt(0,0);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "死亡"))
        {
            roleCtrl.ToDie();
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "胜利"))
        {
            roleCtrl.ToSelect();
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "物理攻击1"))
        {
            roleCtrl.ToAttack(RoleAttackType.PhyAttack,1);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "物理攻击2"))
        {
      
            roleCtrl.ToAttack(RoleAttackType.PhyAttack, 2);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "物理攻击3"))
        {
  
            roleCtrl.ToAttack(RoleAttackType.PhyAttack, 3);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击1"))
        {
    
            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 1);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击2"))
        {
     
            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 2);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击3"))
        {
 
            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 3);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击4"))
        {
    
            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 4);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击5"))
        {
  
            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 5);
        }
        posY += 30;
        if (GUI.Button(new Rect(1, posY, 80, 30), "技能攻击6"))
        {

            roleCtrl.ToAttack(RoleAttackType.SkillAttack, 6);
        }


    }

}
