using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneStageTest : MonoBehaviour
{
    Stage stage;
    Transform player;

    Vector3 stagePos = new Vector3(0,500,0);

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    public void SettingStage(Stage stage)
    {
        if(this.stage != null)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        PlayStage(stage);
    }

    public void PlayStage(Stage stage)
    {
        this.stage = Instantiate(stage,stagePos,Quaternion.identity,transform);
        player.position = this.stage.playerSpawnPos ;
        this.stage.StartWave();
    } 
}