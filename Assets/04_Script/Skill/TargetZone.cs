using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector3(0.001f, 0.001f);
    }

    public void Marking(float time, Vector3 target)
    {
        transform.DOScale(target, time);
        //DOTween.To(() => t, cur => material.SetFloat("_Size", cur), 1, time).SetEase(Ease.InSine);
    }
}
