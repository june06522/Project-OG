using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShakeFeedback : Feedback
{

    private readonly int HASH_SHAKE = Shader.PropertyToID("_VibrateFade");
    private Coroutine currentCo;
    private bool isShake;

    public HitShakeFeedback(FeedbackPlayer player) : base(player)
    {
    }

    public override void Play(float damage)
    {

        if (!isShake)
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

        isShake = true;

        spriteRenderer.material.SetFloat(HASH_SHAKE, 1f);
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.SetFloat(HASH_SHAKE, 0f);

        isShake = false;

    }

}
