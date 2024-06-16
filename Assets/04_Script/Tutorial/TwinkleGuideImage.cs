using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TwinkleGuideImage : MonoBehaviour
{
    private Image image;
    private float twinkleTime = 0.45f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(Twinkle());
    }

    IEnumerator Twinkle()
    {
        while(true) 
        {
            image.DOFade(0.1f, twinkleTime);
            yield return new WaitForSeconds(twinkleTime);
            image.DOFade(1f, twinkleTime);
            yield return new WaitForSeconds(twinkleTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
