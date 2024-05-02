using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneTriggerManager : MonoBehaviour
{
    public static TestSceneTriggerManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : TestSceneTriggerManager is multiply running!");
            Destroy(gameObject);
        }
    }

    public void RoomEnterExecute() => EventTriggerManager.Instance.RoomEnterExecute();
    public void StageClearExecute() => EventTriggerManager.Instance.StageClearExecute();
    public void WaveStartExecute() => EventTriggerManager.Instance.WaveStartExecute();
}