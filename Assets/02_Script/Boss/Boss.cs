using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IHitAble
{
    protected bool _hit = false;

    public Rigidbody2D rigid;

    public GameObject player;

    public Slider bossHpSlider;

    public bool isStop;
    public bool dead;

    public float currentHp;

    public BossDataSO bossSo;

    public FeedbackPlayer feedbackPlayer { get; set; }

    public event Action DeadEvt;

    private Vector2 _beforeVec;

    private void Awake()
    {
        feedbackPlayer = GetComponent<FeedbackPlayer>();
        isStop = false;
        dead = false;
        currentHp = bossSo.MaxHP;
        DeadEvt += DieEvent;
        bossHpSlider.value = currentHp / bossSo.MaxHP;
    }

    protected virtual void Update()
    {
        bossHpSlider.value = currentHp / bossSo.MaxHP;

        if(!WallChecker())
        {
            if (WallChecker())
            {
                StartCoroutine(StepBack());
            }
        }
    }

    public void StopImmediately(Rigidbody2D rigid)
    {
        rigid.velocity = Vector2.zero;
    }

    public void Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (dead)
            return;

        _hit = true;
        currentHp -= damage;
        feedbackPlayer.Play(damage);
        //Debug.Log(currentHp + "," + damage);
        
        if(currentHp < 0)
        {
            IsDead();
            bossHpSlider.value = 0;
            return;
        }
    }

    private IEnumerator StepBack()
    {
        yield return null;

        rigid.velocity = -_beforeVec;
    }

    private bool WallChecker()
    {
        if (Physics2D.OverlapBox(transform.position,
            transform.localScale, 0, LayerMask.GetMask("Wall")))
        {
            _beforeVec = rigid.velocity;
            StopImmediately(rigid);
            return true;
        }

        return false;
    }

    private void IsDead()
    {
        dead = true;
        
        DeadEvt?.Invoke();
    }

    private void DieEvent()
    {
        // �״� �ִϸ��̼�
        StopAllCoroutines();
        ReturnAll();
        Debug.Log("���� ���");
        gameObject.SetActive(false);
    }

    private void ReturnAll()
    {
        int childCount = transform.childCount;
        GameObject[] objs = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            objs[i] = transform.GetChild(i).gameObject;
        }
        for(int i = 0; i < childCount; i++)
        {
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("���� �� ����");
            if (collision.gameObject.TryGetComponent<IHitAble>(out var IhitAble))
            {
                Debug.Log("�÷��̾�� �������� ������ ��");
                IhitAble.Hit(Mathf.Pow(bossSo.Damage, 2));
            }
        }

        
    }
}
