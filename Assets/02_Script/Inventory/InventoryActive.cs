using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    [HideInInspector]
    public Image[] images;

    public bool canOpen = true;
    bool isOn = false;

    [SerializeField] GameObject _playerUI;
    [SerializeField] GameObject _invenPanel;
    [SerializeField] GameObject _invenInfoPanel;
    public bool IsOn => isOn;

    private float _uix;
    private float _invenx;
    private float _inveny;
    private float _infox;

    private int moveXVal = 650;
    private int moveYVal = 2000;

    private float time = 0.7f;

    private void Start()
    {
        _uix = _playerUI.transform.position.x;
        _invenx = _invenPanel.transform.localPosition.x;
        _inveny = _invenPanel.transform.localPosition.y;
        _infox = _invenInfoPanel.transform.localPosition.x;

        _invenInfoPanel.transform.DOLocalMoveX(_invenx + moveXVal, 0f);
        _invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);
        _invenInfoPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);
    }

    private void Update()
    {
        if (canOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && isOn))
            {
                isOn = !isOn;
                if (isOn)
                    ShowInven();
                else
                    ShowUI();
            }
        }

        //images = GetComponentsInChildren<Image>();
        //foreach (var image in images)
        //{
        //    if(image.GetComponent<InvenBrick>() != null)
        //        image.enabled = isOn;
        //}
    }

    private void ShowInven()
    {
        DOTween.KillAll();

        Sequence seq = DOTween.Sequence();
        seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny, time));
        seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny, time));
        seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_infox, time));
        seq.Join(_playerUI.transform.DOMoveX(_uix - moveXVal, time));

    }

    private void ShowUI()
    {
        DOTween.KillAll();

        _playerUI.transform.DOMoveX(_uix, time);
        Sequence seq = DOTween.Sequence();
        seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_invenx + moveXVal, time));
        seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, time));
        seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny + moveYVal, time));
    }
}
