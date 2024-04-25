using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlinkFeedback : Feedback
{
    [SerializeField]
    private List<SpriteRenderer> _sprites;

    [SerializeField]
    private float _blinkTime = 0.1f;


    private readonly int HASH_BLINK = Shader.PropertyToID("_StrongTintFade");
    private Coroutine currentCo;
    private bool isBlink;

    public override void Play(float damage)
    {
        if (!isBlink)
        {

            currentCo = StartCoroutine(BlinkCo());

        }
        else
        {
            StopCoroutine(currentCo);
            currentCo = StartCoroutine(BlinkCo());

        }

    }

    private IEnumerator BlinkCo()
    {

        isBlink = true;

        foreach (var sprite in _sprites)
        {
            sprite.material.SetFloat(HASH_BLINK, 1f);
        }

        yield return new WaitForSeconds(_blinkTime);

        foreach (var sprite in _sprites)
        {
            sprite.material.SetFloat(HASH_BLINK, 0f);
        }


        isBlink = false;

    }

}
