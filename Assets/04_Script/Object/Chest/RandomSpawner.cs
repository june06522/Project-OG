using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RandomSpawner : MonoBehaviour, IInteractable
{

    [SerializeField] Sprite _openSprite;
    [SerializeField] Material _defaultmMaterial;
    [SerializeField] List<GameObject> weapons = new();
    [SerializeField] Collider2D _collider;

    SpriteRenderer _spriteRenderer;
    bool active = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnInteract()
    {

        Spawn();

    }

    private void Spawn()
    {

        if (active == false) return;

        _collider.enabled = false;
        active = false;
        var idx = Random.Range(0, weapons.Count);
        var obj = Instantiate(weapons[idx], transform.position, Quaternion.identity);
        obj.transform.DOJump(transform.position + Vector3.up, 1.5f, 1, 0.7f);

        _spriteRenderer.material = _defaultmMaterial;
        _spriteRenderer.sprite = _openSprite;

    }

}
