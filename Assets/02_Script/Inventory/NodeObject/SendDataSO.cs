using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SendData")]
public class SendDataSO : ScriptableObject
{
    [SerializeField]
    private WeaponType weaponType;
    [SerializeField]
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
