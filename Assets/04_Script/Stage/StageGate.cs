using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnGateEvent;
    public event Action OnMoveEndEvent;

    [field:SerializeField]
    public Stage NextStage { get; private set; }

    private bool _interactCheck = true;
    private StageTransition stageTransition;

    private PlayerController _playerController;

    [SerializeField]
    private Transform _spawnTweeningObject;

    [SerializeField]
    private bool _isPlayJumpAnim;

    private InventoryActive invenactive;


    private void Awake()
    {
        stageTransition = FindObjectOfType<StageTransition>();
        _playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        invenactive = FindObjectOfType<InventoryActive>();

        Sequence seq = DOTween.Sequence();
        transform.localScale = Vector3.zero;
        seq.Append(transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce));
        if (_spawnTweeningObject != null)
        {
            _spawnTweeningObject.localScale = Vector3.zero;
            seq.Join(_spawnTweeningObject.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce));
        }

        seq.AppendCallback(() =>
        {
            _interactCheck = false;
        });
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

        invenactive.canOpen = false;
        _playerController.ChangeState(EnumPlayerState.Idle);
        Transform playerTrm = GameManager.Instance.player;
        if (_isPlayJumpAnim)
        {
             
            Sequence seq = DOTween.Sequence();
            seq.Append(playerTrm.DOJump(transform.position + Vector3.down, 5f, 1, 1f));

            yield return new WaitForSeconds(0.8f);
        }
        GameManager.Instance.Inventory.SettingLineRender();


        stageTransition.StartTransition(1f);
        yield return new WaitForSeconds(2f);
        OnGateEvent?.Invoke();
        if (NextStage != null)
        {

            GameManager.Instance.PlayerTeleport(NextStage.playerSpawnPos);
            NextStage.SetGlobalLight();
            NextStage.SetCameraSize();

        }
        else
            GameManager.Instance.ResetGlobalLight();

        yield return new WaitForSeconds(1f);
        stageTransition.EndTransition(1f);
        yield return new WaitForSeconds(0.2f);
        _playerController.ChangeState(EnumPlayerState.Move);

        if(NextStage != null)
        {

            if (NextStage.ThisStageType == StageType.EnemyStage || NextStage.ThisStageType == StageType.BossStage)
                GameManager.Instance.isPlay = true;

            CameraManager.Instance.SetMinimapCameraPostion(NextStage.transform.position);
            NextStage.SetStageTitle();

        }

        yield return new WaitForSeconds(1f);

        EventTriggerManager.Instance?.RoomEnterExecute();
        
        if (NextStage != null)
        {
            NextStage?.StartWave();

        }

        OnMoveEndEvent?.Invoke();
        GameManager.Instance.Inventory.SettingLineRender();
        invenactive.canOpen = true;
    }
}
