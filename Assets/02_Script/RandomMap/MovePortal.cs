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

    bool isContact = false;
    Vector2 centerpos;

    private void Start()
    {
        playerTrm = GameManager.Instance.player;
    }

    private void Update()
    {
        if (isContact && Input.GetKeyDown(KeyCode.F))
            Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isContact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isContact = false;
    }

    private void Move()
    {
        int len = MapManager.Instance.roomGenarator.PortalLenth;

        switch (dir)
        {
            case MoveDir.left:
                MapManager.Instance.RoomMove(MoveDir.left);
                centerpos = new Vector2(
                    MapManager.Instance.roomGenarator.WidthLength * (MapManager.Instance.CurIdxX - MapManager.Instance.CorrectX),
                    MapManager.Instance.roomGenarator.HeightLength * (MapManager.Instance.CurIdxY - MapManager.Instance.CorrectY));
                playerTrm.position = new Vector3(centerpos.x + MapManager.Instance.GetRoomSize().width / 2 - len,
                    transform.position.y, transform.position.z);
                break;
            case MoveDir.right:
                MapManager.Instance.RoomMove(MoveDir.right);
                centerpos = new Vector2(
                    MapManager.Instance.roomGenarator.WidthLength * (MapManager.Instance.CurIdxX - MapManager.Instance.CorrectX),
                    MapManager.Instance.roomGenarator.HeightLength * (MapManager.Instance.CurIdxY - MapManager.Instance.CorrectY));
                playerTrm.position = new Vector3(centerpos.x - MapManager.Instance.GetRoomSize().width / 2 + len,
                    transform.position.y, transform.position.z);
                break;
            case MoveDir.up:
                MapManager.Instance.RoomMove(MoveDir.up);
                centerpos = new Vector2(
                    MapManager.Instance.roomGenarator.WidthLength * (MapManager.Instance.CurIdxX - MapManager.Instance.CorrectX),
                    MapManager.Instance.roomGenarator.HeightLength * (MapManager.Instance.CurIdxY - MapManager.Instance.CorrectY));
                playerTrm.position = new Vector3(transform.position.x,
                   centerpos.y - MapManager.Instance.GetRoomSize().height / 2+ len
                    , transform.position.z);
                break;
            case MoveDir.down:
                MapManager.Instance.RoomMove(MoveDir.down);
                centerpos = new Vector2(
                    MapManager.Instance.roomGenarator.WidthLength * (MapManager.Instance.CurIdxX - MapManager.Instance.CorrectX),
                    MapManager.Instance.roomGenarator.HeightLength * (MapManager.Instance.CurIdxY - MapManager.Instance.CorrectY));
                playerTrm.position = new Vector3(transform.position.x,
                   centerpos.y + MapManager.Instance.GetRoomSize().height / 2 - len
                    , transform.position.z);
                break;
        }
    }
}