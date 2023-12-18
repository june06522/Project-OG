using UnityEngine;

public interface IHitAble
{

    public FeedbackPlayer feedbackPlayer { get; set; }

    public void Hit(float damage)
    {

        feedbackPlayer.Play(damage + Random.Range(0.25f, 1.75f));

    }

}
