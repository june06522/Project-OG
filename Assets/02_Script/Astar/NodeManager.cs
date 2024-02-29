using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Astar;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class NodeManager : MonoBehaviour
{
    //Debuging

    [SerializeField]
    private Tilemap _mainMap;

    public static NodeManager Instance;
    public List<Node> WallNodes = new ();
    public List<Node> ObstacleNodes = new();
    public List<Node> GroundNodes = new();
    public List<Node> AllNodes = new();
    public int AllNodeCount => AllNodes.Count;
    public bool IsBaking { get; private set; }

    public event Action BakeStartEvent;
    public event Action BakeEndEvent;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        BakeMap();
    }

    //맵에 노드 할당
    [ContextMenu("BakeMap")]
    public void BakeMap()
    {
        //Bake(TilemapManager.Instance.MainMap.cellBounds);
        Bake(_mainMap.cellBounds);
    }

    [ContextMenu("Clear")]
    public void ClearNodes()
    {
        WallNodes.Clear();
        GroundNodes.Clear();
        ObstacleNodes.Clear();
        AllNodes.Clear();  
    }

    private void OnDisable()
    {
        ClearNodes();
    }

    public async void Bake(BoundsInt roomBounds)
    {
        IsBaking = true;

        WallNodes.Clear();
        GroundNodes.Clear();
        ObstacleNodes.Clear();
        AllNodes.Clear();

        BakeStartEvent?.Invoke();
        AllNodes = await MakeNode(roomBounds);
        BakeEndEvent?.Invoke();
        
        IsBaking = false;
        
        //Debug.Log("Bake");
    }

    public Task<List<Node>> MakeNode(BoundsInt bounds)
    {
        int capacity = (bounds.xMax - bounds.xMin) * (bounds.yMax - bounds.yMin);
        Debug.Log($"Capacity : {capacity}");
        List<Node> nodes = new List<Node>(capacity);

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Node node = new Node();
            node.Pos = pos;
            node.Type = NodeType.MoveAble;

            Vector2 worldPos = GetWorldPos(pos);
            Vector2 size = new Vector2(0.1f, 0.1f);

            int wallLayer = LayerMask.NameToLayer("Wall");
            int obstacleLayer = LayerMask.NameToLayer("Obstacle");

            Collider2D collider = Physics2D.OverlapBox(worldPos, size, 0f, (1 << wallLayer) | (1 << obstacleLayer));
            //Debug.Log(wallLayer);
            //Debug.Log(obstacleLayer);

            if (collider != null)
            {
                //Debug.Log(collider.gameObject.layer);
                if (collider.gameObject.layer == wallLayer)
                {
                    node.Type = NodeType.Locomobile;
                    node.Weight = 9999;

                    WallNodes.Add(node);
                }
                else if (collider.gameObject.layer == obstacleLayer)
                {
                    Obstacle obstacle;
                    if (collider.TryGetComponent<Obstacle>(out obstacle))
                    {
                        //node.Weight = obstacle.Weight;
                        node.Weight = 9999;
                    }

                    ObstacleNodes.Add(node);
                }
            }
            else
            {
                node.Weight = 0;
                GroundNodes.Add(node);
            }
            nodes.Add(node);
        }
        //Debug.Log("End");
        return Task.FromResult(nodes);
    }

    private void OnDrawGizmos()
    {
        if(WallNodes.Count != 0)
        {
            Gizmos.color = new Color(0, 0, 0, 0.8f);
            WallNodes.ForEach(node => Gizmos.DrawCube(
                GetWorldPos(node.Pos), Vector3.one));
        }

        if(GroundNodes.Count != 0)
        {
            Gizmos.color = new Color(255, 255, 255, 0.3f);
            GroundNodes.ForEach(node => Gizmos.DrawCube(
               GetWorldPos(node.Pos), Vector3.one));
        }

        if(ObstacleNodes.Count != 0)
        {
            Gizmos.color = new Color(255, 0, 255, 0.5f);
            ObstacleNodes.ForEach(node => Gizmos.DrawCube(
               GetWorldPos(node.Pos), Vector3.one));
        }
    }

    public List<Node> GetRoomNode(BoundsInt bound)
    {
        List<Node> roomNodes = (from node in AllNodes
                               where bound.Contains(node.Pos)
                               select node).ToList();

        return roomNodes;
    }

    public Vector3 GetWorldPos(Vector3Int cellPos)
    {
        return _mainMap.GetCellCenterWorld(cellPos);
    }
}