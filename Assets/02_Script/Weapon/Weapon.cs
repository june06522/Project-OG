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

        Attack(target);

    }

    public abstract void Attack(Transform target);
    public abstract void Skill();

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

        OnWeaponCharged?.Invoke(chargeGauge, chargeValue);

    }

}
