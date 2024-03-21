using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning_Type1 : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private bool minous = true;

    private void OnEnable()
    {
        minous = true;
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(1, 0, 0, 0.5f);
        StartCoroutine(Blinking());
    }

    private void OnDisable()
    {
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
