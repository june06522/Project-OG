using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [NonSerialized]
    public RoomGenarator roomGenarator;

    [SerializeField] private MovePortal _portalPrefab;

    [SerializeField] PolygonCollider2D _vcamConfiner;

    int curIdxX;
    int curIdxY;

    int correctX;
    int correctY;

    private void Awake()
    {
        #region 예외처리
        if (Instance != null)
        {
            Debug.LogError($"{transform} : MapManger is Multiple running!");
            Destroy(gameObject);
        }   
        else
            Instance = this;

        if(_vcamConfiner == null)
        {
            Debug.LogError($"{transform} : _vcamConfiner is null");
        }

        if(roomGenarator == null )
            roomGenarator = GetComponent<RoomGenarator>();

        #endregion

        correctX = roomGenarator.Width / 2;
        correctY = roomGenarator.Height / 2;

        curIdxX = correctX;
        curIdxY = correctY;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            RoomClear();
    }

    public void RoomClear()
    {
        Vector2 tempPos = new Vector2((roomGenarator.RoomWidth + roomGenarator.BGLenth * 2) * (curIdxX - correctX),
            (roomGenarator.RoomHeight + roomGenarator.BGLenth * 2) * (curIdxY - correctY));

        int x = roomGenarator.RoomWidth / 2 - roomGenarator.PortalLenth;
        int y = roomGenarator.RoomHeight / 2 - roomGenarator.PortalLenth;

        if (roomGenarator.checkRoom[curIdxY + 1,curIdxX])
        {
            MovePortal obj = Instantiate(_portalPrefab);
            obj.dir = MoveDir.up;
            obj.transform.position = new Vector2(tempPos.x,tempPos.y + y);
        }

        if (roomGenarator.checkRoom[curIdxY - 1,curIdxX])
        {
            MovePortal obj = Instantiate(_portalPrefab);
            obj.dir = MoveDir.down;
            obj.transform.position = new Vector2(tempPos.x, tempPos.y - y);
        }

        if (roomGenarator.checkRoom[curIdxY, curIdxX + 1])
        {
            MovePortal obj = Instantiate(_portalPrefab);
            obj.dir = MoveDir.right;
            obj.transform.position = new Vector2(tempPos.x + x, tempPos.y);
        }

        if (roomGenarator.checkRoom[curIdxY, curIdxX - 1])
        {
            MovePortal obj = Instantiate(_portalPrefab);
            obj.dir = MoveDir.left;
            obj.transform.position = new Vector2(tempPos.x - x, tempPos.y);
        }

    }

    public void RoomMove(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.left:
                curIdxX--;
                break;
            case MoveDir.right:
                curIdxX++;
                break;
            case MoveDir.up:
                curIdxY++;
                break;
            case MoveDir.down:
                curIdxY--;
                break;
        }
    }
}