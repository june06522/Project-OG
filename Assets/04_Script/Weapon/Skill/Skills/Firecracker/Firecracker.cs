using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Firecracker : Skill
{
    [Header("percent")]
    [SerializeField] private float _rareProbability;     // rare item probability
    [SerializeField] private float _epicProbability;     // epic item probability
    [SerializeField] private float _legendProbability;   // legend item probability

    [Space]
    [SerializeField] private ItemInfoListSO _itemList;
    [SerializeField] private TMP_Text _notice;

    private Dictionary<ItemRate, List<ItemInfoSO>> _rateItems = new Dictionary<ItemRate, List<ItemInfoSO>>();

    private void OnEnable()
    {

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

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        // 넘어온 무기가 rotateClone이면 생성 X
        if (weapon is not RotateClone)
        {
            StartCoroutine(ExcuteCo(weaponTrm, target, power));
        }
    }

    IEnumerator ExcuteCo(Transform weaponTrm, Transform target, int power)
    {
        int num = Random.Range(0, 101);
        //Debug.Log($"num:{num}");

        yield return new WaitForSeconds(1f);

        if (num < 10) // 아이템 드랍
        {

            ItemInfoSO item = RandomItem();

            if (item.ItemObject != null)
            {
                Item itemObject = Instantiate(item.ItemObject, transform.position, Quaternion.identity);

                itemObject.transform.DOJump(weaponTrm.position, 1.5f, 1, 0.7f);
            }

        }
        else if (num < 45) // 힐
        {

            var _playerHP = GameManager.Instance.player.GetComponent<PlayerHP>();

            int healHealth = Random.Range(1, power * 8 + 1);
            _playerHP.RestoreHP(healHealth);

            var text = Instantiate(_notice, weaponTrm.position, Quaternion.identity);
            text.color = Color.green;
            text.text = healHealth.ToString();
            Sequence seq = DOTween.Sequence();

            text.transform.position = weaponTrm.transform.position;
            Transform textTrm = text.transform;
            seq.Append(textTrm.DOMoveY(textTrm.position.y + 0.2f, 0.25f).SetEase(Ease.InOutElastic));
            seq.Join(textTrm.DOScale(Vector3.one * 1.8f, 0.2f).SetEase(Ease.OutBounce));
            seq.Append(textTrm.DOScale(new Vector3(0.6f, 1.75f), 0.1f).SetEase(Ease.InOutBounce));
            seq.Append(textTrm.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBounce));
            seq.AppendCallback(() =>
            {
                Destroy(text);
            });

        }
        else if (num < 95) // 돈
        {

            PlaySceneEffectSound.Instance.PlayMoneyDropSound();
            int amount = Random.Range(1, power * 20 + 1);
            Money.Instance.EarnGold(amount);

            var text = Instantiate(_notice, weaponTrm.position, Quaternion.identity);
            text.color = Color.yellow;
            text.text = amount.ToString();
            Sequence seq = DOTween.Sequence();

            text.transform.position = weaponTrm.transform.position;
            Transform textTrm = text.transform;
            seq.Append(textTrm.DOMoveY(textTrm.position.y + 0.2f, 0.25f).SetEase(Ease.InOutElastic));
            seq.Join(textTrm.DOScale(Vector3.one * 1.8f, 0.2f).SetEase(Ease.OutBounce));
            seq.Append(textTrm.DOScale(new Vector3(0.6f, 1.75f), 0.1f).SetEase(Ease.InOutBounce));
            seq.Append(textTrm.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBounce));
            seq.AppendCallback(() =>
            {
                Destroy(text);
            });

        }
        else
        {

            Debug.Log("꽝");

        }
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

}
