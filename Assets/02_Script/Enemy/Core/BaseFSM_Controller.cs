using System;
using UnityEngine;

public class BaseFSM_Controller<T> : FSM_System.FSM_Controller<T> where T : Enum
{
    [field: SerializeField] public EnemyDataSO EnemyData { get; protected set; }
    Enemy enemy;

    protected override void Awake()
    {
        enemy = GetComponent<Enemy>();
        EnemyData = Instantiate(EnemyData);
    }

    protected override void Update()
    {
        if (enemy.Dead) return;
        base.Update();
    }
}
