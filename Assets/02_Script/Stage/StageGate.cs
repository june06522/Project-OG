using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnGateEvent;


    public void OnInteract()
    {
        OnGateEvent?.Invoke();
    }


}
