using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    Transform obj;

    private void Start()
    {
        obj = GameManager.Instance.player;
    }

    private void Update()
    {
        transform.position = obj.position;
    }
}
