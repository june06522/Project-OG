using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpeaker : Skill
{

    [SerializeField] SpeakerAttack effect;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        var obj = Instantiate(effect, weaponTrm.position, Quaternion.identity);
        obj.transform.localScale = Vector3.one * 1.5f;
        obj.GetComponent<SpriteRenderer>().color = Color.blue;
        obj.SetDamage(power * 3);

    }

}
