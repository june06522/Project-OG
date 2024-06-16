using System.Collections;
using UnityEngine;

public class SequenceAttackSkill : Skill
{
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        StartCoroutine(IRun(weapon, target, power));
    }

    IEnumerator IRun(Weapon weapon, Transform target, int power)
    {
        for (int i = 0; i < power; i++)
        {
            if (weapon != null)
            {
                weapon.Run(target, true);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }
}
