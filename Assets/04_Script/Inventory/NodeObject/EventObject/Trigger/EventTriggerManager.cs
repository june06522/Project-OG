using System.Xml.Serialization;
using UnityEngine;

public class EventTriggerManager : MonoBehaviour
{
    public static EventTriggerManager Instance;
    RotateSkillManager rotateManager;

    static ulong index = 0;

    Rigidbody2D _playerRb;

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

        _playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        if (SkillManager.Instance == null)
            Debug.LogError("SkillManager is null! : 아무데나 넣으세여");
    }

    private void Start()
    {
        rotateManager = FindObjectOfType<RotateSkillManager>(); 
    }

    private void Update()
    {
        AlwaysExecute();
        CoolExecute();
        IdleExecute();
        RunExecute();
    }

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
            SkillManager.Instance?.DetectTrigger(TriggerID.Idle);
            rotateManager.IsRunning = false;
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
            SkillManager.Instance?.DetectTrigger(TriggerID.Move);
            rotateManager.IsRunning = true;
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
    #endregion

    public void a()
    {

    }

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
        
        
        SkillManager.Instance.RegistEndEvent();
    }
}
