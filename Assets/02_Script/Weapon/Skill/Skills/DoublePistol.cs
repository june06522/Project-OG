using System.Collections;
using UnityEngine;

public class DoublePistol : Skill
{

    [SerializeField] private Bullet bullet;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        StartCoroutine(Shooting(weaponTrm, target, power));

    }

    IEnumerator Shooting(Transform weaponTrm, Transform target, int power)
    {
        var a = Instantiate(bullet, weaponTrm.position, weaponTrm.rotation);
        Debug.Log(power);
        a.Shoot(power * 10);

        yield return new WaitForSeconds(0.1f);

        var b = Instantiate(bullet, weaponTrm.position, weaponTrm.rotation);
        Debug.Log(power);
        b.Shoot(power * 10);
    }

}
