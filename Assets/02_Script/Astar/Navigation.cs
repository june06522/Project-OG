
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Astar
{
    public class Navigation
    {
        List<Node> roomNodes;
        List<Node> closeNodes;
        Heap openNodes;

        BoundsInt roomBounds;
        Vector3Int roomWorldPos;
        
        Collider2D conCol;
        Transform conTrm;
        Transform targetTrm;
        LayerMask restrictLayer;

        Vector3Int currentPos;
        Vector3Int targetPos;

        List<Vector3Int> route;


        public Navigation(Vector3 roomWorldPos, BoundsInt roomBounds, Enemy enemy)
        {
            this.roomWorldPos = TilemapManager.Instance.GetTilePos(roomWorldPos);
            this.roomBounds = roomBounds;

            this.conCol = enemy.Collider;
            this.conTrm = enemy.transform;
            this.targetTrm = enemy.TargetTrm;
            this.restrictLayer = enemy.EnemyDataSO.RestrictMovementLayer;

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(targetTrm.position);


            roomNodes = NodeGenerator.MakeNode(this.roomWorldPos, roomBounds);
            openNodes = new Heap(roomNodes.Count);
            closeNodes = new();
        }


        public void UpdateNav()
        {
            openNodes.Clear();
            closeNodes.Clear();
            roomNodes.ForEach(node => { node.Reset(); });

            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(targetTrm.position);

            Node firstNode = roomNodes.Find((node) => node.Pos == currentPos);
            openNodes.Push(firstNode);

            Node targetNode = roomNodes.Find((node) => node.Pos == targetPos);


            bool result = false;
            while (openNodes.Count != 0)
            {
                Node openNode = openNodes.Pop();
                closeNodes.Add(openNode);
                FindOpenList(openNode);

                if (openNodes.Root == targetNode)
                {
                    result = true;
                    break;
                }
            }

            if(result)
            {
                closeNodes.Reverse();
                closeNodes.ForEach((node) => route.Add(node.Pos));
            }
            else
            {
                Debug.Log("길이 없음");
            }
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
    }
}

