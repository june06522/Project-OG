using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar;
using UnityEngine.Tilemaps;

public class AstarTest : MonoBehaviour
{
    [SerializeField] Enemy testEnemy;
    [SerializeField]
    Tilemap tilemap;
    [SerializeField]
    Transform controllerTrm;
    [SerializeField]
    Transform playerTrm;

    Navigation manager;

    List<int> list = new List<int>() { 5,2,1,3,4,6,8,9,0, 10};
    private void Awake()
    {
        Heap heap = new Heap(10);
        for(int i = 0; i< 10; i++)
        {
            Node node = new Node();
            node.F = list[i];
            heap.Push(node);
        }
        
        Debug.Log($"heapCOunt : {heap.Count}");
        Debug.Log($"heapRoot : {heap.Root.F}");

        for(int i = 0; i < 10; i++)
        {
            Node node = heap.Pop();
            Debug.Log($"heapRoot : {node.F}");
        }

        manager = new Navigation(Vector3.zero, tilemap.cellBounds, testEnemy);
    }

    private void Update()
    {
        manager.UpdateNav();
    }



}
