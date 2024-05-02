using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IHitAble
{
    [field: SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }

    //[SerializeField, Range(0f, 1f)]
    //private float _goldDropPercent = 0f;

    [SerializeField]
    private float _minDropGold;
    [SerializeField]
    private float _maxDropGold;

    [SerializeField]
    private ParticleSystem _brokenEffect;
    [SerializeField]
    private int _endureCount = 1;

    public bool Hit(float damage)
    {
        feedbackPlayer.Play(damage + Random.Range(0.25f, 1.75f));

        _endureCount--;
        if (_endureCount <= 0)
        {
            BrakingObject();
            BreakingEffect();
        }

        return true;
    }

    private void BreakingEffect()
    {
        if(_brokenEffect != null)
        { 

            ParticleSystem Effect = Instantiate(_brokenEffect, transform.position, Quaternion.identity);
            Effect.Play();

            Destroy(Effect, 2f);

        }
    }

    protected virtual void BrakingObject()
    {

    }
}
