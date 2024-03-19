using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SendData
{
    public SendData(WeaponType wepaonType, GeneratorID generatorID)
    {
        this.weaponType = wepaonType;
        this.generatorID = generatorID;
        power = 1;
        isVisited = new();
    }

    private WeaponType weaponType;
    private GeneratorID generatorID;
    private int power;
    public Dictionary<Vector2Int, int> isVisited;
    
    public WeaponType WeaponType => weaponType;
    public GeneratorID GeneratorID => generatorID;
    public int Power
    {
        get => power;
        set => power = value;
    }
}
