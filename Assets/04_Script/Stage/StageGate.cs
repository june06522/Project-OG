using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour, IInteractable
{
    public event Action OnInteractEvent;
    public event Action OnGateEvent;
    public event Action OnMoveEndEvent;

    [SerializeField]
    private ItemType _gateItemType;
    [SerializeField]
    private List<GameObject> _itemTypeIcon;

    [field: SerializeField]
    public Stage NextStage { get; private set; }

    // Transition
    private bool _interactCheck = true;
    private StageTransition stageTransition;

    // Player
    private PlayerController _playerController;
    private InventoryActive invenactive;

    // Anim
    [SerializeField]
    private Transform _spawnTweeningObject;

    // BossGate Property
    [SerializeField]
    private bool _isPlayJumpAnim;
    private Collider2D _gateCollider;



    private void Awake()
    {
        _gateCollider = GetComponent<Collider2D>();
        stageTransition = FindObjectOfType<StageTransition>();
        _playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        invenactive = FindObjectOfType<InventoryActive>();

        // Tween
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

    public void SetStage(Stage nextStage, ItemType type)
    {
        NextStage = nextStage;
        _gateItemType = type;

        // Set Type Icon
        _itemTypeIcon[(int)type].SetActive(true);
    }
    public void SetStage(Stage nextStage)
    {
        NextStage = nextStage;
    }
    public void CheckInteract()
    {
        _interactCheck = true;
    }

    public void OnInteract()
    {
        if (_interactCheck || GameManager.Instance.InventoryActive.IsOn) return;

        _interactCheck = true;
        _gateCollider.enabled = false;

        StartCoroutine(GoNextStage());
    }

    IEnumerator GoNextStage()
    {
        EventTriggerManager.Instance.ResetTrigger();
        OnInteractEvent?.Invoke();
        // Next Stage
        invenactive.CanOpen = false;
        _playerController.ChangeState(EnumPlayerState.Idle);

        CameraManager.Instance.ResetCamera();

        // Transition
        stageTransition.StartTransition(1f);
        SoundManager.Instance.BgStop();

        // Jump Anim Sequence
        Transform playerTrm = GameManager.Instance.player;
        if (_isPlayJumpAnim)
        {

            yield return new WaitForSeconds(0.8f);

            Sequence seq = DOTween.Sequence();
            seq.Append(playerTrm.DOJump(transform.position + Vector3.down, 5f, 1, 1f));
            seq.AppendCallback(() =>
            {
                GameManager.Instance.Inventory.SettingLineRender();
            });

        }

        yield return new WaitForSeconds(2f);

        // Teleport
        OnGateEvent?.Invoke();
        if (NextStage != null)
        {

            GameManager.Instance.PlayerTeleport(NextStage.playerSpawnPos);
            NextStage.SetGlobalLight();
            NextStage.SetCameraSize();
            CameraManager.Instance.ResetCamera();

        }
        else
            GameManager.Instance.ResetGlobalLight();

        yield return new WaitForSeconds(1f);

        // Transition
        stageTransition.EndTransition(1f);
        if(NextStage != null)
        {
            SoundManager.Instance.BGMPlay(NextStage.ThisStageType); 
            NextStage.HandleStageStart();
        }

        yield return new WaitForSeconds(0.2f);
        _playerController.ChangeState(EnumPlayerState.Move);

        // Stage Setting
        if (NextStage != null)
        {
            NextStage.SetType(_gateItemType);
            if (NextStage.ThisStageType == StageType.EnemyStage || NextStage.ThisStageType == StageType.BossStage)
                GameManager.Instance.isPlay = true;

            NextStage.SetStageTitle();

        }

        // Delay
        yield return new WaitForSeconds(1f);
        stageTransition.CircleTransitionUI.SetOnOff(false);

        // Enter Room Event
        EventTriggerManager.Instance?.RoomClearExecute();

        if (NextStage != null)
        {
            NextStage?.StartWave();

        }

        OnMoveEndEvent?.Invoke();
        invenactive.CanOpen = true;
    }
}
