using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeModeSkill : Skill
{
    Dictionary<Weapon, int> CoolDownDic = new();
    bool isOn = false;
    Coroutine co = null;
    float coolDownVal = 20f;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        if (CoolDownDic.ContainsKey(weapon))
        {
            if (power != CoolDownDic[weapon])
            {
                weapon.Data.CoolDown -= coolDownVal * CoolDownDic[weapon];
                CoolDownDic[weapon] = power;
                weapon.Data.CoolDown += coolDownVal * CoolDownDic[weapon];
            }
        }
        else
        {
            weapon.Data.CoolDown += coolDownVal * power;
            CoolDownDic.Add(weapon, power);
        }

        isOn = true;
        if (co == null)
            co = StartCoroutine(ISiegeModeOn());
    }

    IEnumerator ISiegeModeOn()
    {
        while (isOn)
        {
            isOn = false;
            yield return null;
        }
        co = null;
        foreach (var v in CoolDownDic)
        {
            v.Key.Data.CoolDown -= coolDownVal * v.Value;
        }
        CoolDownDic.Clear();
    }


}
