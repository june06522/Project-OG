using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinityObject : MonoBehaviour
{
    bool isOn = false;
    Coroutine co = null;
    int lastPower = 0;
    float _defaultVal = 40f;
    float _plusVal = 10f;

    public void Execute(Weapon weapon, Transform target, int power)
    {
        if (power != lastPower && co != null)
        {
            weapon.Data.AddDamege -= GetDamage(weapon, lastPower);
            lastPower = power;
            weapon.Data.AddDamege += GetDamage(weapon, lastPower);
        }

        isOn = true;
        if (co == null)
            co = StartCoroutine(ISkillOn(weapon));
    }

    IEnumerator ISkillOn(Weapon weapon)
    {

        weapon.Data.AddDamege += GetDamage(weapon, lastPower);

        while (isOn)
        {
            isOn = false;
            yield return null;
        }
        co = null;

        weapon.Data.AddDamege -= GetDamage(weapon, lastPower);

    }

    private float GetDamage(Weapon weapon, int power)
    {
        if (power == 0)
            return 0;

        float originDamage = weapon.Data.AttackDamage.GetValue();

        return originDamage * ((_defaultVal + _plusVal * power) / 100);
    }
}
