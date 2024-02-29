using FSM_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour, IHitAble
{
    [SerializeField] EnemyDataSO enemyDataSO;
    public EnemyDataSO EnemyDataSO => enemyDataSO;
    public FeedbackPlayer feedbackPlayer { get; set; }
    public bool Dead { get; private set; } = false;
    private int curHp;

    public event Action DeadEvent;

    private new Collider2D collider;
    private new Rigidbody2D rigidbody;
    public Collider2D Collider => collider;
    public Rigidbody2D Rigidbody => rigidbody;

    public RoomInfo RoomInfo { get; private set; } //내가 지금 위치해있는 room정보;

    [Header("Movement")]
    
    [SerializeField]
    float acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    Vector2 movementInput;
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    //Debug
    [SerializeField]
    private Tilemap _mainMap;
    [SerializeField]
    private Tilemap _wallMap;

    private void Awake()
    {
        curHp = enemyDataSO.MaxHP;
        DeadEvent += DieEvent;
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        //Debug 나중에 한곳에서 할당해줘야함.
        Debug.Log(_mainMap.cellBounds);

        RoomInfo roomInfo = new RoomInfo()
        {
            bound = _mainMap.cellBounds,
            pos = _mainMap.transform.position,
        };
        
        SetRoomInfo(roomInfo);

        //string seed = Time.time.ToString();

        //float xCoord = UnityEngine.Random.Range(0f, 1f);
        
        //Debug.Log($"XCoord : {xCoord}");
        //string debug = "";
        //for (float i = 0; i < 1; i += 0.01f)
        //{
        //    float perlinValue = Mathf.PerlinNoise(xCoord ,i);
        //    debug += perlinValue.ToString() + "\n";
        //}

        //Debug.Log(debug);

        //InvokeRepeating("GetPerlin", 0, 0.1f);
    }

    public void SetRoomInfo(RoomInfo curRoom)
    {
        RoomInfo = curRoom;
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.up * 2f,Color.red);

      
    }

    private void FixedUpdate()
    {
        float maxSpeed = EnemyDataSO.Speed;
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

    public void Hit(float damage)
    {
        if (Dead) return;

        curHp -= (int)damage;

        if (curHp <= 0)
        {
            Dead = true;
            Die();
            return;
        }

        feedbackPlayer.Play(damage + UnityEngine.Random.Range(0.25f, 1.75f));
    }

    private void Die()
    {
        Debug.Log("Die");

        DeadEvent?.Invoke();
    }

    private void DieEvent()
    {
    }
}
