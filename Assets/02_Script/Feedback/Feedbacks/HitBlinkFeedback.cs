using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlinkFeedback : Feedback
{

    private readonly int HASH_BLINK = Shader.PropertyToID("_StrongTintFade");
    private Coroutine currentCo;
    private bool isBlink;

    public HitBlinkFeedback(FeedbackPlayer player) : base(player)
    {
    }

    public override void Play(float damage)
    {

        if (!isBlink)
        {

            currentCo = player.StartCoroutine(BlinkCo());

        }
        else
        {

            player.StopCoroutine(currentCo);
            currentCo = player.StartCoroutine(BlinkCo());

        }

    }

    private IEnumerator BlinkCo()
    {

        isBlink = true;

        spriteRenderer.material.SetFloat(HASH_BLINK, 1f);
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.SetFloat(HASH_BLINK, 0f);

        isBlink = false;

    }

}
