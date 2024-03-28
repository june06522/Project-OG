using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // normal item probabilities are the remaining probabilities other than other item probabilities
    [Header("percent")]
    [SerializeField] private float _rareProbability;     // rare item probability
    [SerializeField] private float _epicProbability;     // epic item probability
    [SerializeField] private float _legendProbability;   // legend item probability

    [Header("Info")]
    [SerializeField]
    private ItemInfoListSO _itemList;
    [SerializeField]
    private int _dropMinGold = 10;
    [SerializeField]
    private int _dropMaxGold = 50;

    private Dictionary<ItemRate, List<ItemInfoSO>> _rateItems = new Dictionary<ItemRate, List<ItemInfoSO>>();

    [Header("Chest Info")]
    [SerializeField] private Sprite _openSprite;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _itemSpawnPos;
    [SerializeField] private ParticleSystem _goldEffect;
    private bool _isOpen = false;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_itemList == null)
        {
            Debug.LogError("ItemSOList is null");
            return;
        }

        SetRateItems(_itemList.ItemInfoList.ToArray());
    }

    private void SetRateItems(ItemInfoSO[] itemInfoSOs)
    {
        _rateItems.Clear();
        _rateItems[ItemRate.NORMAL] = new List<ItemInfoSO>();
        _rateItems[ItemRate.RARE] = new List<ItemInfoSO>();
        _rateItems[ItemRate.EPIC] = new List<ItemInfoSO>();
        _rateItems[ItemRate.LEGEND] = new List<ItemInfoSO>();

        foreach (ItemInfoSO item in itemInfoSOs)
        {
            _rateItems[item.Rate].Add(item);
        }
    }

    public void Open()
    {
        if (_isOpen)
            return;
        _collider.enabled = false;
        _isOpen = true;

        //ChestOpenSound
        //PlaySceneEffectSound.Instance.PlayChestOpenSound();
        PlaySceneEffectSound.Instance.PlayMoneyDropSound();

        _spriteRenderer.sprite = _openSprite;

        ItemInfoSO item = RandomItem();
        
        if (item.ItemObject != null) 
        {
            Item itemObject = Instantiate(item.ItemObject, transform.position, Quaternion.identity);

            itemObject.transform.DOJump(_itemSpawnPos.position, 1.5f, 1, 0.7f);
        }

        if (_goldEffect != null)
            PlayOpenEffect(item.Rate);

        Money.Instance.EarnGold(Random.Range(_dropMinGold, _dropMaxGold + 1));
    }

    private void PlayOpenEffect(ItemRate rate)
    {
        _goldEffect.Stop();

        //var main = _openEffect.main;
        //main.startColor = Color.white;
        //switch (rate)
        //{
        //    case ItemRate.RARE:
        //        main.startColor = Color.cyan;
        //        break;
        //    case ItemRate.EPIC:
        //        main.startColor = Color.magenta;
        //        break;
        //    case ItemRate.LEGEND:
        //        main.startColor = Color.yellow;
        //        break;
        //}

        _goldEffect.Play();
    }

    private ItemInfoSO RandomItem()
    {
        float percent = Random.Range(0f, 100f); // 0 ~ 100
        ItemRate rate = ItemRate.NORMAL;

        if (percent <= _legendProbability)
        {

            rate = ItemRate.LEGEND;

        }
        else if (percent <= _legendProbability + _epicProbability)
        {

            rate = ItemRate.EPIC;

        }
        else if (percent <= _legendProbability + _epicProbability + _rareProbability)
        {

            rate = ItemRate.RARE;

        }

        ItemInfoSO iteminfo = _rateItems[rate][Random.Range(0, _rateItems[rate].Count)];
        return iteminfo;
    }

    public void OnInteract()
    {
        Open();
    }
}
