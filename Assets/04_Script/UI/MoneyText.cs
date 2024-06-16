using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _text.text = Money.Instance.Gold.ToString();
        Money.Instance.GoldChangedEvent += (money) => _text.text = money.ToString();
    }
}
