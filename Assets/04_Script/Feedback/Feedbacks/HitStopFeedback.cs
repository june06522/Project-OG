using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopFeedback : Feedback
{
    [SerializeField]
    private float _stopTime;

    public override void Play(float damage)
    {

        TimeManager.instance.Stop(_stopTime, 0.03f);

    }

}