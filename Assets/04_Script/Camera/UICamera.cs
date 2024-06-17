using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    Camera uiCamera;
    Camera mainCamera;

    private void Start()
    {
        uiCamera = GetComponent<Camera>();
        mainCamera = transform.parent.GetComponent<Camera>();
    }

    private void Update()
    {
        uiCamera.orthographicSize = mainCamera.orthographicSize;
    }
}
