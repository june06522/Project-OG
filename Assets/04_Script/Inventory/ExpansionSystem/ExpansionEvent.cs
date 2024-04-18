using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionEvent : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem _goldEffect;
    private bool _isOpen = false;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Open()
    {
        if (_isOpen)
            return;
        _collider.enabled = false;
        _isOpen = true;

        ExpansionManager.Instance.AddSlotcnt(3);

        if (_goldEffect != null)
            _goldEffect.Play();
    }

    public void OnInteract()
    {
        Open();
    }
}