using System;
using System.Collections.Generic;
using UnityEngine;

// 생성기가 가지고 있는거
// 표적 타입, 생성기 ID(스킬 종류)

// 무기가 가지고 있는거
// 자기 타입, 자기 ID

// 무기별 ID
[Serializable]
public enum WeaponID
{
    None,       // 아무것도 아님
    LightSaber,
    Drone,
    Speaker,
    LaserGun,
    Emp,
}

// 생성기별 ID
[Serializable]
public enum GeneratorID
{
    None,           // 없음
    DashAttack,     // 대쉬 공격              대쉬
    DeathRay,       // 광선                  기본 공격
    Electronic,     // 전기                  대쉬
    ErrorDice,      // 랜덤 스킬              쿨타임 (10초)
    Firecracker,    // 폭죽                   방클
    Force,          // 범위 및 파워 증가       기본 공격
    HeartBeat,      // 슬로우                 대쉬
    LaserPointer,   // 레이저 발사             이동
    MagneticField,  // 자기장                 기본 공격
    OverLoad,       // 폭발 공격              대쉬
    Reboot,         // 잔상 공격              대쉬
    RotateWeapon,   // 무기 회전              이동
    SequenceAttack, // 연속 공격              대쉬 or 쿨타임
    SiegeMode,      // 공속 증가              가만히 있을 시(1초)
    Trinity,        // 대미지 증가            기본 공격
    WeaponShot,     // 무기 발사              쿨타임(5초)
}

public enum TriggerID
{
    None,           // 없음
    Dash,           // 대쉬
    NormalAttack,   // 기본 공격
    CoolTime,       // 쿨타임
    Move,           // 이동
    Idle,           // 가만히
    RoomClear,      // 방 클리어
}


// Skill 2차원 리스트는 인스펙터에서 안보임 이렇게 해야함
[Serializable]
public class Shell
{
    public List<Skill> skillList;
}

public class SkillContainer : MonoBehaviour
{

    private static SkillContainer instance;
    public static SkillContainer Instance => instance;

    [SerializeField] List<Shell> weaponList;

    private void Awake()
    {

        if (instance != null)
        {

            Debug.LogError("Multiple SkillManager is running");
            Destroy(instance);

        }

        instance = this;

    }

    /// <summary>
    /// ID에 맞는 스킬 뱉는 애
    /// </summary>
    /// <param name="i">무기 ID</param>
    /// <param name="j">생성기 ID</param>
    /// <returns></returns>
    public Skill GetSKill(int i, int j)
    {
        // prevention Out of index
        if (weaponList.Count > j && weaponList[j].skillList.Count > i)
        {

            // Checks for existence of a value
            if (weaponList[j] != null && weaponList[j].skillList[i] != null)
            {

                EventTriggerManager.Instance?.SkillExecute();
                return weaponList[j].skillList[i];

            }

        }

        Debug.LogError($"Skill Doesn't exist in SkillList : {j}, {i} ");

        return null;

    }

    public List<Shell> GetList()
    {
        return weaponList;
    }
}