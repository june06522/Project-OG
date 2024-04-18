using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningTxt : MonoBehaviour
{
    TextMeshProUGUI tmpro;

    private void Awake()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
    }

    public void LackMoney()
    {
        Setting();
        tmpro.text = $"골드가 부족합니다.";
        Twin();
    }

    public void FullInven()
    {
        Setting();
        tmpro.text = $"인벤토리가 꽉 찼습니다.";
        Twin();

    }

    private void Setting()
    {
        tmpro.DOKill();
        tmpro.alpha = 0f;
    }

    private void Twin()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(tmpro.DOFade(1f, 0.5f));
        seq.Append(tmpro.DOFade(0f, 0.3f));
    }

}