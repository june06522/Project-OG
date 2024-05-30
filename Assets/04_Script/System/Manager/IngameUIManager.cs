using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoSingleton<IngameUIManager>
{
    [Header("Stage Text")]
    [SerializeField]
    private Image _titleBackground;
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _subText;

    private Sequence _sequence = null;

    [Header("Ingame UI")]
    [SerializeField]
    private IngameTooltip _ingameTooltip;
    public IngameTooltip IngameTooltip => _ingameTooltip;

    public void SetStageTitle(string titleText, string subText, float interval = 0f, float textTime = 0.1f, float time = 1f)
    {
        if (_titleText == null || _subText == null)
            return;

        // Title
        _titleText.text = titleText;

        // Tip
        _subText.text = "";

        // Anim
        _titleText.rectTransform.localScale = Vector3.one;
        _titleText.color = new Color(1f, 1f, 1f, 0f);
        _subText.color = new Color(1f, 1f, 1f, 1f);

        if (_sequence != null && _sequence.active)
            _sequence.Kill();

        _sequence = DOTween.Sequence();
        _sequence.AppendInterval(interval);
        _sequence.Append(_titleBackground.rectTransform.DOScaleY(1, 0.5f).SetEase(Ease.OutBounce));
        _sequence.Append(_titleText.rectTransform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutElastic))
            .Join(_titleText.DOFade(1f, 0.15f));
        _sequence.AppendInterval(1f);
        _sequence.Append(_subText.DOText(subText, subText.Length * textTime));

        _sequence.AppendInterval(time);

        _sequence.Append(_titleText.DOFade(0f, 2f))
            .Join(_subText.DOFade(0f, 2f));
        _sequence.Append(_titleBackground.rectTransform.DOScaleY(0, 0.2f));

    }


}
