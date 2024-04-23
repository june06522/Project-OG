using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitShakeFeedback : Feedback
{
    [SerializeField]
    private List<SpriteRenderer> _sprites;

    [SerializeField]
    private float _shakeTime = 0.2f;


    private readonly int HASH_SHAKE = Shader.PropertyToID("_VibrateFade");
    private Coroutine currentCo;
    private bool isShake;

    private List<Color> _defaultColor = new List<Color>();

    private void Start()
    {
        foreach (var sprite in _sprites)
        {
            _defaultColor.Add(sprite.color);
        }
    }

    public override void Play(float damage)
    {

        if (!isShake)
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

        isShake = true;

        foreach(var sprite in _sprites)
        {
            sprite.material.SetFloat(HASH_SHAKE, 1f);
            sprite.color = Color.white;
        }
        
        yield return new WaitForSeconds(_shakeTime);

        for(int i = 0; i < _sprites.Count; ++i)
        {
            var sprite = _sprites[i];

            sprite.material.SetFloat(HASH_SHAKE, 0f);
            sprite.color = _defaultColor[i];
        }

        isShake = false;

    }

}
