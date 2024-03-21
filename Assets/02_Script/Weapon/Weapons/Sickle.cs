using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sickle : InvenWeapon
{
    public float range;
    [SerializeField] LayerMask hitMask;
    List<Collider2D> monsters = new List<Collider2D>();

    [SerializeField] private GameObject effect;

    public override void GetSignal(object signal)
    {
        var data = (SendData)signal;

        if (sendDatas == null)
        {
            sendDatas = data;
        }
        else
        {
            sendDatas = sendDatas.Power > data.Power ? sendDatas : data;
        }

    }

    public override void Attack(Transform target)
    {

        monsters.Clear();
        
        Instantiate(effect, transform.position, transform.rotation);

        monsters = Physics2D.OverlapCircleAll(transform.position, range, hitMask).ToList();

        foreach (var item in monsters)
        {

            if (item.TryGetComponent<IHitAble>(out var h))
            {

                h.Hit(Data.AttackDamage.GetValue());

            }

        }

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, range);

    }

}
