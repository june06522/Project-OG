using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Astar
{
    public class Navigation
    {
        List<Node> roomNodes; // 맵에 미리 구워놓고 그걸로 판단하려면 얘 살려야함. 나중에 최적화용.
        List<Node> closeNodes;
        Heap openNodes;

        BoundsInt roomBounds;

        Collider2D conCol;
        Transform conTrm;

        LayerMask obstacleLayer;

        Vector3Int currentPos;
        Vector3Int targetPos;

        public bool IsNavActive;

        public Navigation(Enemy enemy)
        {
            this.roomBounds = enemy.RoomInfo.bound;

            this.conCol = enemy.Collider;
            this.conTrm = enemy.transform;

            this.obstacleLayer = enemy.EnemyDataSO.ObstacleLayer;

           
            int capacity = enemy.RoomInfo.bound.size.x * enemy.RoomInfo.bound.size.y;
            openNodes = new Heap(capacity);
            closeNodes = new List<Node>(capacity);
            roomNodes = new List<Node>(capacity);


            NodeManager.Instance.BakeStartEvent += BakeStartEvent;
            NodeManager.Instance.BakeEndEvent += BakeEndEvent;
        }

        ~Navigation()
        {
            NodeManager.Instance.BakeEndEvent -= BakeEndEvent;
        }

        public void BakeStartEvent()
        {
            IsNavActive = false;
        }

        public void BakeEndEvent()
        {
            roomNodes.Clear();
            roomNodes = NodeManager.Instance.GetRoomNode(roomBounds);
            IsNavActive = true;
        }

        public Vector3 GetRandomPos(Vector3 curPos, float distance)
        {
            Vector3Int pos = TilemapManager.Instance.GetTilePos(curPos);
            List<Node> moveAbleNodes = (from node in roomNodes
                                        where node.Weight == 0 && (Vector3Int.Distance(node.Pos, pos) < distance)// 장애물, 벽 x
                                        select node).ToList();

            if (moveAbleNodes.Count == 0 || moveAbleNodes == null)
                return curPos;
            
            Vector3Int randomTilePos = moveAbleNodes[Random.Range(0, moveAbleNodes.Count)].Pos;
            Vector3 randomPos = TilemapManager.Instance.GetWorldPos(randomTilePos);
            return randomPos;
        }

        public List<Vector3> GetRoute(Vector3 target)
        {
            if (IsNavActive == false) return null;
            
            openNodes.Clear();
            closeNodes.Clear();
            roomNodes.ForEach((node) => node.Reset());

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(target);

            Node firstNode =
                roomNodes.Find((node) => node.Pos == currentPos);

            if(firstNode == null)
            {
                firstNode = new Node
                {
                    Pos = currentPos,
                    Parent = null,
                    G = 0,
                    F = CalcH(currentPos)
                };
            }

            openNodes.Push(firstNode);

            //FindOpenList(firstNode);


            bool result = false;
            while (openNodes.Count > 0)
            {
                Node openNode = openNodes.Pop();
                FindOpenList(openNode);
                closeNodes.Add(openNode);

                if (openNode.Pos == targetPos)
                {
                    result = true;
                    break;
                }
            }

            List<Vector3> route = new();
            if (result)
            {
                Node last = closeNodes[closeNodes.Count - 1];
                route.Add(TilemapManager.Instance.GetWorldPos(last.Pos));
                while (last.Parent != null)
                {
                    ////// 노드와 다음 노드 사이에 벽이 없으면 그냥 넣지 않는다
                    Vector3 pos = TilemapManager.Instance.GetWorldPos(last.Pos);
                    //Vector3 dir = TilemapManager.Instance.GetWorldPos(targetPos) - pos;
                    //Vector2 size = conCol.bounds.size;
                    //float angle = Vector3.Angle(targetPos, pos);
                    //if (!Physics2D.BoxCast(pos, size, angle, dir.normalized, dir.magnitude, obstacleLayer))
                    {
                        route.Add(pos);
                    }
                    last = last.Parent;
                }
                route.Add(conTrm.position);
                route.Reverse();
            }
            else
            {
                Debug.Log("길이 없음");
            }
            return route;

        }

        public Vector3 GetNextPos(Vector3 target)
        {
            if (IsNavActive == false) return Vector3.positiveInfinity;

            openNodes.Clear();
            closeNodes.Clear();
            roomNodes.ForEach((node) => node.Reset());

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(target);

            Node firstNode =
                roomNodes.Find((node) => node.Pos == currentPos);

            if (firstNode == null)
            {
                firstNode = new Node
                {
                    Pos = currentPos,
                    Parent = null,
                    G = 0,
                    F = CalcH(currentPos)
                };
            }

            FindOpenList(firstNode);

            Node openNode = openNodes.Pop();

            return TilemapManager.Instance.GetWorldPos(openNode.Pos);
        }

        private void FindOpenList(Node n)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == y) continue;

                    Vector3Int nextPos = n.Pos + new Vector3Int(x, y, 0);
                    
                    Node temp = closeNodes.Find(x => x.Pos == nextPos);
                    if (temp != null) continue;

                    if (CanMove(nextPos, n.Pos))
                    {
                        float g = (n.Pos - nextPos).magnitude + n.G;

                        Node nextOpenNode = roomNodes.Find((node)=> node.Pos == nextPos);
                        if (nextOpenNode == null) continue;

                        nextOpenNode.Parent = n;
                        nextOpenNode.G = g;
                        nextOpenNode.F = g + CalcH(nextPos) + nextOpenNode.Weight;

                        Node exist = openNodes.Contains(nextOpenNode);

                        if (exist != null)
                        {
                            
                            if (nextOpenNode.G < exist.G)
                            {
                                exist.G = nextOpenNode.G;
                                exist.F = nextOpenNode.F;
                                exist.Parent = nextOpenNode.Parent;
                            }
                        }
                        else
                        {
                            openNodes.Push(nextOpenNode);
                        }
                    }
                }
            }
        }

        private float CalcH(Vector3Int pos)
        {
            Vector3Int distance = targetPos - pos;
            return distance.magnitude;
        }

        public bool CanMove(Vector3Int pos, Vector3Int beforePos)
        {
            
            if (pos.x < roomBounds.xMin || pos.x > roomBounds.xMax
                || pos.y < roomBounds.yMin || pos.y > roomBounds.yMax)
            {
                return false;
            }

            Vector3 nPos = TilemapManager.Instance.GetWorldPos(pos);

            if (obstacleLayer != default(LayerMask))
            {
                float angle = Vector3.Angle(pos, beforePos);
                Vector3 dir = pos - beforePos;
                //아예 못지나가는 장애물 체크
                if (Physics2D.BoxCast(nPos, conCol.bounds.size * 1.1f,
                    angle, dir.normalized, 0.5f, obstacleLayer))
                {
                    Debug.Log("CheckWall");
                    return false;
                }
            }


            return TilemapManager.Instance.HasWallTile(pos) == false;
        }
    }
}

