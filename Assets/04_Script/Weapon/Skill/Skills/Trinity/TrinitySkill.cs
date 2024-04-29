using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinitySkill : Skill
{
    TrinityObject obj;

    Dictionary<Weapon, int> damageDic = new();
    bool isOn = false;
    Coroutine co = null;
    float _defaultVal = 40f;
    float _plusVal = 10f;

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();


        if (damageDic.ContainsKey(weapon))
        {
            if (power != damageDic[weapon])
            {
                weapon.Data.AddDamege -= GetDamage(weapon, damageDic[weapon]);
                damageDic[weapon] = power;
                weapon.Data.AddDamege += GetDamage(weapon, damageDic[weapon]);
            }
        }
        else
        {
            weapon.Data.AddDamege += GetDamage(weapon,power);
            damageDic.Add(weapon, power);
        }

        isOn = true;
        if (co == null)
            co = StartCoroutine(ISkillOn());
    }

    IEnumerator ISkillOn()
    {
        while (isOn)
        {
            isOn = false;
            yield return null;
        }
        co = null;
        foreach (var v in damageDic)
        {
            v.Key.Data.AddDamege -= GetDamage(v.Key,v.Value);
        }
        damageDic.Clear();
    }

    private float GetDamage(Weapon weapon,int power)
    {
        float originDamage = weapon.Data.AttackDamage.GetValue();
        
        return originDamage * ((_defaultVal + _plusVal * power) / 100);
    }
}
