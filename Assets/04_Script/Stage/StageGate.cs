using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnGateEvent;
    public event Action OnMoveEndEvent;
    public Stage NextStage {  get; private set; }

    private bool _interactCheck = true;
    private StageTransition stageTransition;

    private PlayerController _playerController;

    [SerializeField]
    private Transform _spawnTweeningObject;

    [SerializeField]
    private bool _isPlayJumpAnim;


    private void Awake()
    {
        stageTransition = FindObjectOfType<StageTransition>();
        _playerController = GameManager.Instance.player.GetComponent<PlayerController>();

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
        _playerController.ChangeState(EnumPlayerState.Idle);

        if(_isPlayJumpAnim)
        {
            Transform playerTrm = GameManager.Instance.player;

            Sequence seq = DOTween.Sequence();
            seq.Append(playerTrm.DOJump(transform.position + Vector3.down, 5f, 1, 1f));

            yield return new WaitForSeconds(0.8f);
        }



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
        _playerController.ChangeState(EnumPlayerState.Move);

        if(NextStage != null)
        {

            CameraManager.Instance.SetMinimapCameraPostion(NextStage.transform.position);

        }

        stageTransition.EndTransition();
        yield return new WaitForSeconds(1f);

        EventTriggerManager.Instance.RoomEnterExecute();
        
        if (NextStage != null)
        {
            NextStage?.StartWave();

        }

        OnMoveEndEvent?.Invoke();
    }
}
