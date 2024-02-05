using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    normalRoom,
    HardRoom,
    Shop,
}

public class RoomInfo
{
    public int x; // 위치인덱스X
    public int y; // 위치인덱스Y
    public RoomType roomType = RoomType.normalRoom;
}
