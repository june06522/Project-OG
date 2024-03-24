using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBossDeadEffect : BossEffect
{
    protected override void OnDisable()
    {
        ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarDeadEffect, this.gameObject);
    }
}
