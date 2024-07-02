using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticEMP : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (weaponTrm.GetComponent<Weapon>() != trigger.startWeapon)
        {
            return;
        }

        StartCoroutine(Generate(target, power, weaponTrm.GetComponent<Weapon>()));

    }

    private IEnumerator Generate(Transform target, int power, Weapon weapon)
    {

        yield return new WaitForSeconds(0.5f);

        if(target != null)
        {
            var obj = Instantiate(prefab, target.position, Quaternion.identity);
            obj.SetDamage(power * weapon.Data.GetDamage() / 10);

        }

    }

}
