using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public UnityEvent interactEvent;

    public void OnInteract()
    {
        interactEvent?.Invoke();
    }
}