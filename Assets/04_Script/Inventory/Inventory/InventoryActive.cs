using DG.Tweening;
using System.Collections;
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

    [SerializeField] GameObject _playerUI;
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

    readonly string invenShader = "_SourceGlowDissolveFade";

    private void Start()
    {
        _uix = _playerUI.transform.position.x;
        _invenx = _invenPanel.transform.localPosition.x;
        _inveny = _invenPanel.transform.localPosition.y;
        _infox = _invenInfoPanel.transform.localPosition.x;

        //_invenInfoPanel.transform.DOLocalMoveX(_invenx + moveXVal, 0f);
        //_invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);
        //_invenInfoPanel.transform.DOLocalMoveY(_inveny + moveYVal, 0f);

        _components = _invenPanel.transform.Find("Components").transform;
        invenRenderer = _invenPanel.GetComponent<Image>();
        fade = transform.Find("FadePanel").GetComponent<PanelFade>();
        mat = invenRenderer.material;

        mat.SetFloat(invenShader, 0f);
    }

    private void Update()
    {
        if (canOpen && !GameManager.Instance.isPlay)
        {
            if (((KeyManager.Instance == null && Input.GetKeyDown(KeyCode.Tab)) ||
                (KeyManager.Instance != null && Input.GetKeyDown(KeyManager.Instance.inven)) || 
                (Input.GetKeyDown(KeyCode.Escape) && isOn)) && isAnimation)
            {
                isAnimation = false;
                StartCoroutine(ShowLineRender());
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
        fade.Fade(true);
        _components.localPosition = new Vector3(0, 0, 0);
        _invenPanel.transform.DOLocalMoveY(_inveny, 0f);
        invenRenderer.enabled = true;
        float initValue = mat.GetFloat(invenShader);
        seq = DOTween.Sequence();
        //seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny, time)).SetEase(Ease.OutBounce);
        //seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny, time)).SetEase(Ease.OutBounce);
        //seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_infox, time)).SetEase(Ease.OutBounce);
        seq.Append(DOTween.To(()=> initValue, value => mat.SetFloat(invenShader, value), 15f, easingtime)).SetEase(ease);
        seq.Join(ScreenManager.Instance.SetEffect(0.3f, 0.65f, DG.Tweening.Ease.InQuad));
        seq.Join(_playerUI.transform.DOMoveX(_uix - moveXVal, time));
        seq.AppendCallback(() => { isAnimation = true; });
        
        //ScreenManager.Instance.SetEffect(5, easingtime, ease);
    }

    private void ShowUI()
    {
        //DOTween.Kill(seq);

        fade.Fade(false);
        _components.localPosition = new Vector3(0, 1000, 0);
        ScreenManager.Instance.SetEffect(0, 0.5f, DG.Tweening.Ease.InQuart);
        float initValue = mat.GetFloat(invenShader);
        _playerUI.transform.DOMoveX(_uix, time).SetEase(Ease.OutBack);
        seq = DOTween.Sequence();
        //seq.Append(_invenInfoPanel.transform.DOLocalMoveX(_invenx + moveXVal, time)).SetEase(Ease.OutBack);
        //seq.Append(_invenPanel.transform.DOLocalMoveY(_inveny + moveYVal, time)).SetEase(Ease.OutBack);
        //seq.Join(_invenInfoPanel.transform.DOLocalMoveY(_inveny + moveYVal, time)).SetEase(Ease.OutBack);
        seq.Append(DOTween.To(() => initValue, value => mat.SetFloat(invenShader, value), 0f, easingtime).SetEase(ease));
        seq.AppendCallback(() => { StartCoroutine(DelayCo()); });
            
        //ScreenManager.Instance.SetEffect(0, 0.5f, ease);
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
