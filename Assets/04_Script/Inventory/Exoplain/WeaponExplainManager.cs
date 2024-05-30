using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID, string> generatorExplain = new()
    {
        {GeneratorID.None,             "테스트용입니다." },
        {GeneratorID.DashAttack,       "대쉬는 훌륭한 공격 수단입니다."}, // 대쉬 어택
        {GeneratorID.DeathRay,         "죽음의 광선"}, // 마관광살포
        {GeneratorID.Electronic,       "일렉 뚜리뚜리"}, // 전기 지짐이
        {GeneratorID.ErrorDice,        "@#!%&($@1@$q!!"}, // 에러다이스
        {GeneratorID.Firecracker,      "축포를 터트립니다."}, // 축포
        {GeneratorID.Force,            "쾅"}, // 포쓰
        {GeneratorID.HeartBeat,        "쿵"}, // 하트비트
        {GeneratorID.LaserPointer,     "무기에서 레이저를 발사합니다."}, // 레이저 포인터
        {GeneratorID.MagneticField,    "자기장을 만들어 냅니다."}, //자기장
        {GeneratorID.OverLoad,         "과부화"}, // 과부화
        {GeneratorID.Reboot,           "리부트"}, // 리부트
        {GeneratorID.RotateWeapon,     "무기를 돌려 공격합니다."}, // 로테이트 웨폰
        {GeneratorID.SequenceAttack,   "연속 공격"}, // 시퀀스
        {GeneratorID.SiegeMode,        "시즈모드 가동"}, // 시즈모드
        {GeneratorID.Trinity,          "데미지 증가"}, //삼위일체
        {GeneratorID.WeaponShot,       "무기를 날려 공격합니다"}, // Weapon Shot
    };
    public static Dictionary<GeneratorID, TriggerID> triggerExplain = new()
    {
        {GeneratorID.None,            TriggerID.None },
        {GeneratorID.DashAttack, TriggerID.Dash},
        {GeneratorID.DeathRay, TriggerID.NormalAttack},
        {GeneratorID.Electronic, TriggerID.Dash},
        {GeneratorID.ErrorDice, TriggerID.CoolTime},
        {GeneratorID.Firecracker, TriggerID.RoomClear},
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
    public static Dictionary<GeneratorID, string> generatorName = new()
    {
        {GeneratorID.None,             "테스트" },
        {GeneratorID.DashAttack,       "대쉬 어택"}, // 대쉬 어택
        {GeneratorID.DeathRay,         "마관광샬포"}, // 마관광살포
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