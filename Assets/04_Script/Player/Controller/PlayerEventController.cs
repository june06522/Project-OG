using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : IDisposable
{
    #region 액션 관리
    public event Action OnMove;         // 이동
    public event Action OnDash;         // 대쉬
    public event Action OnBasicAttack;  // 기본 공격
    public event Action OnCool;         // 쿨타임
    public event Action OnIdle;         // Idle
    public event Action OnHit;          // 피격
    public event Action OnRoomEnter;    // 방 입장
    public event Action OnStageClear;   // 스테이지 클리어
    public event Action OnWaveStart;    // 웨이브 시작시
    public event Action OnSkill;        // 스킬 발동 시
    public event Action OnEnemyDie;     // 적 처치시
    #endregion

    #region 함수 호출
    public void OnMoveExecute()         => OnMove?.Invoke();
    public void OnDashExecute()         => OnDash?.Invoke();
    public void OnBasicAttackExecute()  => OnBasicAttack?.Invoke();
    public void OnCoolExecute()         => OnCool?.Invoke();
    public void OnIdleExecute()         => OnIdle?.Invoke();
    public void OnHitExecute()          => OnHit?.Invoke();
    public void OnRoomEnterExecute()    => OnRoomEnter?.Invoke();
    public void OnStageClearExecute()   => OnStageClear?.Invoke();
    public void OnWaveStartExecute()    => OnWaveStart?.Invoke();
    public void OnSkillxecute()         => OnSkill?.Invoke();
    public void OnEnemyDieExecute()     => OnEnemyDie?.Invoke();
    #endregion

    #region 디스포즈
    public void Dispose()
    {

        OnMove = null;
        OnDash = null;
        OnBasicAttack = null;
        OnCool = null;
        OnIdle = null;
        OnHit = null;
        OnRoomEnter = null;
        OnStageClear = null;
        OnWaveStart = null;
        OnSkill = null;
        OnEnemyDie = null;
    }
    #endregion
}
