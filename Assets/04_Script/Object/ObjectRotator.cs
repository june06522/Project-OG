using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 dir = new Vector3(0, 0, 1);

    void Update()
    {
        transform.eulerAngles += dir.normalized * speed * Time.deltaTime;
    }
}
