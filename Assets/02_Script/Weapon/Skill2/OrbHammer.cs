using UnityEngine;

public class OrbHammer : Skill
{

    [SerializeField] GameObject hammerEffect;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;

        var temp = Instantiate(hammerEffect, weaponTrm.position, Quaternion.Euler(0, 0, angle));

        temp.GetComponent<OrbCollision>().SetDamage(power * 10);
        Destroy(temp, 10f);

    }

}