using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // normal item probabilities are the remaining probabilities other than other item probabilities
    [Header("percent")]
    [SerializeField] private float _rareProbability;     // rare item probability
    [SerializeField] private float _epicProbability;     // epic item probability
    [SerializeField] private float _legendProbability;   // legend item probability

    [Header("Info")]
    [SerializeField]
    private ItemInfoListSO _itemList;
    public Item test;

    private Dictionary<ItemRate, List<ItemInfoSO>> _rateItems = new Dictionary<ItemRate, List<ItemInfoSO>>();

    [Header("Chest Info")]
    [SerializeField] private Sprite _openSprite;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _itemSpawnPos;
    [SerializeField] private ParticleSystem _openEffect;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if(_itemList == null)
        {
            Debug.LogError("ItemSOList is null");
            return;
        }

        SetRateItems(_itemList.ItemInfoList.ToArray());
    }

    // test code
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Open();
    }

    private void SetRateItems(ItemInfoSO[] itemInfoSOs)
    {
        _rateItems.Clear();
        _rateItems[ItemRate.NORMAL] = new List<ItemInfoSO>();
        _rateItems[ItemRate.RARE] = new List<ItemInfoSO>();
        _rateItems[ItemRate.EPIC] = new List<ItemInfoSO>();
        _rateItems[ItemRate.LEGEND] = new List<ItemInfoSO>();

        foreach(ItemInfoSO item in itemInfoSOs)
        {
            _rateItems[item.Rate].Add(item);
        }
    }

    private void Open()
    {
        // 상자 스프라이트 변경
        _spriteRenderer.sprite = _openSprite;

        ItemInfoSO item = RandomItem();
        // 아이템 소환인데.. 아이템 프리팹이 없는데?
        if(item.ItemObject != null || true) // 여기 true 빼야되고 밑에 테스트 코드 지워야함
        {
            //Item itemObject = Instantiate(item.ItemObject, transform.position, Quaternion.identity);
            Item itemObject = Instantiate(test, _itemSpawnPos.position, Quaternion.identity);

            // 애니메이션 연출
            itemObject.transform.DOJump(_itemSpawnPos.position, 1.5f, 1, 0.7f);
        }

        // 이펙트
        PlayOpenEffect(item.Rate);
    }

    private void PlayOpenEffect(ItemRate rate)
    {
        _openEffect.Stop();

        // 등급에 따른 이펙트 색 변화
        var main = _openEffect.main;
        main.startColor = Color.white;
        switch (rate)
        {
            case ItemRate.RARE:
                main.startColor = Color.cyan;
                break;
            case ItemRate.EPIC:
                main.startColor = Color.magenta;
                break;
            case ItemRate.LEGEND:
                main.startColor = Color.yellow;
                break;
        }

        _openEffect.Play();
    }

    private ItemInfoSO RandomItem()
    {
        // 등급 계산
        float percent = Random.Range(0f, 100f); // 0 ~ 100
        ItemRate rate = ItemRate.NORMAL;

        if(percent <= _legendProbability)
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
}
