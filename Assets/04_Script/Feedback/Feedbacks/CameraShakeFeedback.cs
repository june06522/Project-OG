using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeFeedback : Feedback
{
    [SerializeField]
    private float _shakeValue = 5f;
    [SerializeField]
    private float _shakeTime = 0.2f;

    public override void Play(float damage)
    {
        CameraManager.Instance.CameraShake(_shakeValue, _shakeTime);
    }
}
