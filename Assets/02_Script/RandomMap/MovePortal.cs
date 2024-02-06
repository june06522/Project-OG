using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDir
{
    left = 0, right = 1, up = 2, down = 3,
}

public class MovePortal : MonoBehaviour
{
    public MoveDir dir = MoveDir.left;

    private Transform playerTrm;

    private void Start()
    {
        playerTrm = GameManager.Instance.player;
        Debug.Log(playerTrm);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Move();
            }
        }
    }

    private void Move()
    {
        int len = (MapManager.Instance.roomGenarator.PortalLenth + 
            MapManager.Instance.roomGenarator.BGLenth) * 2;
        switch (dir)
        {
            case MoveDir.left:
                playerTrm.position = new Vector3(transform.position.x - len,
                    transform.position.y, transform.position.z);
                MapManager.Instance.RoomMove(MoveDir.left);
                break;
            case MoveDir.right:
                playerTrm.position = new Vector3(transform.position.x + len,
                    transform.position.y, transform.position.z);
                MapManager.Instance.RoomMove(MoveDir.right);
                break; 
            case MoveDir.up:
                playerTrm.position = new Vector3(transform.position.x,
                    transform.position.y + len
                    , transform.position.z);
                MapManager.Instance.RoomMove(MoveDir.up);
                break; 
            case MoveDir.down:
                playerTrm.position = new Vector3(transform.position.x,
                    transform.position.y - len
                    , transform.position.z);
                MapManager.Instance.RoomMove(MoveDir.down);
                break;
        }
    }
}