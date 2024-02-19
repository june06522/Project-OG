using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Astar
{
    public class NodeGenerator : MonoBehaviour
    {
        //Debuging
        public List<Vector3> wallNodes = new List<Vector3>();
        public List<Vector3> obstacleNodes = new List<Vector3>();
        public List<Vector3> groundNodes = new List<Vector3>();

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0,0,0, 0.8f);
            wallNodes.ForEach(node => Gizmos.DrawCube(node, Vector3.one));
     
            Gizmos.color = new Color(255,255,255, 0.3f);
            groundNodes.ForEach(node => Gizmos.DrawCube(node, Vector3.one)); 
     
            Gizmos.color = new Color(255, 0, 255, 0.5f);
            obstacleNodes.ForEach(node => Gizmos.DrawCube(node, Vector3.one));
        }


        public Task<List<Node>> MakeNode(Vector3Int worldCenterPos, BoundsInt bounds)
        {
            List<Node> nodes = new List<Node>();

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {

                Node node = new Node();
                node.Pos = pos + worldCenterPos;
                node.Type = NodeType.MoveAble;

                Vector2 worldPos = TilemapManager.Instance.GetWorldPos(pos);
                Vector2 size = new Vector2(0.9f, 0.9f);

                int wallLayer = LayerMask.NameToLayer("Wall");
                int obstacleLayer = LayerMask.NameToLayer("Obstacle");

                Collider2D collider = Physics2D.OverlapBox(worldPos, size, 0f, (1 << wallLayer) | (1 << obstacleLayer));
                Debug.Log(wallLayer);
                Debug.Log(obstacleLayer);

                if (collider != null)
                {
                    Debug.Log(collider.gameObject.layer);
                    if (collider.gameObject.layer == wallLayer)
                    {
                        node.Type = NodeType.Locomobile;
                        node.Weight = 9999;
                      
                        wallNodes.Add(TilemapManager.Instance.GetWorldPos(node.Pos));
                    }
                    else if (collider.gameObject.layer == obstacleLayer)
                    {
                        Obstacle obstacle;
                        if(collider.TryGetComponent<Obstacle>(out obstacle))
                        {
                            node.Weight = obstacle.Weight;
                        }

                        obstacleNodes.Add(TilemapManager.Instance.GetWorldPos(node.Pos));
                    }
                }
                else
                {
                    node.Weight = 0;
                    groundNodes.Add(TilemapManager.Instance.GetWorldPos(node.Pos));
                }
                nodes.Add(node);
            }
            Debug.Log("End");
            return Task.FromResult(nodes);
        }
    }
}
