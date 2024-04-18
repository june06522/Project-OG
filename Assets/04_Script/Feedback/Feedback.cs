using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback
{

    protected SpriteRenderer spriteRenderer;
    protected FeedbackPlayer player;
    protected Transform transform;
    protected GameObject gameObject;

    public Feedback(FeedbackPlayer player)
    {

        this.player = player;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        gameObject = player.gameObject;
        transform = gameObject.transform;

    }

    public abstract void Play(float damage);

}
