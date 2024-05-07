using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _goldValueText;

    int _curGoldValue = 0;
    int _changedGoldValue = 0;

    float _transitionTime = 0.5f;

    private void Start()
    {
        _curGoldValue = Money.Instance.Gold;
        _goldValueText.text = _curGoldValue.ToString();
        Money.Instance.GoldChangedEvent += HandleSetGoldUI;
        HandleSetGoldUI(Money.Instance.Gold);
    }

    private void HandleSetGoldUI(int value)
    {
        _changedGoldValue = value;
        StopCoroutine("SetGoldUICo");
        StartCoroutine("SetGoldUICo");

        _goldValueText.rectTransform.localScale = Vector3.one;
        _goldValueText.rectTransform.DOKill();
        _goldValueText.rectTransform.DOShakeScale(_transitionTime, 0.5f, 10);
    }

    IEnumerator SetGoldUICo()
    {
        float curTime = 0;
        while(_transitionTime > curTime)
        {
            _goldValueText.text = Mathf.FloorToInt(Mathf.Lerp(_curGoldValue, _changedGoldValue, curTime / _transitionTime)).ToString();

            curTime += Time.deltaTime;
            yield return null;
        }

        _curGoldValue = _changedGoldValue;
        _goldValueText.text = _curGoldValue.ToString();
    }
}
