using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticFeedback : Feedback
{
    [Header("Value")]
    public float time = 0.1f;
    public bool isPlayer = false;

    public override void Play(float damage)
    {
        if(isPlayer)
        {
            CameraManager.Instance.PlayerDamageVolume(1f, time);

        }
        else
        {
            CameraManager.Instance.DamageVolume(Mathf.Clamp(damage * 0.005f, 0f, 1f), time);

        }
    }
}