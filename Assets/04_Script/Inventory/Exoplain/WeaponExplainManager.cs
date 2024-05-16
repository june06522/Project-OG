using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID, string> weaponExplain = new()
    {
        {GeneratorID.None,            "테스트용입니다." },
        {GeneratorID.DashAttack,       "대쉬는 훌륭한 공격 수단입니다."}, // 대쉬 어택
        {GeneratorID.DeathRay,         "죽음의 광선"}, // 마관광살포
        {GeneratorID.Electronic,       "일렉 뚜리뚜리"}, // 전기 지짐이
        {GeneratorID.ErrorDice,        "@#!%&($@1@$q!!"}, // 에러다이스
        {GeneratorID.Firecracker,      "축포를 터트린다"}, // 축포
        {GeneratorID.Force,            "쾅"}, // 포쓰
        {GeneratorID.HeartBeat,        "쿵"}, // 하트비트
        {GeneratorID.LaserPointer,     "전방에 레이저를 발사 합니다."}, // 레이저 포인터
        {GeneratorID.MagneticField,    "자기장을 만들어 냅니다."}, //자기장
        {GeneratorID.OverLoad,         "과부화"}, // 과부화
        {GeneratorID.Reboot,           "리부트"}, // 리부트
        {GeneratorID.RotateWeapon,     "무기를 돌려 공격합니다."}, // 로테이트 웨폰
        {GeneratorID.SequenceAttack,   "시퀀스어택"}, // 시퀀스
        {GeneratorID.SiegeMode,        "이동할 필요가 없다면 이동하지 않아도 된다."}, // 시즈모드
        {GeneratorID.Trinity,          "트포"}, //삼위일체
        {GeneratorID.WeaponShot,       "이 대신 잇몸으로"}, // Weapon Shot
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
}
