using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private Money _playerMoney;
    public Money PlayerMoney => _playerMoney;

    [Header("UI Info")]
    // canvas
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private ShopItemInfo _shopItemInfo;
    public ShopItemInfo ShopItemInfoUI => _shopItemInfo;

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

    private WarningTxt _warningTxt;

    // open
    private bool _isOpen = false;
    public bool isOpen => _isOpen;
    [Header("Item Info")]
    [SerializeField]
    private ItemInfoListSO _itemInfoListSO;
    private List<ItemInfoSO> _items = new List<ItemInfoSO>();
    private InventoryActive inven;

    [Header("Sound Info")]
    [SerializeField] private AudioClip _reRollClip;
    [SerializeField] private AudioClip _buyItemClip;

    // Random Value
    private float _normalProbability;   // 50f
    private float _rareProbability;
    private float _epicProbability;
    private float _legendaryProbability;

    private Dictionary<ItemRate, Queue<ItemInfoSO>> _itemRateQ = new Dictionary<ItemRate, Queue<ItemInfoSO>>();


    private void Awake()
    {
        _playerMoney = FindObjectOfType<Money>();
        if (_playerMoney == null)
            Debug.LogError("Money Object is not found");

        SetRandomItem();
        _isOpen = false;
        inven = FindObjectOfType<InventoryActive>();
        _warningTxt = FindObjectOfType<WarningTxt>();

        _canvas.worldCamera = CameraManager.Instance.UICam;

        GameManager manager = GameManager.Instance;
        _legendaryProbability   = manager.GetRateProbability(ItemRate.LEGEND);
        _epicProbability        = manager.GetRateProbability(ItemRate.EPIC);
        _rareProbability        = manager.GetRateProbability(ItemRate.RARE);
        _normalProbability      = manager.GetRateProbability(ItemRate.NORMAL);

    }

    public void OpenShop()
    {
        if(_isOpen || inven.IsOn) return;

        GameManager.Instance.isShopOpen = true;
        inven.CanOpen = false;
        _isOpen = true;
        _playerMoney.GoldChangedEvent += UpdatePlayerGoldUI;
        _shopUIObject.SetActive(true);

        UpdatePlayerGoldUI(_playerMoney.Gold);
    }

    public void CloseShop()
    {
        if (_isOpen == false) return;

        GameManager.Instance.isShopOpen = false;
        inven.CanOpen = true;
        _isOpen = false;
        _playerMoney.GoldChangedEvent -= UpdatePlayerGoldUI;
        _shopUIObject.SetActive(false);
    }

    private void UpdatePlayerGoldUI(int gold)
    {
        _playerGoldText.text = $"{gold}G";
    }

    Sequence _rerollSeq = null;
    public void ReRoll()
    {
        if (_playerMoney.SpendGold(_reRollGold) == false)
        {
            _warningTxt.LackMoney();
            return;
        }
        SetRandomItem();
        SoundManager.Instance.SFXPlay("ReRoll", _reRollClip, 0.9f);

        if(_rerollSeq != null)
        {
            _rerollSeq.Kill();
        }

        _rerollSeq = DOTween.Sequence();
        for(int i = 0; i < _shopItemList.Count; ++i)
        {
            _shopItemList[i].transform.localScale = Vector3.one * 0.5f;
        }

        _rerollSeq.Append(_shopItemList[0].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce));
        for(int i = 1; i < _shopItemList.Count; ++i)
        {
            _rerollSeq.Join(_shopItemList[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce));
        }
        _rerollSeq.OnComplete(() => { _rerollSeq = null; });


        _reRollGold += _reRollIncreaseGoldValue;
        _reRollGoldText.text = $"{_reRollGold}G";
    }

    private void SetRandomItem()
    {
        _items.Clear();
        _items = _itemInfoListSO.ItemInfoList.ToList<ItemInfoSO>();

        _itemRateQ.Clear();
        _itemRateQ = new Dictionary<ItemRate, Queue<ItemInfoSO>>
        {
            { ItemRate.NORMAL,  new Queue<ItemInfoSO>() },
            { ItemRate.RARE,    new Queue<ItemInfoSO>() },
            { ItemRate.EPIC,    new Queue<ItemInfoSO>() },
            { ItemRate.LEGEND,  new Queue<ItemInfoSO>() }
        };

        // Q Setting
        for(int i = 0; i < _items.Count; ++i)
        {
            int randomIndex = Random.Range(i, _items.Count);

            ItemInfoSO temp = _items[randomIndex];
            _itemRateQ[temp.Rate].Enqueue(temp);

            _items[randomIndex] = _items[i];
        }

        for(int i = 0; i < 4; i++)
        {
            //Rate Check
            float per = Random.Range(0f, 100f);
            float[] ratePer = { _legendaryProbability, _epicProbability, _rareProbability };
            ItemRate[] rateArr = { ItemRate.LEGEND, ItemRate.EPIC, ItemRate.RARE };
            ItemRate rate = ItemRate.NORMAL;

            for(int j = 0; j < 3; ++j)
            {
                if (per <= ratePer[j])
                {

                    rate = rateArr[j];
                    break;

                }

                per -= ratePer[j];
            }

            ItemInfoSO itemInfo = _itemRateQ[rate].Dequeue();
            _shopItemList[i].SetShopItem(itemInfo, _buyItemClip);
        }

        if(_shopItemList.Count < 4)
        {
            Debug.LogError("ShopItem size is less than 4");
            return;
        }

    }

    private void OnDisable()
    {
        _shopItemInfo.SetEnableUI(false);
    }

}
