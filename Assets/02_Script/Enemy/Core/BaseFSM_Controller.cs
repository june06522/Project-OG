using Astar;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseFSM_Controller<T> : FSM_System.FSM_Controller<T> where T : Enum
{
    [field: SerializeField] public EnemyDataSO EnemyData { get; protected set; }
    protected Enemy enemy;
    public Navigation Nav;

    protected override void Awake()
    {
        enemy = GetComponent<Enemy>();
        EnemyData = Instantiate(EnemyData);
        Nav = new(enemy);

        spriteRender = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        
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

    private LineRenderer lineRenderer;
    public void PrintRoute(List<Vector3Int> route)
    {
        if (route.Count < 2 || route == null) return;
        lineRenderer.positionCount = route.Count;

        lineRenderer.SetPositions(route.Select(p => TilemapManager.Instance.GetWorldPos(p)).ToArray());
    }
}
