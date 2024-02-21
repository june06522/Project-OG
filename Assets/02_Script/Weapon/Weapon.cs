using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponID id;
    public WeaponType weaponType;

    protected Transform target;
    private float chargeValue;

    [field: SerializeField] public WeaponDataSO Data { get; protected set; }
    public Guid WeaponGuid { get; protected set; }
    protected List<SkillFeedback> skillFeedbackContainter;

    protected virtual void Awake()
    {

        WeaponGuid = Guid.NewGuid();

        Data = Instantiate(Data);
        Data.Init(this);

    }

    public virtual void Run(Transform target)
    {

        this.target = target;

        RotateWeapon(target);

        if (!Data.isAttackCoolDown && target != null)
        {

            Data.SetCoolDown();
            Attack(target);

        }

    }

    protected virtual void RotateWeapon(Transform target)
    {
        if (target == null) return;

        var dir = target.position - transform.position;

        transform.up = dir.normalized;

    }

    public abstract void Attack(Transform target);

    public virtual void OnRePosition() { }

}
