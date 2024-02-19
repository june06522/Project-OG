using System.Collections.Generic;
using UnityEngine;


namespace Astar
{
    public class Navigation
    {
        List<Node> roomNodes; // 맵에 미리 구워놓고 그걸로 판단하려면 얘 살려야함. 나중에 최적화용.
        List<Node> roomWallNodes;

        List<Node> closeNodes;
        Heap openNodes;

        BoundsInt roomBounds;
        Vector3Int roomWorldPos;
        
        Collider2D conCol;
        Transform conTrm;
        //Transform targetTrm;
        LayerMask restrictLayer;

        Vector3Int currentPos;
        Vector3Int targetPos;

        public Navigation(Enemy enemy)
        {
            this.roomWorldPos = TilemapManager.Instance.GetTilePos(enemy.RoomInfo.pos);
            this.roomBounds = enemy.RoomInfo.bound;

            this.conCol = enemy.Collider;
            this.conTrm = enemy.transform;
            //this.targetTrm = enemy.TargetTrm;
            this.restrictLayer = enemy.EnemyDataSO.RestrictMovementLayer;

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            //targetPos = TilemapManager.Instance.GetTilePos(targetTrm.position);

            Bake(roomBounds);
        }

        //맵에 노드 할당
        public void Bake(BoundsInt roomBounds)
        {
            roomNodes = NodeGenerator.MakeNode(this.roomWorldPos, roomBounds, out roomWallNodes);
            openNodes = new Heap(100);
            closeNodes = new();
        }

        public Vector3 GetRandomPos()
        {
            return Random.Range(0, roomNodes.Count);
        }

        public List<Vector3Int> UpdateNav(Vector3 target)
        {
            openNodes.Clear();
            closeNodes.Clear();
            //roomNodes.ForEach((node) => node.Reset());

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(target);

            //Node firstNode =
            //    roomNodes.Find((node) => node.Pos == currentPos);

            openNodes.Push(new Node
            { 
                Pos = currentPos,
                Parent = null,
                G = 0,
                F = CalcH(currentPos)
            });

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

            List<Vector3Int> route = new();
            if (result)
            {
                Node last = closeNodes[closeNodes.Count - 1];
                while (last.Parent != null)
                {
                    route.Add(last.Pos);
                    last = last.Parent;
                }
                route.Reverse();
            }
            else
            {
                Debug.Log("길이 없음");
            }
            return route;

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

                    if (CanMove(nextPos))
                    {
                        int g = Mathf.RoundToInt((n.Pos - nextPos).magnitude) + n.G;

                        Node nextOpenNode = new Node
                        {
                            Pos = nextPos,
                            Parent = n,
                            G = g,
                            F = g + CalcH(nextPos)
                        };

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

        private int CalcH(Vector3Int pos)
        {
            Vector3Int distance = targetPos - pos;
            return Mathf.RoundToInt(distance.magnitude);
        }

        public bool CanMove(Vector3Int pos)
        {
            
            if (pos.x < roomBounds.xMin || pos.x > roomBounds.xMax
                || pos.y < roomBounds.yMin || pos.y > roomBounds.yMax)
            {
                return false;
            }

            Vector2 nPos = new Vector2(pos.x, pos.y);

            if(restrictLayer != default(LayerMask))
            {
                // 아예 못지나가는 장애물 체크
                if (Physics2D.OverlapBox(nPos, conCol.bounds.size, 0, restrictLayer) != null)
                {
                    return false;
                }
            }

            return TilemapManager.Instance.HasWallTile(pos) == false;
        }

        //public Node GetNode(Vector3Int pos)
        //{
        //    return roomNodes.Find((node) => node.Pos == pos);
        //}
    }
}

