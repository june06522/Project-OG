using System.Collections;
using UnityEngine;

public class Explosion : Skill
{
    [SerializeField] private ParticleSelfDestroyer explosion;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var s = weaponTrm.GetComponent<Spear>();
        s.AttackImmediately();

        if (target == null) return;

        StartCoroutine(ExplosionCo(s, weaponTrm, target, power));
    }


    IEnumerator ExplosionCo(Spear s, Transform weaponTrm, Transform target, int power)
    {
        yield return new WaitForSeconds(s.StingBackTime);
        Instantiate(explosion, target.position, weaponTrm.rotation);
    }
}
