using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementChecker : MonoBehaviour
{
    public static CameraMovementChecker Instance;

    private CinemachineVirtualCamera virtualCamera;
    private Vector3 lastPosition;

    [HideInInspector]
    public bool isMoving = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : CameraMovementChecker is Multiply running!");
            Destroy(gameObject);
        }

        virtualCamera = GameObject.Find("Manager/GameManager/GameVisual/CM").GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        if (virtualCamera != null)
        {
            lastPosition = virtualCamera.transform.position;
        }
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            Vector3 currentPosition = virtualCamera.transform.position;
            if (currentPosition != lastPosition)
            {
                isMoving = true;
                lastPosition = currentPosition;
            }
            else
                isMoving = false;
        }
    }
}
