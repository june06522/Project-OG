using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomRoom : MonoBehaviour
{
    public Tilemap wallTilemap;
    public Tilemap tilemap;

    public GameObject obstacleParent;

    public int width; // 벽포함 가로
    public int height; // 벽포함 세로

    [HideInInspector]
    public Vector3 centerPos;
}
