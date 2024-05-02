using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TestGameSettingObj : MonoBehaviour, IInteractable
{
    public UnityEvent events;

    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0.0f;

        transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void OnInteract()
    {
        events?.Invoke();
    }
}
