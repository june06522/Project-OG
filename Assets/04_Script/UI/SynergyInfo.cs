using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SynergyInfo : MonoBehaviour
{
    [SerializeField] SynergyCard _synergyCardPrefab;
    [SerializeField] List<SynergyCardSO> _synergyCardSOs;
    [SerializeField] SynergyLevelInfo _synergyLevelInfoPrefab;
    public SynergyLevelInfo GetSynergeLevelInfoPrefab => _synergyLevelInfoPrefab;
    public Transform ContentTrm;
    private ScrollRect _scrollView;

    private SynergyManager _synergyManager;
    private List<SynergyCard> _synergyCards;

    [SerializeField] TextMeshProUGUI _triggerInfoText;

    SynergyCard _focusSynergyCard;

    private void Awake()
    {
        _synergyManager = SynergyManager.Instance;
        _scrollView = ContentTrm.parent.parent.GetComponent<ScrollRect>();
        FindObjectOfType<WeaponInventory>().OnItemUpdated += ItemUpdate;
    }

    private void Start()
    {
        MakeSynergyCard();
    }

    private void OnEnable()
    {
        SetSynergyInfo();
    }

    private void MakeSynergyCard()
    {

        _synergyCards = new List<SynergyCard>();
        _synergyCardSOs.ForEach((synergySO) =>
        {
          
            SynergyCard synergyCard = Instantiate(_synergyCardPrefab, ContentTrm);
            synergyCard.Init(this, synergySO);

            _synergyCards.Add(synergyCard);

        });

        _focusSynergyCard = null;
    }

    private void SetSynergyInfo()
    {

        if (_synergyCards == null) return;

        _synergyCards.ForEach((synergyCard) =>
        {

            TriggerID id = synergyCard.GetID;
            
            // 현재 시너지 레벨.
            int level = SynergyManager.Instance.GetSynergyLevel(id);
            float percent = _synergyManager.GetStatFactor(id) * 100f;

            synergyCard.Setting(level, percent);
        
        });

        SetCurrentSynergyCard(null);
    }

    public void SynergyInfoUpdate(TriggerID id)
    {

        SynergyCard synergyCard =
            _synergyCards.Find((synergy) => synergy.GetID == id);

        if(synergyCard != null)
        {
            int level = SynergyManager.Instance.GetSynergyLevel(id);
            float percent = _synergyManager.GetStatFactor(id) * 100f;

            synergyCard.UpdateInfo(level, percent); 
        }

    }

    public void ItemUpdate(TriggerID id)
    {
        SynergyCard synergyCard = _synergyCards.Find((card) => card.GetID == id);
        if (synergyCard != null)
            synergyCard.UpdateItem();
    }

    public void On()
    {
        //ContentTrm.gameObject.SetActive(true);
        _synergyCards.ForEach((card) =>
        {
            card.On();
        });
        _scrollView.vertical = true;
    }

    public void Off()
    {
        //ContentTrm.gameObject.SetActive(false);
        _synergyCards.ForEach((card) =>
        {
            card.Off();
        });
        _scrollView.vertical = false;
        SetCurrentSynergyCard(null);
    }

    public void SetTriggerText(in TriggerID triggerID)
    {
        _triggerInfoText.text = triggerID.ToString();
    }

    public void SetCurrentSynergyCard(in SynergyCard synergyCard)
    {
        if(synergyCard != _focusSynergyCard)
        {
            _focusSynergyCard?.FocusOff();
            _focusSynergyCard = synergyCard;  
        }
    }
}
