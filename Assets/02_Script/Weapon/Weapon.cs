using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [SerializeField] private float chargeGauge;

    private float chargeValue;

    public Guid WeaponGuid { get; protected set; }

    private void Awake()
    {

        WeaponGuid = Guid.NewGuid();

    }

    public void DoAttack(Transform target)
    {

        Attack(target);

    }

    protected abstract void Attack(Transform target);
    protected abstract void Skill();

    public virtual void Charge(float value)
    {

        chargeValue += value;

        if(chargeValue >= chargeGauge)
        {

            int cnt = Mathf.FloorToInt(chargeValue / chargeGauge);

            for(int i = 0; i < cnt; i++)
            {

                Skill();

            }

            chargeValue = 0f;

        }

    }

}
