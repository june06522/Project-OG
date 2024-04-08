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

public class Boss : MonoBehaviour, IHitAble
{
    public FeedbackPlayer feedbackPlayer { get; set; }

    public GameObject bulletCollector;

    public BossMove bossMove;

    public BossDataSO so;

    public Slider bossHPSlider;

    public event Action DieEvt;
    public event Action DeadEndEvt;

    public bool isStop;
    public bool isAttacking;
    public bool isDead;

    public bool IsDie
    {
        get { return _isDie; }
    }

    protected float _currentHP;

    private bool _isDie;

    protected virtual void OnEnable()
    {
        _isDie = false;
        isStop = false;
        isDead = false;
        _currentHP = so.MaxHP;
        DieEvt += DieEvent;
        feedbackPlayer = GetComponent<FeedbackPlayer>();
    }

    protected virtual void OnDisable()
    {
        DeadEndEvt?.Invoke();
    }

    protected virtual void Update()
    {
        bossHPSlider.value = _currentHP / so.MaxHP;
    }

    private void DieEvent()
    {
        _isDie = true;
        bossHPSlider.value = 0;
    }

    public bool Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (_isDie)
            return false;

        _currentHP -= damage;
        feedbackPlayer.Play(damage);

        if (_currentHP < 0)
        {
            DieEvt?.Invoke();

            return false;
        }

        return true;
    }

    public void ChangeMaterial(Material mat)
    {
        transform.GetComponent<SpriteRenderer>().material = mat;
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
}
