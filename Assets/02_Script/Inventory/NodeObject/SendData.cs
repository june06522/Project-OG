using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SendData : MonoBehaviour
{
    public SendData(WeaponType wepaonType, GeneratorID generatorID)
    {
        this.weaponType = wepaonType;
        this.generatorID = generatorID;
        power = 1;
    }

    private WeaponType weaponType;
    private GeneratorID generatorID;
    private int power;

    public WeaponType WeaponType => weaponType;
    public GeneratorID GeneratorID => generatorID;
    public int Power
    {
        get => power;
        set => power = value;
    }
}
