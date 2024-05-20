using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : Skill
{
    [SerializeField] LaserSkillObj skillObj;
    
    Dictionary<Tuple<Transform,Transform>, LaserSkillObj> laserSkillDic = new();

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (target == null || weaponTrm == null || trigger == null || trigger.trigger == null)
            return;

        var tuple = Tuple.Create(weaponTrm, trigger.trigger);

        if (!laserSkillDic.ContainsKey(tuple))
        {
            laserSkillDic.Add(tuple, Instantiate(skillObj));
        }

        laserSkillDic[tuple].Execute(weaponTrm,target, power);
    }

    
}
