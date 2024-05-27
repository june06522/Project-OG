using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameTooltip : MonoBehaviour
{

    TextMeshProUGUI _nameText;
    TextMeshProUGUI _rateText;
    TextMeshProUGUI _explainText;

    private void Awake()
    {

        _nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _rateText = transform.Find("Rate").GetComponent<TextMeshProUGUI>();
        _explainText = transform.Find("Explain").GetComponent<TextMeshProUGUI>();

    }

    public void SetInfo(Item item)
    {

        InvenBrick brick = item.Brick;

        // check
        if (brick == null || brick.Type == ItemType.Connector)
            return;

        GeneratorID id = brick.InvenObject.generatorID;
        ItemRate rate = brick.ItemRate;

        string nameText = WeaponExplainManager.generatorName[id];
        string rateText = WeaponExplainManager.itemRate[rate];
        string explainText = WeaponExplainManager.generatorExplain[id];

        _nameText.text = nameText;
        _rateText.text = rateText;
        _explainText.text = explainText;

    }

}
