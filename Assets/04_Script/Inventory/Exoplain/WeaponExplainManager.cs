using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID,string> weaponExplain = new();
    public static Dictionary<GeneratorID, TriggerID> triggerExplain = new();

    private void Awake()
    {
        #region 스킬 설명
        weaponExplain.Add(GeneratorID.DashAttack,       "대쉬 어택");
        weaponExplain.Add(GeneratorID.DeathRay,         "대쓰 레이");
        weaponExplain.Add(GeneratorID.Electronic,       "일렉트로닉");
        weaponExplain.Add(GeneratorID.ErrorDice,        "에러 다이스");
        weaponExplain.Add(GeneratorID.Force,            "포스");
        weaponExplain.Add(GeneratorID.HeartBeat,        "하트비트");
        weaponExplain.Add(GeneratorID.LaserPointer,     "레이저포인터");
        weaponExplain.Add(GeneratorID.MagneticField,    "자기장");
        weaponExplain.Add(GeneratorID.OverLoad,         "과부화");
        weaponExplain.Add(GeneratorID.Reboot,           "리부트");
        weaponExplain.Add(GeneratorID.RotateWeapon,     "로테이트 웨폰");
        weaponExplain.Add(GeneratorID.SequenceAttack,   "시퀀스어택");
        weaponExplain.Add(GeneratorID.SiegeMode,        "시즈모드");
        weaponExplain.Add(GeneratorID.Trinity,          "트포");
        weaponExplain.Add(GeneratorID.WeaponShot,       "무기샷");
        #endregion

        #region 트리거 설명
        triggerExplain.Add(GeneratorID.DashAttack,      TriggerID.Dash);
        triggerExplain.Add(GeneratorID.DeathRay,        TriggerID.NormalAttack);
        triggerExplain.Add(GeneratorID.ErrorDice,       TriggerID.Dash);
        triggerExplain.Add(GeneratorID.Electronic,      TriggerID.CoolTime);
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
