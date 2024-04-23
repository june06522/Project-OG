using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpeaker : Skill
{

    [SerializeField] SpeakerAttack effect;
    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var obj = Instantiate(effect, transform.position, Quaternion.identity);
        obj.transform.localScale = Vector3.one * 1.5f;
        obj.GetComponent<SpriteRenderer>().color = Color.blue;
        obj.SetDamage(power * 3);

    }

}
