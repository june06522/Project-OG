using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private Money _playerMoney;
    public Money PlayerMoney => _playerMoney;

    [Header("UI Info")]
    // value
    [SerializeField]
    private int _reRollGold = 2;
    [SerializeField]
    private int _reRollIncreaseGoldValue = 1;

    // ui object
    [SerializeField]
    private GameObject _shopUIObject;
    [SerializeField]
    private TextMeshProUGUI _playerGoldText;
    [SerializeField]
    private TextMeshProUGUI _reRollGoldText;

    [SerializeField]
    private List<ShopItem> _shopItemList = new List<ShopItem>();

    // open
    private bool _isOpen = false;

    [Header("Item Info")]
    [SerializeField]
    private ItemInfoListSO _itemInfoListSO;
    private List<ItemInfoSO> _items = new List<ItemInfoSO>();

    private void Awake()
    {
        Debug.Log("Shop.awake");
        _playerMoney = FindObjectOfType<Money>();
        if (_playerMoney == null)
            Debug.LogError("Money Object is not found");

        SetRandomItem();
        _isOpen = false;
    }

    public void OpenShop()
    {
        Debug.Log(_isOpen);
        if(_isOpen) return;

        Debug.Log("open");

        _isOpen = true;
        _playerMoney.GoldChangedEvent += UpdatePlayerGoldUI;
        _shopUIObject.SetActive(true);

        UpdatePlayerGoldUI(_playerMoney.Gold);
    }

    public void CloseShop()
    {
        if (_isOpen == false) return;

        _isOpen = false;
        _playerMoney.GoldChangedEvent -= UpdatePlayerGoldUI;
        Debug.Log("close");
        _shopUIObject.SetActive(false);
    }

    private void UpdatePlayerGoldUI(int gold)
    {
        _playerGoldText.text = $"{gold}G";
    }

    public void ReRoll()
    {
        if(_playerMoney.SpendGold(_reRollGold) == false) return;

        SetRandomItem();

        // 리롤마다 비용 증가
        _reRollGold += _reRollIncreaseGoldValue;
        _reRollGoldText.text = $"{_reRollGold}G";
    }

    private void SetRandomItem()
    {
        // 아이템 SO List 필요할 듯
        _items.Clear();
        _items = _itemInfoListSO.ItemInfoList.ToList<ItemInfoSO>();

        for(int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(i, _items.Count);

            ItemInfoSO temp = _items[randomIndex];
            _items[randomIndex] = _items[i];
            _items[i] = temp;
        }

        if(_shopItemList.Count < 4)
        {
            Debug.LogError("ShopItem size is less than 4");
            return;
        }

        for(int i = 0; i < 4; ++i)
        {
            _shopItemList[i].SetShopItem(_items[i]);
        }
    }
}
