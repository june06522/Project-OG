using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnGateEvent;
    public event Action OnMoveEndEvent;
    public Stage NextStage {  get; private set; }

    private bool _interactCheck;
    private StageTransition stageTransition;

    private void Awake()
    {
        stageTransition = FindObjectOfType<StageTransition>();
    }

    //test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnInteract();
    }

    public void SetStage(Stage nextStage)
    {
        NextStage = nextStage;
    }

    public void OnInteract()
    {
        if (_interactCheck || GameManager.Instance.InventoryActive.IsOn) return;
        
        _interactCheck = true;

        StartCoroutine(GoNextStage());
    }

    IEnumerator GoNextStage()
    {
        stageTransition.StartTransition();
        yield return new WaitForSeconds(0.2f);
        if (NextStage != null)
        {

            GameManager.Instance.player.position = NextStage.playerSpawnPos;
            NextStage.SetGlobalLight();
            NextStage.SetCameraSize();

        }
        else
            GameManager.Instance.ResetGlobalLight();
        GameManager.Instance.InventoryActive.isPlaying = true;
        OnGateEvent?.Invoke();
        yield return new WaitForSeconds(0.5f);
        stageTransition.EndTransition();
        yield return new WaitForSeconds(1f);

        if (NextStage != null)
        {
            NextStage?.StartWave();

        }

        OnMoveEndEvent?.Invoke();
    }
}