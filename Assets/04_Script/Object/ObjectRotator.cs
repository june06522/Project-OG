using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, speed) * Time.deltaTime;
    }
}
