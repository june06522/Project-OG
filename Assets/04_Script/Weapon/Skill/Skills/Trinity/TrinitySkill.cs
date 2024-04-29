using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinitySkill : Skill
{
    [SerializeField]TrinityObject trinityObj;

    Dictionary<Tuple<Transform,SendData>, TrinityObject> damageDic = new();
    

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        var tuple = Tuple.Create(weaponTrm, trigger);
        if(!damageDic.ContainsKey(tuple))
        {
            damageDic.Add(tuple, Instantiate(trinityObj));
        }
        damageDic[tuple].Execute(weapon, target, power);
        
    }

    
}
