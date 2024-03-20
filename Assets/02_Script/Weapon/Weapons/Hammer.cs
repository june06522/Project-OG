using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : InvenWeapon
{
    public override void Attack(Transform target)
    {
        // 망치 돌리기 && 파티클 소환해서 피격처리 해주기
    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        var data = (SendData)signal;
        SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

    }




}
