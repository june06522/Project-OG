using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Astar
{
    public class NodeGenerator
    {
        public static List<Node> MakeNode(Vector3Int worldCenterPos, BoundsInt bounds, out List<Node> roomWallNodes)
        {
            List<Node> nodes = new List<Node>();
            roomWallNodes = new List<Node>();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {

                Node node = new Node();
                node.Pos = pos + worldCenterPos;
                node.Type = NodeType.MoveAble;


                int wallLayer = LayerMask.GetMask("Wall");
                int obstacleLayer = LayerMask.GetMask("Obstacle");

                //가중치는 장애물 완성되고 따로 할당.
                Collider2D collider = Physics2D.OverlapCircle(TilemapManager.Instance.GetWorldPos(pos)
                                                                  ,0.5f, wallLayer | obstacleLayer);
                
                if(collider != null)
                {
                    if(collider.gameObject.layer == wallLayer)
                    {
                        roomWallNodes.Add(node);
                        node.Type = NodeType.Locomobile;
                    }
                    else if(collider.gameObject.layer == obstacleLayer)
                    {
                        // Obstacle 가중치 가져오기
                        // node.Weight = ??;
                    }
                }
                else
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }
    }
}
