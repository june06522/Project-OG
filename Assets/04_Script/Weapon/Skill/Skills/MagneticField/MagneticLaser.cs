using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLaser : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (weaponTrm.GetComponent<Weapon>() != trigger.startWeapon)
            return;

        RaycastHit2D hit = Physics2D.Raycast(weaponTrm.position, weaponTrm.right, int.MaxValue, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {

            var obj = Instantiate(prefab, hit.point, Quaternion.identity);
            obj.SetDamage(power * weaponTrm.GetComponent<Weapon>().Data.GetDamage() / 10);

        }
    }

}
