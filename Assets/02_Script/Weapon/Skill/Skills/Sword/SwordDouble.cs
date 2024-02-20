using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;

public class SwordDouble : Skill
{
    [SerializeField] Effect redBullet;
    [SerializeField] Effect blueBullet;

    [SerializeField] GameObject sword;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        StartCoroutine(SkillCo(weaponTrm, target, power));

    }

    IEnumerator SkillCo(Transform weaponTrm, Transform target, int power)
    {

        var a = Instantiate(sword, weaponTrm.position, weaponTrm.rotation);
        var ren1 = a.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ren1.color = Color.red;
        DOTween.Sequence().
            Append(a.transform.DORotate(new Vector3(0, 0, a.transform.rotation.eulerAngles.z - 60), 0)).
            Append(a.transform.DORotate(new Vector3(0, 0, a.transform.rotation.eulerAngles.z + 60), 0.1f).SetEase(Ease.Linear));
        Destroy(a, 0.1f);

        yield return new WaitForSeconds(0.1f);

        var r = Instantiate(redBullet, weaponTrm.position + weaponTrm.right * 2, weaponTrm.rotation);
        r.Shoot(r.Data.Damage * power, 0.5f);
        Destroy(r, 0.3f);

        var aa = Instantiate(sword, weaponTrm.position, weaponTrm.rotation);
        var ren2 = aa.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ren2.color = Color.blue;
        DOTween.Sequence().
            Append(aa.transform.DORotate(new Vector3(0, 0, aa.transform.rotation.eulerAngles.z + 60), 0)).
            Append(aa.transform.DORotate(new Vector3(0, 0, aa.transform.rotation.eulerAngles.z - 60), 0.1f).SetEase(Ease.Linear));
        Destroy(aa, 0.1f);

        yield return new WaitForSeconds(0.1f);

        var b = Instantiate(blueBullet, weaponTrm.position + weaponTrm.right * 2, weaponTrm.rotation);
        b.Shoot(b.Data.Damage * power, 0.5f);
        Destroy(b, 0.3f);

    }

}
