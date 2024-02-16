using DG.Tweening;
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
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemy.TargetTrm = GameManager.Instance.player;   
    }

    protected override void Update()
    {
        if (enemy.Dead) return;
        base.Update();
    }

    public void Flip(bool left)
    {
        transform.rotation = left ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    //Debug
    private SpriteRenderer spriteRender;
    public void ChangeColor(Color color)
    {
        spriteRender.DOColor(color, 0.25f);
    }
}
