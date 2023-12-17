using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChargedWeapon(float curCharge, float maxCharge);

public abstract class Weapon : MonoBehaviour
{

    [SerializeField] private float chargeGauge;

    public event ChargedWeapon OnWeaponCharged;
    public event Action OnAttackEvent;

    private float chargeValue;

    public void DoAttack(Transform target)
    {



    }

    public abstract void Attack(Transform target);
    public abstract void Skill();

    public virtual void Charge(float value)
    {

        chargeValue += value;

        if(chargeValue >= chargeGauge)
        {

            chargeValue = 0f;
            Skill();

        }

        OnWeaponCharged?.Invoke(chargeGauge, chargeValue);

    }

}
