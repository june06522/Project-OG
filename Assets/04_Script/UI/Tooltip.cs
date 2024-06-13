using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct RateColor
{
    public ItemRate Rate;
    public Color color;
}

public class Tooltip : MonoBehaviour
{
    [Header("Tooltip value")]
    [SerializeField] List<RateColor> colorList;
    [SerializeField] float duration;
    [SerializeField] Ease ease;
    Material tooltipMat;

    [Header("TextValue")]
    [SerializeField]
    Material textMat;
    [SerializeField]
    float textTweenDuration;

    private readonly float _tooltipDissolveStart = 1.6f;
    private readonly float _tooltipDissolveEnd = -0.2f;

    private readonly float _textDissolveStart = 0f;
    private readonly float _textDissolveEnd = 1f;

    private readonly string directionalGlowFade = "_DirectionalGlowFadeFade";
    private readonly string alphaFade = "_FullAlphaDissolveFade";

    public ItemRate CurrentItemRate { get; set; }

    RectTransform rectTransform;
    Sequence seq;

    DissolveParameters tooltipDissolveParmas;
    DissolveParameters textDissolveParams;

    private ImageDissolve imageDissolve;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Image img = GetComponent<Image>();
        img.material = Instantiate(img.material);
        tooltipMat = img.materialForRendering;

        imageDissolve = transform.Find("Image").GetComponent<ImageDissolve>();

        tooltipDissolveParmas =
            new DissolveParameters(tooltipMat, _tooltipDissolveStart, _tooltipDissolveEnd, duration, ease, directionalGlowFade);

        textDissolveParams =
            new DissolveParameters(textMat, _textDissolveStart, _textDissolveEnd, textTweenDuration, Ease.Linear, alphaFade);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //Material ÃÊ±âÈ­
        tooltipMat.SetFloat(directionalGlowFade, _textDissolveStart);
        textMat.SetFloat(alphaFade, _textDissolveStart);
        imageDissolve.Init(GetColor(CurrentItemRate));

        seq.Kill();
    }

    private Color GetColor(ItemRate currentItemRate)
    {
        return colorList.Find((item) => item.Rate == currentItemRate).color;
    }

    public void On(Vector2 pos)
    {
        rectTransform.parent.position = pos;

        Init();

        seq = DOTween.Sequence();

        seq.Append(Dissolver.Dissolve(tooltipDissolveParmas, true));
        seq.Append(Dissolver.Dissolve(textDissolveParams, true));
        seq.Append(imageDissolve.Dissolve(true));

        seq.Restart();
    }

    public void FadeIn()
    {
        
    }

    public void FadeOut()
    {

    }
}
