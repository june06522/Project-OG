using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundFeedback : Feedback
{
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private float _volume = 0.5f;


    public override void Play(float damage)
    {
        SoundManager.Instance.SFXPlay($"{gameObject.name}'s HitSound", _clip, _volume);
    }
}
