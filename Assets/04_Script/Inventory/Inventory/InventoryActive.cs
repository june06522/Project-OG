using Cinemachine;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    [SerializeField]
    private float power = 5f;
    [SerializeField]
    private float easingtime = 0.5f;
    [SerializeField]
    Ease ease = Ease.Linear;
    [SerializeField]
    AnimationCurve animCurve;

    private ConnectVisible cv;

    [HideInInspector]
    public Image[] images;
    [HideInInspector]
    public bool canOpen = true;

    bool isOn = false;
    public bool isDrag = false;

    //[SerializeField] GameObject _playerUI;
    [SerializeField] GameObject _invenPanel;
    [SerializeField] GameObject _invenInfoPanel;
    [SerializeField] Transform _components;

    Sequence seq;

    public bool IsOn => isOn;

    bool isAnimation = true;

    private float _uix;
    private float _invenx;
    private float _inveny;
    private float _infox;

    private int moveXVal = 650;
    private int moveYVal = 2000;

    private float time = 0.4f;

    Image invenRenderer;
    Material mat;

    Vector3 playerPos;

    PanelFade fade;

    TextMeshProUGUI _warningText;
    TextMeshProUGUI _warningTextInven;
    CinemachineVirtualCamera _cmVCam;

    TooltipDissolve _tooltipDissolve;

    readonly string invenShader = "_SourceGlowDissolveFade";

    private void Start()
    {
        if (CameraMovementChecker.Instance == null)
            Debug.LogError($"CameraMovementChecker is null! : 아무데다가 이 스크립트 넣으셈");

        _invenx = _invenPanel.transform.localPosition.x;
        _inveny = _invenPanel.transform.localPosition.y;
        _infox = _invenInfoPanel.transform.localPosition.x;

        //_components = _invenPanel.transform.Find("Components").transform;
        invenRenderer = _invenPanel.GetComponent<Image>();
        fade = transform.Find("FadePanel").GetComponent<PanelFade>();
        _warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        _warningTextInven = transform.Find("WarningTextInven").GetComponent<TextMeshProUGUI>();
        _tooltipDissolve = transform.Find("Panel/Tooltip").GetComponent<TooltipDissolve>();
        cv = FindObjectOfType<ConnectVisible>();

        mat = invenRenderer.material;

        mat.SetFloat(invenShader, 0f);
    }

    private void Update()
    {
        if (canOpen)
        {
            if (((KeyManager.Instance == null && Input.GetKeyDown(KeyCode.Tab)) ||
                (KeyManager.Instance != null && Input.GetKeyDown(KeyManager.Instance.inven)) ||
                (Input.GetKeyDown(KeyCode.Escape) && isOn)) && isAnimation)
            {
                if (GameManager.Instance.isPlay)
                {
                    WarningText();
                    return;
                }
                isAnimation = false;
                isOn = !isOn;
                if (isOn)
                    StartCoroutine(ShowInven());
                else
                    ShowUI();
            }
        }
    }

    IEnumerator ShowInven()
    {
        isOn = true;
        CameraMovementChecker.Instance.Off();
        fade.Fade(true);
        yield return null;

        _components.localPosition = new Vector3(0, 0, 0);
        seq = DOTween.Sequence();
        seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny, 0f));
        seq.AppendCallback(() =>
        {
            cv.VisibleLineAllChange();
        });

        invenRenderer.enabled = true;
        float initValue = mat.GetFloat(invenShader);

        seq.Append(DOTween.To(() => initValue, value => mat.SetFloat(invenShader, value), 20.0f, easingtime)).SetEase(ease);
        seq.Join(ScreenManager.Instance.SetEffect(0.11f, 0.65f, DG.Tweening.Ease.InQuad));
        seq.Insert(easingtime - 0.05f, _tooltipDissolve.On());
        seq.OnComplete(() => { isAnimation = true; });

    }

    public void ShowUI()
    {
        isOn = false;

        CameraMovementChecker.Instance.On();
        fade.Fade(false);

        //var energeBar = FindObjectOfType<PlayerEnergeBar>();
        //energeBar.Image.color = new Color(energeBar.Image.color.r, energeBar.Image.color.g, energeBar.Image.color.b, 1);

        EventTriggerManager.Instance.ResetTrigger();

        _components.localPosition = new Vector3(0, 1000, 0);
        cv.ClearLineRender();
        ScreenManager.Instance.SetEffect(0, 0.5f, DG.Tweening.Ease.InQuart);
        float initValue = mat.GetFloat(invenShader);

        seq = DOTween.Sequence();
        seq.Append(_tooltipDissolve.Off());
        seq.Append(DOTween.To(() => initValue, value => mat.SetFloat(invenShader, value), 0f, easingtime).SetEase(ease));
        seq.AppendCallback(() => { StartCoroutine(DelayCo()); });

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
        _invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);
        invenRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        isAnimation = true;
    }
}
