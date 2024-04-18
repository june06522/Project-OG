using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomEnemy : MonoBehaviour, IHitAble
{

    public FeedbackPlayer feedbackPlayer { get; set; }

    private void Awake()
    {
        
        feedbackPlayer = GetComponent<FeedbackPlayer>();

    }

    public void Hit(float damage)
    {

        var suDamage = damage + Random.Range(0.25f, 1.75f);

        feedbackPlayer.Play(suDamage);

    }

}
