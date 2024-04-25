using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID, string> weaponExplain = new();
    public static Dictionary<GeneratorID, TriggerID> triggerExplain = new();

    private void Awake()
    {
        #region 스킬 설명
        weaponExplain.Add(GeneratorID.None,            "테스트용입니다.");

        weaponExplain.Add(GeneratorID.DashAttack,       "대쉬는 훌륭한 공격 수단입니다."); // 대쉬 어택
        weaponExplain.Add(GeneratorID.DeathRay,         "죽음의 광선"); // 마관광살포
        weaponExplain.Add(GeneratorID.Electronic,       "일렉 뚜리뚜리"); // 전기 지짐이
        weaponExplain.Add(GeneratorID.ErrorDice,        "@#!%&($@1@$q!!"); // 에러다이스
        weaponExplain.Add(GeneratorID.Force,            "쾅"); // 포쓰
        weaponExplain.Add(GeneratorID.HeartBeat,        "쿵"); // 하트비트
        weaponExplain.Add(GeneratorID.LaserPointer,     "전방에 레이저를 발사 합니다."); // 레이저 포인터
        weaponExplain.Add(GeneratorID.MagneticField,    "자기장을 만들어 냅니다."); //자기장
        weaponExplain.Add(GeneratorID.OverLoad,         "과부화"); // 과부화
        weaponExplain.Add(GeneratorID.Reboot,           "리부트"); // 리부트
        weaponExplain.Add(GeneratorID.RotateWeapon,     "무기를 돌려 공격합니다."); // 로테이트 웨폰
        weaponExplain.Add(GeneratorID.SequenceAttack,   "시퀀스어택"); // 시퀀스
        weaponExplain.Add(GeneratorID.SiegeMode,        "이동할 필요가 없다면 이동하지 않아도 된다."); // 시즈모드
        weaponExplain.Add(GeneratorID.Trinity,          "트포"); //삼위일체
        weaponExplain.Add(GeneratorID.WeaponShot,       "이 대신 잇몸으로"); // Weapon Shot
        #endregion

        #region 트리거 설명
        triggerExplain.Add(GeneratorID.None,      TriggerID.None);
        
        triggerExplain.Add(GeneratorID.DashAttack,      TriggerID.Dash);
        triggerExplain.Add(GeneratorID.DeathRay,        TriggerID.NormalAttack);
        triggerExplain.Add(GeneratorID.Electronic,      TriggerID.Dash);
        triggerExplain.Add(GeneratorID.ErrorDice,       TriggerID.CoolTime);
        triggerExplain.Add(GeneratorID.Force,           TriggerID.NormalAttack);
        triggerExplain.Add(GeneratorID.HeartBeat,       TriggerID.Dash);
        triggerExplain.Add(GeneratorID.LaserPointer,    TriggerID.Move);
        triggerExplain.Add(GeneratorID.MagneticField,   TriggerID.NormalAttack);
        triggerExplain.Add(GeneratorID.OverLoad,        TriggerID.Dash);
        triggerExplain.Add(GeneratorID.Reboot,          TriggerID.Dash);
        triggerExplain.Add(GeneratorID.RotateWeapon,    TriggerID.Move);
        triggerExplain.Add(GeneratorID.SequenceAttack,  TriggerID.Dash); // 얜 미정
        triggerExplain.Add(GeneratorID.SiegeMode,       TriggerID.Idle);
        triggerExplain.Add(GeneratorID.Trinity,         TriggerID.NormalAttack);
        triggerExplain.Add(GeneratorID.WeaponShot,      TriggerID.CoolTime);
        #endregion
    }
}
