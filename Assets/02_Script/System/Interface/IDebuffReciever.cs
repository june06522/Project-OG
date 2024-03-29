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

    public void SetDebuff(EDebuffType debuffType)
    {
        this.DebuffType = debuffType;
        DebuffEffect(debuffType);
    }

    void DebuffEffect(EDebuffType debuffType);
    void DisposeDebuff();

    IEnumerator DebuffCor(float coolTime)
    {
        yield return new WaitForSeconds(DebuffCoolTime);
        DisposeDebuff();
    }

}
