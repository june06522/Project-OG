using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : InvenWeapon
{
    public override void Attack(Transform target)
    {
        // ��ġ ������ && ��ƼŬ ��ȯ�ؼ� �ǰ�ó�� ���ֱ�
    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        var data = (SendData)signal;

        if (sendDatas == null)
        {
            sendDatas = data;
        }
        else
        {
            sendDatas = sendDatas.Power > data.Power ? sendDatas : data;
        }

    }




}
