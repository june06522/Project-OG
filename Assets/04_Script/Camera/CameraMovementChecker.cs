using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementChecker : MonoBehaviour
{
    public static CameraMovementChecker Instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

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
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void Off() => transposer.enabled = false;
    public void On() => transposer.enabled = true;
}
