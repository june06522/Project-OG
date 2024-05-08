using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageDissolve : MonoBehaviour
{
    Image image;
    Material mat;
    
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    private readonly string shader = "_SourceGlowDissolveFade";

    float startPos = 0f;
    float endPos = 1f;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.material = Instantiate(image.material);
        mat = image.materialForRendering;
    }

    public void Init()
    {
        mat.SetFloat(shader, startPos);
    }

    public Tween Dissolve()
    {
        Debug.Log("why");
        return DOTween.To(() => startPos,
                  (value) => mat.SetFloat(shader, value),
                  endPos,
                  duration);
    }
}
