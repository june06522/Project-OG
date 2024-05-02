using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState
{
    Idle,
    Tied,
    OneBroken,
    Free,

    FullHP,
    HalfHP,

    Flowering,
    FullBloom,
    Withered,
    Dead
}

public struct Body
{
    public Color color;
    public Vector3 scale;
    public Quaternion rotation;
}

// inspector에서 보이는 변수들
public partial class Boss
{
    [field: Header("Feedback")]
    [field: SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }

    [field: Header("Boss Move")]
    public BossMove bossMove;

    [field: Header("SO")]
    public BossDataSO so;

    [field: Header("UI")]
    public Slider bossHPSlider;

    [Header("GameObject")]
    public GameObject bulletCollector;
}

public partial class Boss : MonoBehaviour, IHitAble
{
    [HideInInspector] public bool isStop;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isBlocked;

    private Color _originColor;

    public event Action DieEvt;
    public event Action DeadEndEvt;

    public bool IsDie
    {
        get { return _isDie; }
    }

    protected float _currentHP;

    protected bool _fakeDie;

    private bool _isDie;

    protected virtual void OnEnable()
    {
        _fakeDie = false;
        _isDie = false;
        isStop = false;
        isDead = false;
        _currentHP = so.MaxHP;
        DieEvt += DieEvent;
    }

    protected virtual void OnDisable()
    {
        DeadEndEvt?.Invoke();
    }

    protected virtual void Update()
    {
        bossHPSlider.value = _currentHP / so.MaxHP;
    }

    public void SetBodyToBasic(Body body, GameObject obj)
    {
        obj.transform.localScale = body.scale;
        obj.transform.rotation = body.rotation;
        obj.GetComponent<SpriteRenderer>().color = body.color;
    }

    protected void SetBody(ref Body body, GameObject obj)
    {
        body.color = obj.GetComponent<SpriteRenderer>().color;
        if(_originColor != body.color)
        {
            _originColor = body.color;
        }
        body.scale = obj.transform.localScale;
        body.rotation = obj.transform.rotation;
    }

    private void DieEvent()
    {
        if(!_fakeDie)
        {
            _isDie = true;
        }
        bossHPSlider.value = 0;
    }

    public bool Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (_isDie)
            return false;

        _currentHP -= damage;
        feedbackPlayer?.Play(damage);

        if (_currentHP < 0)
        {
            DieEvt?.Invoke();

            return false;
        }

        return true;
    }

    protected void MakeDie(bool b)
    {
        _isDie = b;
        bossHPSlider.value = 0;
    }

    public void ChangeMaterial(GameObject obj, Material mat)
    {
        obj.GetComponent<SpriteRenderer>().material = mat;
    }

    public void ChangeSprite(GameObject obj, Sprite sprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void StopImmediately(Transform trans)
    {
        trans.position = Vector2.MoveTowards(trans.position, trans.position, Time.deltaTime);
    }

    public void ReturnAll()
    {
        int childCount = bulletCollector.transform.childCount;
        GameObject[] objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = bulletCollector.transform.GetChild(i).gameObject;
        }
        
        for(int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }

        if(bulletCollector.transform.childCount > 0)
        {
            Destroy(bulletCollector.transform.GetChild(0).gameObject);
        }
    }

    public IEnumerator Blinking(GameObject obj, float blinkingTime, float a, int orderInLayer, Color color, Sprite sprite = null, bool willDisappear = false)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Sprite originSprite = renderer.sprite;
        int originOrderInLayer = renderer.sortingOrder;
        float currentTime = 0;

        if(sprite != null)
        {
            renderer.sprite = sprite;
        }
        renderer.color = color * new Color(1, 1, 1, a);
        renderer.sortingOrder = orderInLayer;

        while (currentTime < blinkingTime)
        {
            currentTime += Time.deltaTime;

            renderer.color -= new Color(0, 0, 0, Time.deltaTime);

            yield return null;
        }


        renderer.color = _originColor;
        if(willDisappear)
        {
            renderer.color *= new Color(1, 1, 1, 0);
        }
        renderer.sortingOrder = originOrderInLayer;
        if(sprite != null)
        {
            renderer.sprite = originSprite;
        }
    }

    public IEnumerator Poping(GameObject obj, float popingTime, float multiply)
    {
        Vector3 originSize = obj.transform.localScale;
        float currentTime = 0;

        obj.transform.localScale = originSize * multiply;

        while(currentTime < popingTime)
        {
            currentTime += Time.deltaTime;

            obj.transform.localScale -= new Vector3(Time.deltaTime / multiply, Time.deltaTime / multiply, 0);

            yield return null;
        }

        obj.transform.localScale = originSize;
    }

    public IEnumerator RotateYObject(GameObject obj, float rotateTime, float endAngle)
    {
        float currentTime = 0;
        float currentAngle = 0;

        while (currentTime < rotateTime)
        {
            currentTime += Time.deltaTime;
            currentAngle += Time.deltaTime * endAngle;

            obj.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
            yield return null;
        }
        obj.transform.rotation = Quaternion.Euler(0, endAngle, 0);
        yield return null;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isBlocked = true;
        }
    }
}
