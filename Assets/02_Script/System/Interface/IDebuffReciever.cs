using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EDebuffType
{
    None = 0,
    Frozen = 1 << 0,
    Burn = 1 << 1,
    Poison = 1 << 2,
}

public interface IDebuffReciever
{
    public EDebuffType  DebuffType { get; set; }
    public float    DebuffCoolTime { get; set; }

    public void SetDebuff(EDebuffType debuffType, float coolTime)
    {
        this.DebuffType = debuffType;
        this.DebuffCoolTime = coolTime;
    }

    void DebuffEffect(EDebuffType debuffType, float coolTime);
    void DisposeDebuff();
}
