using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    private float chargeValue;

    [field:SerializeField] public WeaponDataSO Data { get; protected set; }
    public Guid WeaponGuid { get; protected set; }

    protected virtual void Awake()
    {

        WeaponGuid = Guid.NewGuid();

        Data = Instantiate(Data);
        Data.Init(this);

    }

    public virtual void Run(Transform target)
    {

        if (!Data.isAttackCoolDown && target != null)
        {

            Data.SetCoolDown();
            Attack(target);

        }

    }

    protected abstract void Attack(Transform target);
    protected abstract void Skill(int count);

    public virtual void Charge(float value)
    {

        chargeValue += value;

        if(chargeValue >= Data.ChangeGauge.GetValue())
        {

            int cnt = Mathf.FloorToInt(chargeValue / Data.ChangeGauge.GetValue());
            Skill(cnt);

            chargeValue = 0f;

        }

    }

    public virtual void OnRePosition() { }

}
