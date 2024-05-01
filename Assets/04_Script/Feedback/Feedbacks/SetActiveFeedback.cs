using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFeedback : Feedback
{
    [Header("Info")]
    public GameObject activeObject;
    public bool value;

    public float delay = 0f;

    public override void Play(float damage)
    {
        FAED.InvokeDelay(() =>
        {
            activeObject.SetActive(value);
        }, delay);
        
    }

    
}
