using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YondoSkill : Skill
{
    [SerializeField] SwordYondo yondo;
    [Header("ETC")]
    [SerializeField] LayerMask layerMask;
    [SerializeField]
    float coolTime = 5f;
    [SerializeField]
    float radius = 50f;
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        if(Physics2D.OverlapCircle(weaponTrm.parent.position, radius, layerMask))
        {
            SwordYondo obj = Instantiate(yondo, weaponTrm.parent.position, weaponTrm.rotation);
            obj.Init(layerMask, power, radius, coolTime);
        }
    }

}
