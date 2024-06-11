using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID, string> generatorExplain = new()
    {
        {GeneratorID.None,             "아무 스킬도 없습니다." },
        {GeneratorID.DashAttack,       "대쉬 시 강화 공격이 나갑니다."}, // 대쉬 어택
        {GeneratorID.DeathRay,         "죽음의 광선을 발사합니다."}, // 마관광살포
        {GeneratorID.Electronic,       "무기에 전기 속성을 부여합니다."}, // 전기 지짐이
        {GeneratorID.ErrorDice,        "무작위 스킬이 실행됩니다."}, // 에러다이스
        {GeneratorID.Firecracker,      "축포를 터트립니다."}, // 축포
        {GeneratorID.Force,            "쾅"}, // 포쓰
        {GeneratorID.HeartBeat,        "쿵"}, // 하트비트
        {GeneratorID.LaserPointer,     "무기에서 레이저를 발사합니다."}, // 레이저 포인터
        {GeneratorID.MagneticField,    "무기가 자기장을 생성합니다."}, //자기장
        {GeneratorID.OverLoad,         "과부화"}, // 과부화
        {GeneratorID.Reboot,           "리부트"}, // 리부트
        {GeneratorID.RotateWeapon,     "무기를 회전시킵니다."}, // 로테이트 웨폰
        {GeneratorID.SequenceAttack,   "연속 공격"}, // 시퀀스
        {GeneratorID.SiegeMode,        "공격 속도가 빨라집니다."}, // 시즈모드
        {GeneratorID.Trinity,          "데미지가 증가합니다."}, //삼위일체
        {GeneratorID.WeaponShot,       "무기를 날려 공격합니다."}, // Weapon Shot
    };
    public static Dictionary<GeneratorID, TriggerID> triggerExplain = new()
    {
        {GeneratorID.None,            TriggerID.None },
        {GeneratorID.DashAttack, TriggerID.Dash},
        {GeneratorID.DeathRay, TriggerID.NormalAttack},
        {GeneratorID.Electronic, TriggerID.Dash},
        {GeneratorID.ErrorDice, TriggerID.CoolTime},
        {GeneratorID.Firecracker, TriggerID.RoomEnter},
        {GeneratorID.Force, TriggerID.NormalAttack},
        {GeneratorID.HeartBeat, TriggerID.Dash},
        {GeneratorID.LaserPointer, TriggerID.Move},
        {GeneratorID.MagneticField, TriggerID.NormalAttack},
        {GeneratorID.OverLoad, TriggerID.Dash},
        {GeneratorID.Reboot, TriggerID.Dash},
        {GeneratorID.RotateWeapon, TriggerID.Move},
        {GeneratorID.SequenceAttack, TriggerID.CoolTime}, // 얜 미정
        {GeneratorID.SiegeMode, TriggerID.Idle},
        {GeneratorID.Trinity, TriggerID.NormalAttack},
        {GeneratorID.WeaponShot, TriggerID.CoolTime},
    };
    public static Dictionary<TriggerID, string> triggerName = new()
    {
        { TriggerID.None,          "발동되지 않음" },// 없음
        { TriggerID.Dash,          "대쉬 시 발동" },// 대쉬
        { TriggerID.NormalAttack,  "기본 공격 시 발동" },// 기본 공격
        { TriggerID.CoolTime,      "쿨타임마다 발동" },// 쿨타임
        { TriggerID.Move,          "움직일 때 발동" },// 이동
        { TriggerID.Idle,          "가만히 있을 시 발동" },// 가만히
        { TriggerID.RoomEnter,     "방 입장 시 발동" },   // 방 입장시
        { TriggerID.GetHit,        "피격 시 발동" },             // 피격
        { TriggerID.Kill,          "적 처치 시 발동" },              // 처치
        { TriggerID.UseSkill,      "다른 스킬 발동 시 발동" },       // 스킬 사용
        { TriggerID.StageClear,    "스테이지 클리어 시 발동" },// 스테이지 클리어
        { TriggerID.WaveStart,     "웨이브 시작 때 발동" },// 웨이브 시작
        { TriggerID.Always,        "상시 발동" },// 항상
        { TriggerID.Regist,        "움직일 때 발동" },// 등록(로테이트 전용)
    };

    public static Dictionary<GeneratorID, string> generatorName = new()
    {
        {GeneratorID.None,             "테스트" },
        {GeneratorID.DashAttack,       "대쉬 어택"}, // 대쉬 어택
        {GeneratorID.DeathRay,         "마관광살포"}, // 마관광살포
        {GeneratorID.Electronic,       "전기 지짐이"}, // 전기 지짐이
        {GeneratorID.ErrorDice,        "에러다이스"}, // 에러다이스
        {GeneratorID.Firecracker,      "축포"}, // 축포
        {GeneratorID.Force,            "포스"}, // 포쓰
        {GeneratorID.HeartBeat,        "하트비트"}, // 하트비트
        {GeneratorID.LaserPointer,     "레이저 포인터"}, // 레이저 포인터
        {GeneratorID.MagneticField,    "자기장"}, //자기장
        {GeneratorID.OverLoad,         "과부화"}, // 과부화
        {GeneratorID.Reboot,           "리부트"}, // 리부트
        {GeneratorID.RotateWeapon,     "로테이트 웨폰"}, // 로테이트 웨폰
        {GeneratorID.SequenceAttack,   "시퀀스"}, // 시퀀스
        {GeneratorID.SiegeMode,        "시즈모드"}, // 시즈모드
        {GeneratorID.Trinity,          "삼위일체"}, //삼위일체
        {GeneratorID.WeaponShot,       "웨폰 샷"}, // Weapon Shot
    };
    public static Dictionary<ItemRate, string> itemRate = new()
    {
        {ItemRate.NORMAL,   "노말" },
        {ItemRate.RARE,     "레어" },
        {ItemRate.EPIC,     "에픽" },
        {ItemRate.LEGEND,   "레전드" }
    };
    public static Dictionary<WeaponID, string> weaponName = new()
    {
        {WeaponID.Emp,          "충격폭탄" },
        {WeaponID.LaserGun,     "레이저건" },
        {WeaponID.Speaker,      "스피커" },
        {WeaponID.LightSaber,   "광선검" },
        {WeaponID.Drone,        "드론" }
    };
    public static Dictionary<WeaponID, string> weaponExplain = new()
    {
        {WeaponID.Emp,          "폭탄을 던져 범위 피해를 입힙니다." },
        {WeaponID.LaserGun,     "강력한 레이저를 발사합니다." },
        {WeaponID.Speaker,      "전방위에 범위 피해를 입힙니다." },
        {WeaponID.LightSaber,   "가까운 적을 공격합니다." },
        {WeaponID.Drone,        "적에게 총알을 발사합니다." }
    };
}