using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextFeedback : Feedback
{

    public DamageTextFeedback(FeedbackPlayer player) : base(player)
    {
    }

    public override void Play(float damage)
    {

        var pos = transform.position + (Vector3)Random.insideUnitCircle;

        FAED.TakePool<DamageText>("DamageText", pos).Set(damage);

    }

}
