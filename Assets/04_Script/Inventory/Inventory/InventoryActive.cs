using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    //Objects
    [SerializeField] GameObject _invenPanel;
    [SerializeField] GameObject _synergyPanel;
    [SerializeField] Transform  _components;

    [SerializeField]
    private float   _easingtime = 0.4f;

    [HideInInspector]
    public bool     CanOpen = true;
    [HideInInspector]
    public bool     TutoCanOpen = true;

    public bool     IsOn = false;
    public bool     IsDrag = false;

    private bool    _isEndAnimation = true;

    private int     _moveYValue = 2000;
    private float   _invenY;
    private float   _time = 0.4f;

    Image               _invenRenderer;
    TextMeshProUGUI     _warningText;
    TextMeshProUGUI     _warningTextInven;

    ConnectVisible      _connectVisible;
    TooltipDissolve     _tooltipDissolve;
    PanelFade           _fadePanel;
    SynergyInfo         _synergyInfo;

    Sequence            _seq;

    //Actions
    public event Action InventoryOnEvent;
    public event Action InventoryOffEvent;

    private void Awake()
    {
        InitValue();
        InitComponents();
        InitActions();
    }

    private void Start()
    {
        if (CameraMovementChecker.Instance == null)
            Debug.LogError($"CameraMovementChecker is null! : 아무데다가 이 스크립트 넣으셈");
    }

    private void InitValue()
    {
        TutoCanOpen = true;
        _invenY = _invenPanel.transform.localPosition.y;
    }

    private void InitComponents()
    {
        _invenRenderer = _invenPanel.GetComponent<Image>();
        _fadePanel = transform.Find("FadePanel").GetComponent<PanelFade>();
        _warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        _warningTextInven = transform.Find("WarningTextInven").GetComponent<TextMeshProUGUI>();
        _tooltipDissolve = transform.Find("Panel/Tooltip").GetComponent<TooltipDissolve>();
        _synergyInfo = _synergyPanel.GetComponent<SynergyInfo>();
        _connectVisible = FindObjectOfType<ConnectVisible>();
    }

    private void InitActions()
    {
        InventoryOnEvent = ShowInven;
        InventoryOffEvent = ShowUI;
    }

    private void Update()
    {
        if (CanOpen && TutoCanOpen)
        {
            bool inventoryOpen =
                (KeyManager.Instance == null && Input.GetKeyDown(KeyCode.Tab)) ||
                (KeyManager.Instance != null && Input.GetKeyDown(KeyManager.Instance.inven));
                
            if (inventoryOpen && _isEndAnimation)
            {
                if (GameManager.Instance.isPlay)
                {
                    WarningText();
                    return;
                }

                _isEndAnimation = false;

                IsOn = !IsOn;
                if (IsOn)
                {
                    InventoryOnEvent?.Invoke();
                }
                else
                {
                    InventoryOffEvent?.Invoke();
                }
            }
        }
    }

    void ShowInven()
    {

        CameraMovementChecker.Instance.Off();
        _fadePanel.Fade(true);

        _components.localPosition = new Vector3(0, 0, 0);
        _seq = DOTween.Sequence();
        _seq.Append(_invenPanel.transform.DOLocalMoveY(_invenY, 0f));
        _seq.AppendCallback(() =>
        {
            _connectVisible.VisibleLineAllChange();
        });

        _invenRenderer.enabled = true;

        _seq.Append(_tooltipDissolve.InvenOn(true));
        _seq.Join(ScreenManager.Instance.SetEffect(0.11f, 0.65f, DG.Tweening.Ease.InQuad));
        _seq.Join(_tooltipDissolve.BrickOn(true));
        _seq.Insert(_easingtime - 0.05f, _tooltipDissolve.On()).InsertCallback(_easingtime -0.05f, () =>
            _synergyInfo.On());
        _seq.OnComplete(() => 
        {
            _isEndAnimation = true;
        });   
    
    }

    void ShowUI()
    {

        CameraMovementChecker.Instance.On();
        _fadePanel.Fade(false);

        EventTriggerManager.Instance.ResetTrigger();

        _components.localPosition = new Vector3(0, 1000, 0);
        _connectVisible.ClearLineRender();
        
        _seq = DOTween.Sequence();
        _seq.Append(_tooltipDissolve.Off());
        _seq.Join(_tooltipDissolve.BrickOn(false));
        _seq.Join(ScreenManager.Instance.SetEffect(0, 0.5f, DG.Tweening.Ease.InQuart));
        _seq.Insert(0.35f, _tooltipDissolve.InvenOn(false));
        _seq.OnComplete(() => { StartCoroutine(DelayCo()); });

        _synergyInfo.Off();

    }

    private void WarningText()
    {
        CameraManager.Instance.CameraShake(3f, 0.05f);
        _warningText.DOFade(1f, 0f);
        _warningText.DOFade(0f, 2f);
    }

    public void WarningTextInvenFull()
    {
        CameraManager.Instance.CameraShake(3f, 0.05f);
        _warningTextInven.DOFade(1f, 0f);
        _warningTextInven.DOFade(0f, 2f);
    }

    IEnumerator DelayCo()
    {
        _invenPanel.transform.DOLocalMoveY(_invenY + _moveYValue, 0f);
        _invenRenderer.enabled = false;
        yield return new WaitForSeconds(_time);
        _isEndAnimation = true;
    }
}
