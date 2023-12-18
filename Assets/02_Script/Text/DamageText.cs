using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    private TMP_Text text;

    private void Awake()
    {
        
        text = GetComponent<TMP_Text>();

    }

    public void Set(float damage)
    {

        text.text = damage.ToString("0.#");

        transform.localScale = new Vector3(1.75f, 0.15f, 1);
        text.color = Color.white;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBounce));
        seq.Join(transform.DOMoveY(transform.position.y + 0.2f, 0.4f).SetEase(Ease.InOutElastic));
        seq.Join(text.DOColor(Color.red, 0.4f).SetEase(Ease.InSine));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {

            FAED.InsertPool(gameObject);

        });

    }

}
