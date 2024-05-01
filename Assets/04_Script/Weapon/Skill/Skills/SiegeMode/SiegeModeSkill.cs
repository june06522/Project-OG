using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeModeSkill : Skill
{
    [SerializeField] SiegeModeObj siegeModeobj;

    Dictionary<Tuple<Transform, SendData>, SiegeModeObj> _coolDownDic = new();
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        var tuple = Tuple.Create(weaponTrm, trigger);
        if(!_coolDownDic.ContainsKey(tuple))
        {
            _coolDownDic.Add(tuple, Instantiate(siegeModeobj));
        }
        _coolDownDic[tuple].Excute(weapon,power);
    }

    


}
