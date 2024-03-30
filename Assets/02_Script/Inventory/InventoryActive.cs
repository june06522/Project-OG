using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    [HideInInspector]
    public Image[] images;
    [HideInInspector]
    public bool canOpen = true;
    [HideInInspector]
    public bool isPlaying = false;

    bool isOn = false;

    [SerializeField] GameObject _playerUI;
    [SerializeField] GameObject _invenPanel;
    [SerializeField] GameObject _invenInfoPanel;
    Sequence seq;

    public bool IsOn => isOn;

    bool isAnimation = true;

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
        if (canOpen && !isPlaying)
        {
            if (((KeyManager.Instance == null && Input.GetKeyDown(KeyCode.Tab)) ||
                (KeyManager.Instance != null && Input.GetKeyDown(KeyManager.Instance.inven)) || 
                (Input.GetKeyDown(KeyCode.Escape) && isOn)) && isAnimation)
            {
                isAnimation = false;
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
        //DOTween.Kill(seq);

        seq = DOTween.Sequence();
        seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny, time));
        seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny, time));
        seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_infox, time));
        seq.Join(_playerUI.transform.DOMoveX(_uix - moveXVal, time));
        seq.AppendCallback(() => { isAnimation = true; });
    }

    private void ShowUI()
    {
        //DOTween.Kill(seq);

        _playerUI.transform.DOMoveX(_uix, time);
        seq = DOTween.Sequence();
        seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_invenx + moveXVal, time));
        seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, time));
        seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny + moveYVal, time));
        seq.AppendCallback(() => { isAnimation = true; });

    }
}
