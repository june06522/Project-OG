using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YondoSkill : Skill
{
    [SerializeField] GameObject yondo;

    [SerializeField]
    float coolTime = 5f;
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var obj = Instantiate(yondo, weaponTrm.parent.position, weaponTrm.rotation);
        Destroy(obj, coolTime);
    }

}
