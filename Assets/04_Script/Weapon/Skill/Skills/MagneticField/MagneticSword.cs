using UnityEngine;

public class MagneticSword : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        if (weaponTrm.GetComponent<Weapon>() != trigger.startWeapon)
            return;

        var obj = Instantiate(prefab, target.position, Quaternion.identity);
        obj.SetDamage(power * weaponTrm.GetComponent<Weapon>().Data.GetDamage() / 10);
    }

}
