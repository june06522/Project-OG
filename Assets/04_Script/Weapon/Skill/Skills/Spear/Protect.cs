using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : Skill
{
    [SerializeField] private ParticleSelfDestroyer protection;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        var player = GameManager.Instance.player;

        var p = Instantiate(protection, player.position, Quaternion.identity);
        p.transform.SetParent(player);
        p.Attack(power * 5);

    }

}
