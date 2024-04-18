using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTest : MonoBehaviour, IHitAble
{
    public FeedbackPlayer feedbackPlayer { get; set; }

    private void Awake()
    {
        feedbackPlayer = GetComponent<FeedbackPlayer>();
    }
}
