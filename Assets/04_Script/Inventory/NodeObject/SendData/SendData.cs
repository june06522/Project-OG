using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SendData
{
    public SendData(GeneratorID generatorID, Transform triggerTrm, TriggerID triggerID, double targetCnt = 0, int power = 1, Weapon weapon = null)
    {
        this.generatorID = generatorID;
        this.power = power;
        this.triggerID = triggerID;
        trigger = triggerTrm;
        isVisited = new();
        index = EventTriggerManager.GetIndex();
        if (weapon != null)
            startWeapon = weapon;

        cnt = 0;
        this.targetCnt = targetCnt;
    }

    public virtual bool GetTrriger()
    {
        switch (triggerID)
        {
            case TriggerID.None:
            case TriggerID.Dash:
            case TriggerID.NormalAttack:
            case TriggerID.Move:
            case TriggerID.Idle:
            case TriggerID.RoomEnter:
            case TriggerID.GetHit:
            case TriggerID.StageClear:
            case TriggerID.WaveStart:
            case TriggerID.Always:
                return true;
            case TriggerID.CoolTime:
                cnt += Time.deltaTime;
                break;
            case TriggerID.Kill:
                cnt++;
                break;
            case TriggerID.UseSkill:
                cnt++;
                break;
        }
        if (cnt >= targetCnt)
        {
            cnt -= targetCnt;
            return true;
        }
        return false;
    }

    private GeneratorID generatorID;
    private TriggerID triggerID;
    private int power;
    public Dictionary<Vector2Int, int> isVisited;
    public Hashtable checkVisit = new();

    public ulong index = 0;
    public Transform trigger;

    public Weapon startWeapon;

    public GeneratorID GeneratorID => generatorID;
    public TriggerID TriggerID => triggerID;
    public int Power
    {
        get => power;
        set => power = value;
    }

    public double targetCnt = 0;
    private double cnt = 0;

}