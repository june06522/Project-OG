using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbag : MonoBehaviour, IHitAble
{
    [field:SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }
    public event Action<float> OnHit;

    public bool Hit(float damage)
    {
        feedbackPlayer.Play(damage + UnityEngine.Random.Range(0.25f, 1.75f));
        OnHit?.Invoke(damage);
        return true;
    }
}
