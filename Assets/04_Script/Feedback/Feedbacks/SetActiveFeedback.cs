using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public struct SetActiveFeedbackValueInfo
{
    [Header("Info")]
    public List<GameObject> targets;
    public bool value;

    [Header("Delay")]
    public float time;
}

public class SetActiveFeedback : Feedback
{
    [Header("Info")]
    public List<SetActiveFeedbackValueInfo> values;

    public override void Play(float damage)
    {

        foreach(SetActiveFeedbackValueInfo info in values)
        {
            if (info.targets.Count <= 0)
                continue;

            if(info.time <= 0)
            {

                for (int i = 0; i < info.targets.Count; ++i)
                {
                    info.targets[i].SetActive(info.value);
                }

            }
            else
            {

                FAED.InvokeDelay(() =>
                {
                    for(int i = 0; i < info.targets.Count; ++i)
                    {
                        info.targets[i].SetActive(info.value);
                    }
                    
                }, info.time);

            }
        }
        
        
    }

    
}
