using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _guideTextObject;
    [SerializeField]
    private TextMeshProUGUI _guideText;
    [SerializeField]
    private List<ContentSizeFitter> _textSizeFitter;

    private Sequence _sequence;

    public void ResetGuideText()
    {
        _guideText.text = string.Empty;
        _guideTextObject.SetActive(false);
    }

    public void SetGuideText(string text, float time)
    {
        if (_guideText == null)
            return;

        if (string.IsNullOrEmpty(text))
            ResetGuideText();

        if (_guideTextObject.activeInHierarchy == false)
            _guideTextObject.SetActive(true);

        text = $"{text} ";

        if (time == 0)
            _guideText.text = text;

        // Tween Guide Text
        _guideText.text = " ";

        if (_sequence != null && _sequence.active)
            _sequence.Kill();

        _sequence = DOTween.Sequence();
        _sequence.Append(_guideText.DOText(text, time));
        

    }
}
