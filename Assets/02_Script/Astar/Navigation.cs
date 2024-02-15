
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
        List<Node> openNodes;

        BoundsInt roomBounds;
        Vector3Int roomWorldPos;

        Vector3Int currentPos;
        Vector3Int targetPos;

        List<Vector3Int> route;

        public Navigation(Transform conTrm, Transform targetTrm, Vector3 roomWorldPos, BoundsInt roomBounds)
        {
            this.roomBounds = roomBounds;
            this.roomWorldPos = TilemapManager.Instance.GetTilePos(roomWorldPos);
            
            currentPos = TilemapManager.Instance.GetTilePos(conTrm.position);
            targetPos = TilemapManager.Instance.GetTilePos(targetTrm.position);

            roomNodes = NodeGenerator.MakeNode(this.roomWorldPos, roomBounds);
            
            openNodes = new();
            closeNodes = new();

            Node openNode = roomNodes.Find((node) => node.Pos == currentPos);
            openNodes.Add(openNode);
        }


        public void UpdateNav()
        {
            Node targetNode = roomNodes.Find((node) => node.Pos == targetPos);
            while (openNodes.Count == 0 || targetNode )
            if()
        }



        public void FindOpenList(Node node)
        {
            node.
        }

        public bool CanMove(Vector3Int pos)
        {
            if (pos.x < roomBounds.xMin || pos.x > roomBounds.xMax
                || pos.y < roomBounds.yMin || pos.y > roomBounds.yMax)
            {
                return false;
            }
            return TilemapManager.Instance.HasWallTile(pos) == false;
        }
    }
}

