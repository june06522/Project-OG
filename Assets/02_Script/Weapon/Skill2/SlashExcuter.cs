using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashExcuter : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Play();
        }
    }

    void Play()
    {
        DOTween.Sequence().
            AppendCallback(() =>
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }).
            AppendInterval(0.15f).
            AppendCallback(() =>
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }).
            AppendInterval(0.2f).
            AppendCallback(() =>
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(true);
            }).
            AppendInterval(0.2f).
            AppendCallback(() =>
            {
                transform.GetChild(4).gameObject.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(true);
            });
    }
}
