using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectZone : MonoBehaviour
{
    [HideInInspector] public
    BoxCollider2D boxCol2D;

    private void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            MapManager.Instance.curZone = this;
    }
}