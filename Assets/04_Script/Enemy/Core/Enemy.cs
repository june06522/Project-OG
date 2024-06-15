using DSNavigation;
using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitAble
{
    [SerializeField]
    EnemyDataSO enemyDataSO;
    public EnemyDataSO EnemyDataSO => enemyDataSO;

    [field: SerializeField]
    public EnemyAnimController enemyAnimController { get; set; }

    [Header("Movement")]
    [SerializeField]
    float acceleration = 50;
    [SerializeField]
    float deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private float speedRatio = 1;
    
    private Vector2 oldMovementInput;
    private Vector2 movementInput;
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    
    [Header("Health")]
    private int curHp;
    public int CurHP
    {
        get => curHp;
    }

    public bool Dead { get; private set; } = false;

    public bool isNotMoveingEnemy = false;

    public event Action DeadEvent;
    public event Action<float> HitEvent;

    [SerializeField]
    private AudioClip _hitSound;

    //ETC
    [field:SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }

    private new Collider2D collider;
    private new Rigidbody2D rigidbody;
    public Collider2D Collider => collider;
    public Rigidbody2D Rigidbody => rigidbody;

    //PathFinding
    private JPSPathFinderFaster m_jpsPathFinderFaster;
    public JPSPathFinderFaster GetPathFinder => m_jpsPathFinderFaster;
    public Stage OwnerStage { get; set; }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        m_jpsPathFinderFaster = GetComponent<JPSPathFinderFaster>();
    }

    private void Start()
    {
        curHp = enemyDataSO.MaxHP;
        DeadEvent += DieEvent;
        speedRatio = 1;
    }

    private void Update()
    {
        if(movementInput != Vector2.zero)
            enemyAnimController.Flip(oldMovementInput);

        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftControl))
            Die(); //test
    }

    private void FixedUpdate()
    {
        if(isNotMoveingEnemy)
        {
            return;
        }

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
        HitEvent?.Invoke(damage);
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
        EventTriggerManager.Instance?.EnemyDieExecute();
        Destroy(gameObject);
    }
}
