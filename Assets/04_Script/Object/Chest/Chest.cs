using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("ItemType")]
    [SerializeField] private bool _useOnlyOneTypeChest = false;
    [SerializeField] private ItemType _itemType;

    // normal item probabilities are the remaining probabilities other than other item probabilities
    [Header("percent")]
    [SerializeField] private float _rareProbability;     // rare item probability
    [SerializeField] private float _epicProbability;     // epic item probability
    [SerializeField] private float _legendProbability;   // legend item probability

    [Header("Info")]
    [SerializeField]
    private FeedbackPlayer _feedbackPlayer;
    [SerializeField]
    private AudioClip _openSound;
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
            if(_useOnlyOneTypeChest && item.Brick.Type != _itemType)
            {
                continue;
            }

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

        if(_spriteRenderer != null)
            _spriteRenderer.sprite = _openSprite;

        ItemInfoSO item = GetRandomItem();
        
        if (item.ItemObject != null) 
        {
            Item itemObject = Instantiate(item.ItemObject, transform.position, Quaternion.identity);

            itemObject.transform.DOJump(_itemSpawnPos.position, 1.5f, 1, 0.7f);
        }

        if (_goldEffect != null)
            PlayOpenEffect(item.Rate);
        _feedbackPlayer?.Play(0);

        SoundManager.Instance.SFXPlay("ChestOpen", _openSound, 1f);
        Money.Instance.EarnGold(Random.Range(_dropMinGold, _dropMaxGold + 1));
    }

    private void PlayOpenEffect(ItemRate rate)
    {
        if (_goldEffect == null)
            return;

        if(_goldEffect.isPlaying)
            _goldEffect.Stop();
        _goldEffect.Play();
    }

    private ItemInfoSO GetRandomItem()
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

        while(_rateItems[rate].Count <= 0)
        {
            if (rate == ItemRate.LEGEND)
                rate = ItemRate.NORMAL;
            else
                rate = rate + 1;
        }

        ItemInfoSO iteminfo = _rateItems[rate][Random.Range(0, _rateItems[rate].Count)];
        return iteminfo;
    }

    public void OnInteract()
    {
        Open();
    }
}
