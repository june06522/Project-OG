using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopFeedback : Feedback
{

    public HitStopFeedback(FeedbackPlayer player) : base(player)
    {
    }

    public override void Play(float damage)
    {

        TimeManager.instance.Stop(0.4f, 0.03f);

    }

}