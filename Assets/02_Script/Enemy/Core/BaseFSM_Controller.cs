using Astar;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseFSM_Controller<T> : FSM_System.FSM_Controller<T> where T : Enum
{
    [field: SerializeField] public EnemyDataSO EnemyDataSO { get; protected set; }
    [SerializeField] public EnemyFindEffect enemyFindEffect;
    public Enemy Enemy { get; private set; }
    public Navigation Nav;
    public ContextSolver Solver;
    public AIData AIdata;

    public event Action FixedUpdateAction;
    public Transform Target;

    protected override void Awake()
    {
        Enemy = GetComponent<Enemy>();
        AIdata = GetComponent<AIData>();
        spriteRender = transform.Find("Visual").GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        EnemyDataSO = Instantiate(EnemyDataSO);
        Nav = new(Enemy);
        Solver = new();
        Target = GameManager.Instance.player;
    }

    protected override void Update()
    {
        if (Enemy.Dead) return;
        base.Update();
    }

    protected void FixedUpdate()
    {
        FixedUpdateAction?.Invoke();
    }

    public void StopImmediately()
    {
        Enemy.MovementInput = Vector2.zero;
    }


    public void Flip(bool left)
    {
        transform.rotation = left ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    //Debug
    private SpriteRenderer spriteRender;
    public void ChangeColor(Color color, bool useTween = true)
    {
        if (useTween)
            spriteRender.DOColor(color, 0.25f);
        else
            spriteRender.color = color;

    }

    public void PlayDiscoverAnim()
    {
        Vector2 spawnPoint = new Vector2(-0.8f, 1.2f);
        Instantiate(enemyFindEffect, transform).transform.localPosition = spawnPoint;
    }

    //public void PrintRoute(List<Vector3> route)
    //{
    //    if (route == null) return;
    //    if (route.Count < 2) return;
    //    lineRenderer.positionCount = route.Count;

    //    lineRenderer.SetPositions(route.ToArray());
    //}
}
