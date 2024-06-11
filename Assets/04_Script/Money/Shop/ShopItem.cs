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
    private bool _isSold = false;

    public void SetShopItem(ItemInfoSO item, AudioClip buyItemSound)
    {
        _warningTxt = FindObjectOfType<WarningTxt>();

        if(_warningTxt == null )
        {
            Debug.LogError($"{transform} : warning Text is null!");
        }

        _item = item;
        SetName(item);

        _itemImage.color = Color.white;
        _itemImage.sprite = item.Sprite;
        _buyItemClip = buyItemSound;

        _isSold = false;

        SetColor(item.Rate);
        SetPrice(item.Rate);
    }

    private void SetName(ItemInfoSO item)
    {
        InvenBrick brick = item.Brick;

        switch (brick.Type)
        {
            case ItemType.Weapon:
                SetWeaponInfo(brick);
                break;
            case ItemType.Generator:
                SetGeneratorInfo(brick);
                break;
            case ItemType.Connector:
                SetConnectorInfo(brick);
                break;
        }

    }

    private void SetWeaponInfo(InvenBrick brick)
    {
        WeaponBrick weaponBrick = brick as WeaponBrick;

        if (weaponBrick != null)
        {

            WeaponID id = weaponBrick.WeaponPrefab.id;

            string nameText = WeaponExplainManager.weaponName[id];

            _itemName.text = nameText;

        }
    }

    private void SetGeneratorInfo(InvenBrick brick)
    {
        GeneratorID id = brick.InvenObject.generatorID;

        string nameText = WeaponExplainManager.generatorName[id];

        _itemName.text = nameText;
    }

    private void SetConnectorInfo(InvenBrick brick)
    {
        _itemName.text = "¿¬°á±â";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isSold)
            return;

        if(_shop.PlayerMoney.SpendGold(_itemPrice))
        {
            if(_item.GetItem())
            {
                _itemPrice = 0;
                _itemPriceText.text = string.Empty;
                _itemName.text = "Sold Out";
                _itemName.color = Color.white;

                _itemImage.color = Color.clear;

                _isSold = true;
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
