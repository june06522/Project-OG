using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum IconType
{
    INVEN,
    INFO
}

public class IconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    IconType iconType;

    Material material;

    RectTransform childTransform;
    RectTransform myTransform;

    [SerializeField]
    float scaleUpRatio = 1.5f; 
    [SerializeField]
    float duration = 0.4f;

    struct UISize
    {
        public float Width;
        public float Height;

        public UISize(float width, float height)
        {
            Width = width; 
            Height = height;
        }
    }

    UISize childBaseSize;
    UISize myBaseSize;

    bool isHover = false;

    Coroutine scaleCoroutine;
    void Awake()
    {
        childTransform = transform.GetComponentInChildren<RectTransform>();
        myTransform = GetComponent<RectTransform>();    
    }

    void Start()
    {
        Image image = transform.Find("Image").GetComponent<Image>();
        //image.material = Instantiate(image.material);
        material = image.material;

        //myBaseSize = new UISize(myTransform.rect.width, myTransform.rect.height);
        //childBaseSize = new UISize(childTransform.rect.width, childTransform.rect.height);

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        ItemExplain.Instance.SetCurrentInfoType(iconType);   
    }

    public void DisConnect()
    {
        material.SetFloat("_SplitToningFade", 1f);
    }

    public void OnConnect()
    {
        material.SetFloat("_SplitToningFade", 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //isHover = true;
        //if (scaleCoroutine != null)
        //    StopCoroutine(scaleCoroutine);

        //scaleCoroutine = StartCoroutine(ScaleTween());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //isHover = false;
        //if (scaleCoroutine != null)
        //    StopCoroutine(scaleCoroutine);

        //scaleCoroutine = StartCoroutine(ReTurnScaleTween());
    }

    private IEnumerator ScaleTween()
    {
        float t = 0;
        float startScale = myBaseSize.Width;
        float endScale = myBaseSize.Width * scaleUpRatio;
        float currentScale = myTransform.rect.width; //0.2f

        //(2f - 1.2f / (2f - 1f))
        float startScaleRatio = (endScale - currentScale) / (endScale - startScale);
        float duration = this.duration * startScaleRatio;

        float curScaleRatio  = currentScale / startScale;

        while (t < duration)
        {
            Rect childRect = childTransform.rect;
            Rect myRect = myTransform.rect;

            childRect.width = childBaseSize.Width * curScaleRatio;
            childRect.height = childBaseSize.Height * curScaleRatio;

            myRect.width = myBaseSize.Width * curScaleRatio;
            myRect.height = myBaseSize.Height * curScaleRatio;

            curScaleRatio = Mathf.Lerp(1, scaleUpRatio, t);
            t += Time.deltaTime;
            Debug.Log("CurScaleRatio : " + curScaleRatio);
            yield return null;
        }
    }

    private IEnumerator ReTurnScaleTween()
    {
        float t = 0;
        float startScale = myBaseSize.Width * scaleUpRatio;
        float endScale = myBaseSize.Width;
        float currentScale = myTransform.rect.width; // 0.8f

        float startScaleRatio = 1f - (startScale - currentScale) / (startScale - endScale);
        float duration = this.duration * startScaleRatio;

        float curScaleRatio = currentScale / endScale;

        while (t < duration)
        {
            Rect childRect = childTransform.rect;
            Rect myRect = myTransform.rect;

            childRect.width = childBaseSize.Width * curScaleRatio;
            childRect.height = childBaseSize.Height * curScaleRatio;

            myRect.width = myBaseSize.Width * curScaleRatio;
            myRect.height = myBaseSize.Height * curScaleRatio;

            curScaleRatio = Mathf.Lerp(curScaleRatio, 1, t);
            t += Time.deltaTime;


            Debug.Log("CurScaleRatio : " + curScaleRatio);
            yield return null;
        }
    }
}
