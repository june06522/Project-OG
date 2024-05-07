using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneStageTest : MonoBehaviour
{
    Stage stage;

    public void SettingStage(Stage stage)
    {
        this.stage = stage;
    }

    public void PlayStage()
    {
        Instantiate(stage,transform);
    }
}
