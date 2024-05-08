using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SendData
{
    public SendData( GeneratorID generatorID,Transform triggerTrm, int power = 1,Weapon weapon = null)
    {
        this.generatorID = generatorID;
        this.power = power;
        trigger = triggerTrm;
        isVisited = new();
        index = EventTriggerManager.GetIndex();

        if(weapon != null )
            startWeapon = weapon;
    }

    private GeneratorID generatorID;
    private int power;
    public Dictionary<Vector2Int, int> isVisited;
    public Hashtable checkVisit = new();

    public ulong index = 0;
    public Transform trigger;

    public Weapon startWeapon;
   
    public GeneratorID GeneratorID => generatorID;
    public int Power
    {
        get => power;
        set => power = value;
    }
}
