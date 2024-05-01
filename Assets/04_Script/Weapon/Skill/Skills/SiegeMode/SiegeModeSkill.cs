using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SiegeModeSkill : Skill
{
    [SerializeField] SiegeModeObj siegeModeobj;
    [SerializeField] VisualEffect effect;
    VisualEffect v;

    Dictionary<Tuple<Transform, Transform>, SiegeModeObj> _coolDownDic = new();
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if(v != null)
        {
            v = Instantiate(effect,GameManager.Instance.player.position,Quaternion.identity);
        }

        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        var tuple = Tuple.Create(weaponTrm, trigger.trigger);
        if(!_coolDownDic.ContainsKey(tuple))
        {
            _coolDownDic.Add(tuple, Instantiate(siegeModeobj));
        }
        _coolDownDic[tuple].Excute(weapon,power);
    }

    


}
