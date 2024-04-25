using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdamageShockwaveFeedback : Feedback
{
    float overdamageStandard = 100f;
    [SerializeField]
    private bool _justShockwave = false;

    [Header("Value")]
    public float _strength = -0.3f;
    public float _endValue = 0.5f;
    public float _shockwaveTime = 0.8f;

    public override void Play(float damage)
    {
        if(_justShockwave || damage >= overdamageStandard)
        {
            CameraManager.Instance.Shockwave(transform.position, _strength, _endValue, _shockwaveTime);

        }
    }
}
