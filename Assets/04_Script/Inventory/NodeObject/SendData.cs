using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SendData
{
    public SendData( GeneratorID generatorID, int power = 1)
    {
        this.generatorID = generatorID;
        this.power = power;
        isVisited = new();
    }

    private GeneratorID generatorID;
    private int power;
    public Dictionary<Vector2Int, int> isVisited;
    public Hashtable checkVisit = new();
   
    public GeneratorID GeneratorID => generatorID;
    public int Power
    {
        get => power;
        set => power = value;
    }
}