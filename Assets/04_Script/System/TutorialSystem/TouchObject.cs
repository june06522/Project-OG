using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public event Action OnTouchEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTouchEvent?.Invoke();
    }
}
