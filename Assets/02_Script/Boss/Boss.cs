using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IHitAble
{
    public GameObject G_bulletCollector;

    public Slider bossHpSlider;

    public bool B_isStop;
    public bool B_dead;
    public bool B_isDead;
    public bool B_blocked;
    public bool B_isRunning;
    public bool B_awakening;

    public float F_currentHp;

    public BossDataSO bossSo;

    public FeedbackPlayer feedbackPlayer { get; set; }
    public Vector3 V_originPos;

    public event Action DeadEvt;

    protected Material m_mat;

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

    public bool Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (B_dead)
            return false;

        F_currentHp -= damage;
        feedbackPlayer.Play(damage);
        //Debug.Log(currentHp + "," + damage);
        
        if(F_currentHp < 0)
        {
            DeadEvt?.Invoke();

            return false;
        }
    
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bossSo.StopRadius);
    }

    private void DieEvent()
    {
        B_dead = true;
        bossHpSlider.value = 0;
    }

    public bool DontNeedToFollow()
    {
        if (CheckPlayerCircleCastB(bossSo.StopRadius))
        {
            return true;
        }

        return false;
    }

    private bool CheckPlayerCircleCastB(float radius)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        foreach (var h in hit)
        {
            if (h.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    public void ReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = G_bulletCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = G_bulletCollector.transform.GetChild(i).gameObject;
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;

            transform.position = Vector2.MoveTowards(transform.position, -dir * 2, Time.deltaTime);
            StopImmediately(transform);
            B_blocked = true;
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
