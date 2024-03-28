using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneEffectSound : MonoBehaviour
{
    public static PlaySceneEffectSound Instance;

    [Header("인벤관련")]
    [SerializeField] private AudioClip _invenEat;
    [SerializeField] private AudioClip _invenDragAndDrop;
    [Header("UI")]
    [SerializeField] private AudioClip _btnClick;
    [Header("시스템 관련")]
    [SerializeField] private AudioClip _gateAppearSound;
    [SerializeField] private AudioClip _chestAppearSound;
    [SerializeField] private AudioClip _chestOpen;
    [SerializeField] private AudioClip _moneyDropSound;
    [Header("상점관련")]
    [SerializeField] private AudioClip _buySound;
    [SerializeField] private AudioClip _closeSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"{transform} : is multiply running!");
            Destroy(gameObject);
        }    
    }

    public void PlayItemEat() => SoundManager.Instance.SFXPlay("ItemEat", _invenEat);

    public void PlayDragAndDrop() => SoundManager.Instance.SFXPlay("ItemEat", _invenDragAndDrop);

    public void PlayBtnClickSound() =>
        SoundManager.Instance.SFXPlay("BtnClick",_btnClick);

    public void PlayGateSound() =>
        SoundManager.Instance.SFXPlay("GateSound", _gateAppearSound);

    public void PlayChestSound() =>
        SoundManager.Instance.SFXPlay("ChestAppear", _chestAppearSound);

    public void PlayChestOpenSound() =>
        SoundManager.Instance.SFXPlay("ChestOpen", _chestOpen);

    public void PlayMoneyDropSound() =>
        SoundManager.Instance.SFXPlay("MoneyDrop", _moneyDropSound,1f);

    public void PlayBuySound() =>
        SoundManager.Instance.SFXPlay("Buy", _buySound);

    public void PlayShopCloseSound() =>
        SoundManager.Instance.SFXPlay("ShopClose", _closeSound);
}
