using System;
using UnityEngine;

public delegate void Trigger(TriggerID trigger, Weapon weapon);
public class EventTriggerManager : MonoBehaviour
{
    public static EventTriggerManager Instance;

    static ulong index = 0;

    Rigidbody2D _playerRb;

    #region 이벤트
    public event Trigger OnIdleExecute;
    public event Trigger OnMoveExecute;
    #endregion

    #region 초기화
    private void Awake()
    {
        #region 싱긅톤
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : EventTriggerManager is multiply running!");
            Destroy(gameObject);

        }
        #endregion

        #region 객체 할당
        _playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        #endregion

        #region 예외처리
        if (SkillManager.Instance == null)
            Debug.LogError("SkillManager is null! : 아무데나 넣으세여");
        #endregion

    }
    #endregion

    private void Start()
    {
        #region 이벤트 연결
        OnIdleExecute += SkillManager.Instance.DetectTrigger;
        OnMoveExecute += SkillManager.Instance.DetectTrigger;
        #endregion
    }

    #region 항상 호출되는 트리거
    private void Update()
    {
        AlwaysExecute();
        CoolExecute();
        IdleExecute();
        RunExecute();
    }
    #endregion
    
    public static ulong GetIndex()
    {
        if (index++ == ulong.MaxValue)
            index = ulong.MinValue;
        return index;
    }

    #region 익스큐트 관리
    public void CoolExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.CoolTime);
    }

    public void IdleExecute()
    {
        if (_playerRb.velocity == Vector2.zero)
        {
            OnIdleExecute.Invoke(TriggerID.Idle, null);
        }
    }

    public void BasicAttackExecute(Weapon weapon)
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.NormalAttack,weapon);
    }

    public void RunExecute()
    {
        if (_playerRb.velocity != Vector2.zero)
        {
            OnMoveExecute?.Invoke(TriggerID.Move, null);
        }
    }

    public void DashExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.Dash);
    }

    public void HitExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.GetHit);
    }

    public void RoomClearExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.RoomClear);
    }

    public void StageClearExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.StageClear);
    }

    public void WaveStartExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.WaveStart);
    }

    public void SkillExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.UseSkill);
    }

    public void EnemyDieExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.Kill);
    }

    public void AlwaysExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.Always);
    }

    public void RegistExecute()
    {
        SkillManager.Instance?.DetectTrigger(TriggerID.Regist);
    }
    #endregion

    // 트리거 초기화 및 재탐색
    public void ResetTrigger()
    {
        SkillManager.Instance.Init();

        PlayerController.EventController?.OnMoveExecute();
        PlayerController.EventController?.OnDashExecute();
        PlayerController.EventController?.OnBasicAttackExecute();
        PlayerController.EventController?.OnEnemyDieExecute();
        PlayerController.EventController?.OnCoolExecute();
        PlayerController.EventController?.OnIdleExecute();
        PlayerController.EventController?.OnHitExecute();
        PlayerController.EventController?.OnRoomClearExecute();
        PlayerController.EventController?.OnStageClearExecute();
        PlayerController.EventController?.OnWaveStartExecute();
        PlayerController.EventController?.OnSkillxecute();
        PlayerController.EventController?.OnAlwaysExecute();
        PlayerController.EventController?.OnRegistExecute();
        
        
        SkillManager.Instance.RegistEndEvent();
    }
}
