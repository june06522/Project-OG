using UnityEngine;
using UnityEngine.VFX;

public class OrbHammer : Skill
{

    [SerializeField] GameObject hammerEffect;
    [SerializeField] AudioClip clip;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;

        var temp = Instantiate(hammerEffect, weaponTrm.position, Quaternion.Euler(0, 0, angle));
        transform.localScale = Vector3.one + power * Vector3.one * 0.15f;
        SoundManager.Instance?.SFXPlay("Orb", clip);

        temp.GetComponent<OrbCollision>().SetDamage(power * 10);
        temp.GetComponent<VisualEffect>().SetFloat("Duration", 5 + power * 0.5f);
        Destroy(temp, 10f);

    }

}
