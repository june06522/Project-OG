using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Shop _shop;

    [Header("Info")]
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemName;
    [SerializeField]
    private TextMeshProUGUI _itemPriceText;

    private WarningTxt _warningTxt;

    private int _itemPrice = 1;
    ItemInfoSO _item;

    public void SetShopItem(ItemInfoSO item)
    {
        _warningTxt = FindObjectOfType<WarningTxt>();

        if(_warningTxt == null )
        {
            Debug.LogError($"{transform} : warning Text is null!");
        }

        _item = item;

        _itemImage.color = Color.white;
        _itemImage.sprite = item.Sprite;
        _itemName.text = item.ItemName;
        SetColor(item.Rate);
        SetPrice(item.Rate);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_shop.PlayerMoney.SpendGold(_itemPrice))
        {
            if(_item.GetItem())
            {
                _itemPrice = 0;
                _itemPriceText.text = string.Empty;
                _itemName.text = "Solved";
                _itemName.color = Color.white;

                _itemImage.color = Color.clear;

            }
            else
            {
                _warningTxt.FullInven();
                _shop.PlayerMoney.EarnGold(_itemPrice);
            }
        }
        else
        {
            _warningTxt.LackMoney();
        }
    }

    private void SetColor(ItemRate rate)
    {
        switch (rate)
        {
            case ItemRate.NORMAL:
                _itemName.color = Color.white;
                break;
            case ItemRate.RARE:
                _itemName.color = Color.cyan;
                break;
            case ItemRate.EPIC:
                _itemName.color = Color.magenta;
                break;
            case ItemRate.LEGEND:
                _itemName.color = Color.yellow;
                break;
        }
    }

    private void SetPrice(ItemRate rate)
    {
        switch (rate)
        {
            case ItemRate.NORMAL:
                _itemPrice = Random.Range(2, 15 + 1);
                break;
            case ItemRate.RARE:
                _itemPrice = Random.Range(9, 20 + 1);
                break;
            case ItemRate.EPIC:
                _itemPrice = Random.Range(12, 25 + 1);
                break;
            case ItemRate.LEGEND:
                _itemPrice = Random.Range(22, 30 + 1);
                break;
        }

        _itemPriceText.text = $"{_itemPrice}G";
    }
}
