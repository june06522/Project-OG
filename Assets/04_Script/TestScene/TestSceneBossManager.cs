using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneBossManager : MonoBehaviour
{
    public static TestSceneBossManager Instance;

    [Header("Boss")]
    [SerializeField] Stage triangle;
    [SerializeField] Stage square;
    [SerializeField] Stage pentagon;
    [SerializeField] Stage snake;
    [SerializeField] Stage crab;

    TestSceneStageTest stagetest;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : TestSceneBossManager is multiply running!");
            Destroy(gameObject);
        }

        stagetest = FindObjectOfType<TestSceneStageTest>();
    }

    public void Triangle() => stagetest.SettingStage(triangle);
    public void Square() => stagetest.SettingStage(square);
    public void Pentagon() => stagetest.SettingStage(pentagon);
    public void Snake() => stagetest.SettingStage(snake);
    public void Crab() => stagetest.SettingStage(crab);
}