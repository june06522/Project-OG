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

    private AudioClip _buyItemClip;

    public void SetShopItem(ItemInfoSO item, AudioClip buyItemSound)
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
        _buyItemClip = buyItemSound;

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

                SoundManager.Instance.SFXPlay("Buy", _buyItemClip, 0.6f);

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
                _itemPrice = Random.Range(6, 20 + 1);
                break;
            case ItemRate.RARE:
                _itemPrice = Random.Range(11, 40 + 1);
                break;
            case ItemRate.EPIC:
                _itemPrice = Random.Range(24, 70 + 1);
                break;
            case ItemRate.LEGEND:
                _itemPrice = Random.Range(60, 90 + 1);
                break;
        }

        _itemPriceText.text = $"{_itemPrice}G";
    }
}
