using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Ease ease;

    private List<TextDissolve> titleTexts;
    private List<TextDissolve> mainTexts;
    private ImageDissolve image;

    private readonly string shader = "_DirectionalGlowFadeFade";
    private readonly float fadeEnd = 0;
    private readonly float fadeStart = 1.5f;

    Material mat;

    RectTransform rectTransform;
    Sequence seq;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Image img = GetComponent<Image>();
        img.material = Instantiate(img.material);
        mat = img.materialForRendering;
        mat.SetFloat(shader, fadeStart); 
        

        image = transform.Find("Image").GetComponent<ImageDissolve>();
        TextDissolve[] texts = GetComponentsInChildren<TextDissolve>();
        
        titleTexts = (from text in texts
                     where text.Type == TextType.TITLE
                     select text).ToList();

        mainTexts = (from text in texts
                      where text.Type == TextType.MAIN
                      select text).ToList();
    }

    private void Start()
    {
        Init();
    }


    public void Init()
    {
        mat.SetFloat(shader, fadeStart);    
        titleTexts.ForEach((text) => text.Init());
        mainTexts.ForEach((text) => text.Init());
        image.Init();
    }

    public void On(Vector2 pos)
    {
        rectTransform.parent.position = pos;

        Init();

        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => fadeStart,
                  (value) => mat.SetFloat(shader, value), 
                  fadeEnd,
                  duration));
        
        seq.Append(image.Dissolve());
        for(int i = 0; i < titleTexts.Count; ++i)
        {
            seq.Join(titleTexts[i].Dissolve(true));
        }

        seq.Append(null);
        for (int i = 0; i < mainTexts.Count; ++i)
        {
            seq.Join(mainTexts[i].Dissolve(true));
        }
        
        seq.Restart();
    }
}
