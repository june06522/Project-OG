using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float AttackPower { get; set; }
    [field: SerializeField] public float AttackAbleRange { get; set; }
    [field: SerializeField] public float Range { get; set; }
    [field: SerializeField] public float AttackCoolDown { get; set; }
    [field: SerializeField] public LayerMask TargetAbleLayer { get; private set; }

    public bool IsAttackCoolDown { get; private set; }

    public void SetCoolDown()
    {
        if (IsAttackCoolDown) return;
        IsAttackCoolDown = true;

        FAED.InvokeDelay(() =>
        {

            IsAttackCoolDown = false;

        }, AttackCoolDown);

    }
}
