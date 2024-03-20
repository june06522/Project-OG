using UnityEngine;

public class EarthquakeHammer : Skill
{

    [SerializeField] GameObject hammerEffect;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var temp = Instantiate(hammerEffect, weaponTrm.position, weaponTrm.rotation);

        temp.GetComponent<CrackCollision>().SetDamage(power * 10);
        Destroy(temp, 1.6f);

    }

}
