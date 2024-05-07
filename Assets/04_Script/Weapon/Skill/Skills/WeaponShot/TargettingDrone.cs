using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingDrone : Skill
{

    [SerializeField] FollowDrone dronePrefab;


    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        if (target == null) return;

        var obj = Instantiate(dronePrefab, target.position + (Vector3)Random.insideUnitCircle * 3f, Quaternion.identity);

        obj.SetLifeTime(power * 2f, target);

    }

}
