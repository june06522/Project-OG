using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTransition : MonoBehaviour
{
    [SerializeField]
    private AudioClip _stageTransitionClip;
    [SerializeField]
    private CircleTransition _circleTransition;

    public void StartTransition(float time)
    {
        if(_stageTransitionClip != null)
            SoundManager.Instance.SFXPlay("Transition", _stageTransitionClip, 1f);

        _circleTransition.PlayCircleSizeChange(Vector3.one * 2800, Vector3.zero, time, true);
    }

    public void EndTransition(float time)
    {
        
        _circleTransition.PlayCircleSizeChange(Vector3.zero, Vector3.one * _circleTransition.CircleMaxValue, time);
        FAED.InvokeDelay(() =>
        {
            _circleTransition.SetOnOff(false);
        }, time + 0.1f);

    }

}
