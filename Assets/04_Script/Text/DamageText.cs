using DG.Tweening;
using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    private TMP_Text text;

    public Color _weakColor;
    public Color _simpleColor;
    public Color _powerfulColor;
    public Color _unbelievableColor;


    private void Awake()
    {
        
        text = GetComponent<TMP_Text>();

    }

    public void Set(float damage)
    {

        text.text = damage.ToString("0");

        transform.localScale = new Vector3(1.75f, 0.15f, 1);
        text.color = Color.white;

        Color tweenColor = DamageColor(damage);
        Vector3 tweenScale = DamageScale(damage);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(transform.position.y + 0.2f, 0.25f).SetEase(Ease.InOutElastic));
        seq.Join(transform.DOScale(tweenScale + Vector3.one * 0.8f, 0.2f).SetEase(Ease.OutBounce));
        seq.Append(transform.DOScale(new Vector3(0.6f, 1.75f), 0.1f).SetEase(Ease.InOutBounce));
        seq.Append(transform.DOScale(tweenScale, 0.1f).SetEase(Ease.OutBounce));
        seq.InsertCallback(0.2f, () =>
        {

            text.color = tweenColor;

        });
        seq.AppendInterval(0.15f);
        seq.Append(text.DOFade(0, 0.15f).SetEase(Ease.InOutSine));
        seq.AppendCallback(() =>
        {

            FAED.InsertPool(gameObject);

        });

    }

    private Vector3 DamageScale(float damage)
    {
        Vector3 scale = Vector3.one;

        if (damage < 10f)
        {
            scale = Vector3.one * 0.5f;
        }
        else if (damage < 100f)
        {
            scale = Vector3.one * 1.4f;
        }
        else if (damage < 1000f)
        {
            scale = Vector3.one * 2f;
        }
        else
        {
            scale = Vector3.one * 5f;
        }

        return scale;
    }

    private Color DamageColor(float damage)
    {
        Color returnColor = Color.white;

        if(damage < 10f)
        {
            returnColor = _weakColor;
        }
        else if(damage < 100f)
        {
            returnColor = _simpleColor;
        }
        else if(damage < 1000f)
        {
            returnColor = _powerfulColor;
        }
        else
        {
            returnColor = _unbelievableColor;
        }

        return returnColor;
    }
}
