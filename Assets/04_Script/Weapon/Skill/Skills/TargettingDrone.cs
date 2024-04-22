using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingDrone : Skill
{

    [SerializeField] FollowDrone dronePrefab;


    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        var obj = Instantiate(dronePrefab, target.position + (Vector3)Random.insideUnitCircle * 1.2f, Quaternion.identity);
        
        obj.SetLifeTime(power, target);

    }

}
