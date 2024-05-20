using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticDrone : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (trigger == null || weaponTrm.GetComponent<Weapon>() != trigger.startWeapon)
            return;

        var obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);
        obj.transform.localScale = Vector3.one * 0.8f;
        obj.transform.SetParent(weaponTrm, true);
        obj.SetDamage(power * weaponTrm.GetComponent<Weapon>().Data.GetDamage() / 10);

    }

}
