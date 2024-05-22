using Astar;
using DG.Tweening;
using DSNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BaseFSM_Controller<T> : FSM_System.FSM_Controller<T> where T : Enum
{
    [field: SerializeField] public EnemyDataSO EnemyDataSO { get; protected set; }
    //public Navigation Nav;
    public ContextSolver Solver;

    public event Action FixedUpdateAction;
    public Transform Target { get; set; }
    public Enemy Enemy { get; private set; }
    public AIData AIdata { get; private set; }

    private LinkedList<Vector2> m_fasterPath = new();

    protected override void Awake()
    {
        Enemy = GetComponent<Enemy>();
        AIdata = GetComponent<AIData>();
        //spriteRender = transform.Find("Body").GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        EnemyDataSO = Instantiate(EnemyDataSO);
        //Nav = new(Enemy);
        Solver = new();
        //    Target = GameManager.Instance.player;
        Target = GameObject.Find("Player").transform;
    }

    protected override void Update()
    {
        if (Enemy.Dead) return;
        //Debug.Log(currentState);
        base.Update();
    }

    protected void FixedUpdate()
    {
        FixedUpdateAction?.Invoke();
    }

    public void StopImmediately()
    {
        if(Enemy.enemyAnimController.IsMove)
            Enemy.enemyAnimController.SetMove(false);
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

    public void FindPath()
    {
        if (Enemy.GetPathFinder == null)
            return;
        if(Enemy.OwnerStage == null) 
            return;    
        
        Vector2 m_start = transform.position;
        Vector2 m_goal = Target.position;

        m_fasterPath.Clear();
        {
            if(Enemy.OwnerStage.GridInfo != null)
            {
                bool isPathFound = Enemy.GetPathFinder.FindPath
                    (Enemy.OwnerStage.GridInfo,
                    m_start,
                    m_goal,
                    ref m_fasterPath); // ** find path from start to goal **
            }
        }
    }

    public bool GetNextPath(ref Vector2 nextPos)
    {
        if (m_fasterPath != null && m_fasterPath.Count > 0)
        {
            nextPos = m_fasterPath.First.Value;
            if (Vector3.Distance(transform.position, m_fasterPath.First.Value) < 0.1f)
            {
                m_fasterPath.RemoveFirst();
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool IsBetweenObstacle()
    {
        if (AIdata.currentTarget != null)
        {
            Vector2 dir = AIdata.currentTarget.transform.position - transform.position;
            if (Physics2D.CircleCast(transform.position, 
                0.5f,
                dir.normalized, 
                dir.magnitude, 
                EnemyDataSO.ObstacleLayer))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //if (EditorApplication.isPlaying)
        //{
        //    Color originalColor = Gizmos.color;

        //    if (m_fasterPath.Count > 0)
        //    {
        //        Gizmos.color = Color.green;

        //        foreach (var loc in m_fasterPath)
        //            Gizmos.DrawCube(new Vector3(loc.x, loc.y, 0), new Vector3(0.5f, 0.5f, 0.5f));

        //        Gizmos.DrawLine(transform.position, m_fasterPath.First.Value);

        //        for (LinkedListNode<Vector2> iter = m_fasterPath.First; iter.Next != null; iter = iter.Next)
        //        {
        //            Vector3 from = iter.Value;
        //            Vector3 to = iter.Next.Value;

        //            Gizmos.DrawLine(from, to);
        //        }
        //    }

        //    Gizmos.color = originalColor;
        //}
    }

    public bool IsPath()
    {
        return m_fasterPath.Count > 0;
    }

    public void ResetPath()
    {
        m_fasterPath.Clear();
    }
    //public void PrintRoute(List<Vector3> route)
    //{
    //    if (route == null) return;
    //    if (route.Count < 2) return;
    //    lineRenderer.positionCount = route.Count;

    //    lineRenderer.SetPositions(route.ToArray());
    //}
}
