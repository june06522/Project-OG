using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sickle : InvenWeapon
{
    public float range;

    List<Collider2D> monsters = new List<Collider2D>();

    public override void GetSignal(object signal)
    {

        var data = (SendData)signal;
        SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

    }

    public override void Attack(Transform target)
    {
        monsters.Clear();
        Debug.Log(monsters.Capacity);
        monsters = Physics2D.OverlapCircleAll(transform.position, range, 1 << 3).ToList();

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
