using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int _price;

    [SerializeField]
    private Transform _interactLinkObject;
    private IInteractable _interact;

    [SerializeField]
    private FeedbackPlayer _falseFeedback;

    private void Awake()
    {
        _interact = _interactLinkObject.GetComponent<IInteractable>();
    }

    public void OnInteract()
    {
        if (Money.Instance.Gold >= _price)
        {
            Money.Instance.SpendGold(_price);
            _interact.OnInteract();
        }
        else
            _falseFeedback.Play(_price - Money.Instance.Gold);
    }
}
