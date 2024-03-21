using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IHitAble
{
    public Material MyMat { get => m_mat; set => m_mat = value; }

    public List<Material> L_mats;

    public GameObject G_player;
    public GameObject G_bulletCollector;
    public GameObject G_chainCollector;

    public Slider bossHpSlider;

    public bool B_isStop;
    public bool B_dead;
    public bool B_wasDead;
    public bool B_blocked;
    public bool B_isTied;
    public bool B_isRunning;
    public bool B_isOneBroken;
    public bool B_awakening;

    public float F_currentHp;

    public BossDataSO bossSo;

    public FeedbackPlayer feedbackPlayer { get; set; }

    public Vector3 V_originPos;

    public event Action DeadEvt;

    private Vector2 v_beforeVec;
    private Material m_mat;

    private void Awake()
    {
        m_mat = transform.GetComponent<SpriteRenderer>().material;
        feedbackPlayer = GetComponent<FeedbackPlayer>();
        B_isStop = false;
        B_dead = false;
        F_currentHp = bossSo.MaxHP;
        DeadEvt += DieEvent;
        bossHpSlider.value = F_currentHp / bossSo.MaxHP;
    }

    protected virtual void Update()
    {
        transform.GetComponent<SpriteRenderer>().material = m_mat;
        bossHpSlider.value = F_currentHp / bossSo.MaxHP;
    }

    public void StopImmediately(Transform trans)
    {
        trans.position = Vector2.MoveTowards(trans.position, trans.position, Time.deltaTime);
    }

    public void Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (B_dead)
            return;

        F_currentHp -= damage;
        feedbackPlayer.Play(damage);
        //Debug.Log(currentHp + "," + damage);
        
        if(F_currentHp < 0)
        {
            DeadEvt?.Invoke();

            return;
        }
    }

    private void DieEvent()
    {
        B_dead = true;
        bossHpSlider.value = 0;
    }

    public void ReturnAll(bool isInBulletCollector)
    {
        int childCount = 0;
        GameObject[] objs;
        if (isInBulletCollector)
        {
            childCount = G_bulletCollector.transform.childCount;
            objs = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                objs[i] = G_bulletCollector.transform.GetChild(i).gameObject;
            }
        }
        else
        {
            childCount = G_chainCollector.transform.childCount;
            objs = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                objs[i] = G_chainCollector.transform.GetChild(i).gameObject;
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
            B_blocked = true;
        }
    }
}
