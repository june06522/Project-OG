using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnGateEvent;
    public Stage NextStage {  get; private set; }

    public void SetStage(Stage nextStage)
    {
        NextStage = nextStage;
    }

    public void OnInteract()
    {
        OnGateEvent?.Invoke();
    }


}
