using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    private Dictionary<ItemRate, Queue<ItemInfoSO>> _rateItems = new Dictionary<ItemRate, Queue<ItemInfoSO>>();

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
        
    }

    public void SetRateItems(ItemType itemType)
    {
        List<ItemInfoSO> itemInfoSOs = _itemList.ItemInfoList;

        _rateItems.Clear();
        _rateItems[ItemRate.NORMAL]     = new Queue<ItemInfoSO>();
        _rateItems[ItemRate.RARE]       = new Queue<ItemInfoSO>();
        _rateItems[ItemRate.EPIC]       = new Queue<ItemInfoSO>();
        _rateItems[ItemRate.LEGEND]     = new Queue<ItemInfoSO>();

        Dictionary<ItemRate, List<ItemInfoSO>> rateItems = new Dictionary<ItemRate, List<ItemInfoSO>>();
        rateItems[ItemRate.NORMAL]     = new List<ItemInfoSO>();
        rateItems[ItemRate.RARE]       = new List<ItemInfoSO>();
        rateItems[ItemRate.EPIC]       = new List<ItemInfoSO>();
        rateItems[ItemRate.LEGEND]     = new List<ItemInfoSO>();

        foreach (ItemInfoSO item in itemInfoSOs)
        {
            if (item.Brick.Type != itemType)
                continue;

            rateItems[item.Rate].Add(item);
        }

        // List Shuffle
        foreach (var items in rateItems)
        {
            SetRateItemQueue(items.Key, items.Value);
        }

    }

    private void SetRateItemQueue(ItemRate type, List<ItemInfoSO> list)
    {
        int shuffleCount = Mathf.Min(_itemCount, list.Count);

        for(int i = 0; i < shuffleCount; ++i)
        {
            int randomIdx = Random.Range(i, list.Count);

            _rateItems[type].Enqueue(list[randomIdx]);
            list[randomIdx] = list[i];
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
        _itemCount = randomItems.Count;
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
        for(int i = 0; i < _itemCount; ++i)
        {

            float endPercent = 100f;
            float normalProbability = Mathf.Clamp(endPercent - _legendProbability - _epicProbability - _rareProbability, 0f, 100f);

            // �ش� ����� Item�� ���� ��� �̸� üũ�ϱ�
            bool normalCheck    = RateItemQueueCheck(ItemRate.NORMAL, normalProbability, ref endPercent);
            bool rareCheck      = RateItemQueueCheck(ItemRate.RARE, _rareProbability, ref endPercent);
            bool epicCheck      = RateItemQueueCheck(ItemRate.EPIC, _epicProbability, ref endPercent);
            bool legendCheck    = RateItemQueueCheck(ItemRate.LEGEND, _legendProbability, ref endPercent);

            if (!normalCheck && !rareCheck && !epicCheck && !legendCheck)
                break;

            float value = Random.Range(0f, endPercent); // 0 ~ 100
            ItemRate rate = ItemRate.NORMAL;


            if (legendCheck && ItemPercentCheck(ItemRate.LEGEND, _legendProbability, ref value))
                rate = ItemRate.LEGEND;
            else if (epicCheck && ItemPercentCheck(ItemRate.EPIC, _epicProbability, ref value))
                rate = ItemRate.EPIC;
            else if (rareCheck && ItemPercentCheck(ItemRate.RARE, _rareProbability, ref value))
                rate = ItemRate.RARE;

            itemList.Add(_rateItems[rate].Dequeue());
        }
        
        return itemList;
    }

    private bool RateItemQueueCheck(ItemRate rate, float rateProability ,ref float endPercent)
    {
        if(_rateItems[rate].Count == 0)
        {

            endPercent -= rateProability;
            return false;

        }

        return true;
    }
    private bool ItemPercentCheck(ItemRate rate, float rateProbability, ref float value)
    {
        if (value <= rateProbability)
        {

            return true;

        }

        value -= rateProbability;
        return false;

    }

    public virtual void OnInteract()
    {
        Open();
    }
}
