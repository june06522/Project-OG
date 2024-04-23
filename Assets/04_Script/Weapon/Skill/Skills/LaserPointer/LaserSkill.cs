using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : Skill
{
    [SerializeField] LaserSkillObj skillObj;
    
    Dictionary<Transform, LaserSkillObj> laserSkillDic = new();

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        if (target == null)
            return;

        if(!laserSkillDic.ContainsKey(weaponTrm) )
        {
            laserSkillDic.Add(weaponTrm, Instantiate(skillObj));
        }

        laserSkillDic[weaponTrm].Execute(weaponTrm,target, power);
    }

    
}
