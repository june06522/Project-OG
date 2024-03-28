using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealObject : MonoBehaviour, IInteractable
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    [Header("DefaultSprite")]
    [SerializeField]
    private Material _spriteDefaultMat;
    private PlayerHP _playerHP;

    [Header("Info")]
    [SerializeField]
    private int _restoreMinHealth = 10;
    [SerializeField]
    private int _restoreMaxHealth = 100;

    [Header("Object Info")]
    [SerializeField]
    private GameObject _light;
    [SerializeField]
    private TMP_Text _healText;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void OnInteract()
    {
        _playerHP = GameManager.Instance.player.GetComponent<PlayerHP>();

        int healHealth = Random.Range(_restoreMinHealth, _restoreMaxHealth + 1);
        _playerHP.RestoreHP(healHealth);

        _healText.text = healHealth.ToString();
        Sequence seq = DOTween.Sequence();

        Transform healTextTrm = _healText.transform;
        seq.Append(healTextTrm.DOMoveY(healTextTrm.position.y + 0.2f, 0.25f).SetEase(Ease.InOutElastic));
        seq.Join(healTextTrm.DOScale(Vector3.one * 1.8f, 0.2f).SetEase(Ease.OutBounce));
        seq.Append(healTextTrm.DOScale(new Vector3(0.6f, 1.75f), 0.1f).SetEase(Ease.InOutBounce));
        seq.Append(healTextTrm.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBounce));


        _spriteRenderer.material = _spriteDefaultMat;
        _collider.enabled = false;
        _light.SetActive(false);
    }
}
