using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneEffectSound : MonoBehaviour
{
    public static PlaySceneEffectSound Instancce;

    [SerializeField] private AudioClip _invenEat;
    [SerializeField] private AudioClip _invenDragAndDrop;
    [SerializeField] private AudioClip _btnClick;

    private void Awake()
    {
        if(Instancce == null)
        {
            Instancce = this;
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
}
