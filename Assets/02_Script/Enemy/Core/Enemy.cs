using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour, IHitAble, IDebuffReciever
{
    [SerializeField] EnemyDataSO enemyDataSO;
    public EnemyDataSO EnemyDataSO => enemyDataSO;

    [Header("Visual")]
    [SerializeField]
    private Transform _visualTrm;
    [field: SerializeField]
    public EnemyAnimController enemyAnimController { get; set; }

    [Header("Movement")]
    [SerializeField]
    float acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private float speedRatio = 1;
    
    private Vector2 oldMovementInput;
    private Vector2 movementInput;
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    
    [Header("Health")]
    private int curHp;
    public bool Dead { get; private set; } = false;
    public event Action DeadEvent;

    [SerializeField]
    private AudioClip _hitSound;
    //ETC
    public FeedbackPlayer feedbackPlayer { get; set; }

    private new Collider2D collider;
    private new Rigidbody2D rigidbody;
    public Collider2D Collider => collider;
    public Rigidbody2D Rigidbody => rigidbody;

    public EDebuffType DebuffType { get; set; }
    public float DebuffCoolTime { get; set; }

    private SpriteRenderer spriteRender;

    public Color frozenColor = new Color(0.7584906f, 0.9797822f, 1, 1);

    public bool IsDebuffing;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        spriteRender = _visualTrm.GetComponent<SpriteRenderer>();
        feedbackPlayer = _visualTrm.GetComponent<FeedbackPlayer>();
    }

    private void Start()
    {
        DebuffType = EDebuffType.None;
        curHp = enemyDataSO.MaxHP;
        DeadEvent += DieEvent;
        speedRatio = 1;
        IsDebuffing = false;
    }

    private void Update()
    {
        if(movementInput != Vector2.zero)
            enemyAnimController.Flip(oldMovementInput);

        if (Input.GetKeyDown(KeyCode.T))
            Die(); //test
    }

    private void FixedUpdate()
    {
        float maxSpeed = EnemyDataSO.Speed * speedRatio;
        if (MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
       
        //Debug.Log(movementInput);
        Vector3 position = rigidbody.position
                            + (oldMovementInput * currentSpeed * Time.deltaTime);
        rigidbody.MovePosition(position);
    }

    public bool Hit(float damage)
    {
        if (Dead) return false;

        SoundManager.Instance.SFXPlay("HitEnemy", _hitSound, 0.55f);
        feedbackPlayer?.Play(damage + UnityEngine.Random.Range(0.25f, 1.75f));
        curHp -= (int)damage;

        if (curHp <= 0)
        {
            Die();
            return false;
        }
         
        return true;
    }

    public void Die()
    {
        if(Dead) return;
        Dead = true;
        DeadEvent?.Invoke();
    }

    private void DieEvent()
    {
        Destroy(gameObject);
    }

    public void DisposeDebuff()
    {
        if (DebuffType.HasFlag(EDebuffType.Frozen))
        {
            speedRatio = 1f;
        }
        else if (DebuffType.HasFlag(EDebuffType.Burn))
        {

        }
        else if (DebuffType.HasFlag(EDebuffType.Poison))
        {

        }
        spriteRender.color = Color.white;

        IsDebuffing = false;
    }


    public void DebuffEffect(EDebuffType debuffType, float coolTime)
    {
        if (IsDebuffing)
            return;

        Debug.Log(debuffType);
        if(debuffType == EDebuffType.None)
        {
            return;
        }
        else if(debuffType.HasFlag(EDebuffType.Frozen))
        {
            Debug.Log("Gang");
            spriteRender.color = frozenColor;
            speedRatio = 0.2f;
        }
        else if (debuffType.HasFlag(EDebuffType.Burn))
        {

        }
        else if (debuffType.HasFlag(EDebuffType.Poison))
        {

        }


        IsDebuffing = true;
        StopAllCoroutines();
        StartCoroutine(DebuffCor(coolTime));
    }
    IEnumerator DebuffCor(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        DisposeDebuff();
    }
}
