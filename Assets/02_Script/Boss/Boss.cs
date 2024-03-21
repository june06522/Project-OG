using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IHitAble
{
    public Material MyMat { get => _mat; set => _mat = value; }

    public List<Material> mats;

    public GameObject player;
    public GameObject bulletCollector;
    public GameObject chainCollector;

    public Slider bossHpSlider;

    public bool isStop;
    public bool dead;
    public bool wasDead;
    public bool blocked;
    public bool isTied;
    public bool isRunning;
    public bool isOneBroken;
    public bool awakening;

    public float currentHp;

    public BossDataSO bossSo;

    public FeedbackPlayer feedbackPlayer { get; set; }

    public Vector3 originPos;

    public event Action DeadEvt;

    private Vector2 _beforeVec;
    private Material _mat;

    private void Awake()
    {
        _mat = transform.GetComponent<SpriteRenderer>().material;
        feedbackPlayer = GetComponent<FeedbackPlayer>();
        isStop = false;
        dead = false;
        currentHp = bossSo.MaxHP;
        DeadEvt += DieEvent;
        bossHpSlider.value = currentHp / bossSo.MaxHP;
    }

    protected virtual void Update()
    {
        transform.GetComponent<SpriteRenderer>().material = _mat;
        bossHpSlider.value = currentHp / bossSo.MaxHP;
    }

    public void StopImmediately(Transform trans)
    {
        trans.position = Vector2.MoveTowards(trans.position, trans.position, Time.deltaTime);
    }

    public void Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (dead)
            return;

        currentHp -= damage;
        feedbackPlayer.Play(damage);
        //Debug.Log(currentHp + "," + damage);
        
        if(currentHp < 0)
        {
            DeadEvt?.Invoke();

            return;
        }
    }

    private void DieEvent()
    {
        dead = true;
        bossHpSlider.value = 0;
    }

    public void ReturnAll(bool isInBulletCollector)
    {
        int childCount = 0;
        GameObject[] objs;
        if (isInBulletCollector)
        {
            childCount = bulletCollector.transform.childCount;
            objs = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                objs[i] = bulletCollector.transform.GetChild(i).gameObject;
            }
        }
        else
        {
            childCount = chainCollector.transform.childCount;
            objs = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                objs[i] = chainCollector.transform.GetChild(i).gameObject;
            }
        }
        
        for(int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<IHitAble>(out var IhitAble))
            {
                IhitAble.Hit(Mathf.Pow(bossSo.Damage, 2));
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;

            transform.position = Vector2.MoveTowards(transform.position, -dir * 2, Time.deltaTime);
            StopImmediately(transform);
            blocked = true;
        }
    }
}
