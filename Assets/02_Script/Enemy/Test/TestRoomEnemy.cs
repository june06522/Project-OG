using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomEnemy : MonoBehaviour, IHitAble
{

    private DPSSystem dpsSys;
    public FeedbackPlayer feedbackPlayer { get; set; }

    private void Awake()
    {
        
        feedbackPlayer = GetComponent<FeedbackPlayer>();
        dpsSys = GetComponent<DPSSystem>();

    }

    public void Hit(float damage)
    {

        var suDamage = damage + Random.Range(0.25f, 1.75f);

        feedbackPlayer.Play(suDamage);
        dpsSys.TakeDamage(suDamage);

    }

}
