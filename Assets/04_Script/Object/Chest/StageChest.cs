using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class StageChest : MonoBehaviour, IInteractable
{
    // normal item probabilities are the remaining probabilities other than other item probabilities
    [Header("percent")]
    [SerializeField] private float _rareProbability;     // rare item probability
    [SerializeField] private float _epicProbability;     // epic item probability
    [SerializeField] private float _legendProbability;   // legend item probability

    [Header("Info")]
    [SerializeField]
    private FeedbackPlayer _feedbackPlayer;
    [SerializeField]
    private FeedbackPlayer _choiceItemFeedbackPlayer;
    [SerializeField]
    private ItemInfoListSO _itemList;
    [SerializeField]
    private int _itemCount = 3;

    [SerializeField]
    private int _dropMinGold = 10;
    [SerializeField]
    private int _dropMaxGold = 50;

    private Dictionary<ItemRate, List<ItemInfoSO>> _rateItems = new Dictionary<ItemRate, List<ItemInfoSO>>();

    [Header("Chest Info")]
    [SerializeField] private Sprite _openSprite;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _itemSpawnTrm;
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

        // List Shuffle
        foreach (var items in _rateItems)
        {
            ShuffleList(items.Value);
        }

    }

    private void ShuffleList(List<ItemInfoSO> list)
    {
        int shuffleCount = Mathf.Min(_itemCount, list.Count);

        for(int i = 0; i < shuffleCount; ++i)
        {
            int randomIdx = Random.Range(i, list.Count);

            ItemInfoSO temp = list[randomIdx];
            list[randomIdx] = list[i];
            list[i] = temp;

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

        List<ItemInfoSO> randomItems = GetRandomItems();
        for(int i = 0; i < _itemCount; ++i)
        {
            float angle = (360 / _itemCount) * i * Mathf.Deg2Rad;
            float dist = 3f;

            Item spawnItem = Instantiate(randomItems[i].ItemObject, _itemSpawnTrm);
            spawnItem.OnInteractItem += HandleChoiceItem;

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * dist;
            spawnItem.transform.position = _itemSpawnTrm.position + offset;
        }

        _itemSpawnTrm.gameObject.SetActive(true);
        
        Sequence seq = DOTween.Sequence();
        seq.Append(_itemSpawnTrm.DOScale(Vector3.zero, 0.01f));
        seq.Append(_itemSpawnTrm.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic));

        if (_goldEffect != null)
            PlayOpenEffect();

        _feedbackPlayer?.Play(0);
        Money.Instance.EarnGold(Random.Range(_dropMinGold, _dropMaxGold + 1));
    }

    private void HandleChoiceItem(Transform interectObjectTrm)
    {
        _choiceItemFeedbackPlayer.transform.position = interectObjectTrm.position;
        _choiceItemFeedbackPlayer.Play(0);
        _itemSpawnTrm.gameObject.SetActive(false);
    }

    private void PlayOpenEffect()
    {
        if (_goldEffect == null)
            return;

        if(_goldEffect.isPlaying)
            _goldEffect.Stop();
        _goldEffect.Play();
    }

    private List<ItemInfoSO> GetRandomItems()
    {
        List<ItemInfoSO> itemList = new List<ItemInfoSO>();

        // Set ItemRate
        float percent = Random.Range(0f, 100f); // 0 ~ 100
        ItemRate rate = ItemRate.NORMAL;

        int index = 0;

        if (percent <= _legendProbability)
            rate = ItemRate.LEGEND;
        else if (percent <= _legendProbability + _epicProbability)
            rate = ItemRate.EPIC;
        else if (percent <= _legendProbability + _epicProbability + _rareProbability)
            rate = ItemRate.RARE;

        while (itemList.Count < _itemCount)
        {
            while (_rateItems[rate].Count <= index)
            {
                if (rate == ItemRate.LEGEND)
                    rate = ItemRate.NORMAL;
                else
                    rate = rate + 1;

                index = 0;
            }

            itemList.Add(_rateItems[rate][index]);
            index++;
        }
        
        return itemList;
    }

    public void OnInteract()
    {
        Open();
    }
}
