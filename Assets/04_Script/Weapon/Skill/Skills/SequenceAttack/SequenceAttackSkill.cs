using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAttackSkill : Skill
{
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        StartCoroutine(IRun(weapon, target, power));
    }

    IEnumerator IRun(Weapon weapon, Transform target, int power)
    {
        for (int i = 0; i < power; i++)
        {
            weapon.Data.isSkillAttack = true;
            weapon.Run(target);
            weapon.Data.isSkillAttack = false;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
