using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticFeedback : Feedback
{
    [Header("Value")]
    public float time = 0.1f;

    public override void Play(float damage)
    {
        CameraManager.Instance.DamageVolume(Mathf.Clamp(damage * 0.005f, 0f, 1f), time);
    }
}
