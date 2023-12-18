using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback
{

    protected FeedbackPlayer player;
    protected SpriteRenderer spriteRenderer;

    public Feedback(FeedbackPlayer player)
    {

        this.player = player;
        spriteRenderer = player.GetComponent<SpriteRenderer>();

    }

    public abstract void Play(float damage);

}
