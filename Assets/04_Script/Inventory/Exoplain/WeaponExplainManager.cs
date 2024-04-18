using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExplainManager : MonoBehaviour
{
    public static Dictionary<GeneratorID,string> weaponExplain = new();

    private void Awake()
    {
        weaponExplain.Add(GeneratorID.DashAttack,       "");
        weaponExplain.Add(GeneratorID.DeathRay,         "");
        weaponExplain.Add(GeneratorID.Electronic,       "");
        weaponExplain.Add(GeneratorID.ErrorDice,        "");
        weaponExplain.Add(GeneratorID.Force,            "");
        weaponExplain.Add(GeneratorID.HeartBeat,        "");
        weaponExplain.Add(GeneratorID.LaserPointer,     "");
        weaponExplain.Add(GeneratorID.MagneticField,    "");
        weaponExplain.Add(GeneratorID.OverLoad,         "");
        weaponExplain.Add(GeneratorID.Reboot,           "");
        weaponExplain.Add(GeneratorID.RotateWeapon,     "");
        weaponExplain.Add(GeneratorID.SequenceAttack,   "");
        weaponExplain.Add(GeneratorID.SiegeMode,        "");
        weaponExplain.Add(GeneratorID.Trinity,          "");
        weaponExplain.Add(GeneratorID.WeaponShot,       "");
    }
}
