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


    [HideInInspector]
    public Image[] images;
    [HideInInspector]
    public bool canOpen = true;

    bool isOn = false;

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

    PanelFade fade;

    TextMeshProUGUI _warningText;

    readonly string invenShader = "_SourceGlowDissolveFade";

    private void Start()
    {
        _invenx = _invenPanel.transform.localPosition.x;
        _inveny = _invenPanel.transform.localPosition.y;
        _infox = _invenInfoPanel.transform.localPosition.x;

        _components = _invenPanel.transform.Find("Components").transform;
        invenRenderer = _invenPanel.GetComponent<Image>();
        fade = transform.Find("FadePanel").GetComponent<PanelFade>();
        _warningText = transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
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
                if(GameManager.Instance.isPlay)
                {
                    WarningText();
                    return;
                }
                isAnimation = false;
                StartCoroutine(ShowLineRender());
                isOn = !isOn;
                if (isOn)
                    ShowInven();
                else
                    ShowUI();
            }
        }
    }

    private void ShowInven()
    {
        isOn = true;

        fade.Fade(true);
        
        var energeBar = FindObjectOfType<PlayerEnergeBar>();
        energeBar.Image.color = new Color(energeBar.Image.color.r, energeBar.Image.color.g, energeBar.Image.color.b, 0);
        
        _components.localPosition = new Vector3(0, 0, 0);
        _invenPanel.transform.DOLocalMoveY(_inveny, 0f);
        invenRenderer.enabled = true;
        float initValue = mat.GetFloat(invenShader);

        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => initValue, value => mat.SetFloat(invenShader, value), 15f, easingtime)).SetEase(ease);
        seq.Join(ScreenManager.Instance.SetEffect(0.3f, 0.65f, DG.Tweening.Ease.InQuad));

        seq.AppendCallback(() => { isAnimation = true; });

    }

    public void ShowUI()
    {
        isOn = false;

        fade.Fade(false);

        var energeBar = FindObjectOfType<PlayerEnergeBar>();
        energeBar.Image.color = new Color(energeBar.Image.color.r, energeBar.Image.color.g, energeBar.Image.color.b, 1);
        
        _components.localPosition = new Vector3(0, 1000, 0);
        ScreenManager.Instance.SetEffect(0, 0.5f, DG.Tweening.Ease.InQuart);
        float initValue = mat.GetFloat(invenShader);

        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => initValue, value => mat.SetFloat(invenShader, value), 0f, easingtime).SetEase(ease));
        seq.AppendCallback(() => { StartCoroutine(DelayCo()); });

    }

    private void WarningText()
    {
        _warningText.DOFade(1f, 0f);
        _warningText.DOFade(0f, 2f);
    }

    IEnumerator DelayCo()
    {
        _invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);
        invenRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        isAnimation = true;
    }

    IEnumerator ShowLineRender()
    {
        ConnectVisible cv = FindObjectOfType<ConnectVisible>();
        while (!isAnimation)
        {
            cv.VisibleLine();
            yield return null;
        }
    }
}
