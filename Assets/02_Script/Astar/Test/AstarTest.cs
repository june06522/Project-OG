using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar;
using UnityEngine.Tilemaps;

public class AstarTest : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;
    Navigation manager;

    private void Awake()
    {
        manager = new Navigation(Vector3.zero, tilemap.cellBounds);
    }
    


}
