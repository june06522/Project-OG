using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEventReceiver : InventoryEventReceiverBase
{
    public GeneratorID generatorID;
    public int cnt = 1;
    private int curskill = 0;

    protected override void OnInit()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnSkill += UseSkill;

        }

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal(object parm)
    {

        base.GetSignal(parm);

    }

    private void UseSkill()
    {
        curskill++;
        if (curskill >= cnt)
        {
            curskill -= cnt;
            HandleSkill();
        }
    }

    private void HandleSkill()
    {

        SendData s = new SendData(generatorID, transform);

        GetSignal(s);

    }

    public override void Dispose()
    {
        if (PlayerController.EventController != null)
        {
            PlayerController.EventController.OnSkill -= UseSkill;

        }

    }
}