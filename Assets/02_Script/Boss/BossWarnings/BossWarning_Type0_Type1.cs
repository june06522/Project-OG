using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning_Type0_Type1 : BossWarning
{
    private SpriteRenderer _sprite;
    private bool minous = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        minous = true;
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(1, 0, 0, 0.5f);
        StartCoroutine(Blinking());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StopAllCoroutines();
    }

    private IEnumerator Blinking()
    {
        float a = _sprite.color.a;
        while(true)
        {
            if (a <= 0.1f)
            {
                minous = false;
            }
            else if(a >= 0.5f)
            {
                minous = true;
            }

            if(minous)
            {
                _sprite.color = new Color(1, 0, 0, a -= Time.deltaTime);
            }
            else
            {
                _sprite.color = new Color(1, 0, 0, a += Time.deltaTime);
            }
            yield return null;
        }
    }
}
