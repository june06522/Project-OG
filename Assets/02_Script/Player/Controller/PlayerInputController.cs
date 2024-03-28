using System;
using UnityEngine;

public delegate void TwoDirInput(Vector2 dir);
public delegate void OneDirInput(float value);

public class PlayerInputController : IDisposable
{
    public ParticleSystem Particle;

    public event Action OnDashKeyPressed;
    public Vector2 MoveDir { get; private set; }
    public Vector2 LastMoveDir { get; private set; } = Vector2.right;
    public bool isDashKeyPressed { get; private set; }

    private Vector3 _lastNearObjPos = Vector3.zero;
    public GameObject _interactUI { get; private set; }
    private bool _isDetectIntractObj = false;

    private PlayerEnerge _playerEnerge;

    public void Update(Rigidbody2D rb2D)
    {

        CheckMovementKeyInput(rb2D);
        CheckDashKey();
        CheckInteractable();

    }

    public void SetInteractUI(GameObject interactUI)
    {
        _interactUI = interactUI;
    }
    public void SetPlayerEnerge(PlayerEnerge playerEnerge)
    {
        _playerEnerge = playerEnerge;
    }

    private void CheckInteractable()
    {
        Vector2 pos = GameManager.Instance.player.position;
        float radius = 2f;
        Collider2D[] col = Physics2D.OverlapCircleAll(pos, radius, LayerMask.GetMask("Interactable"));

        // if not detect interact object, return
        if (col.Length == 0 && _isDetectIntractObj == true)
        {
            _isDetectIntractObj = false;
            if (_interactUI != null)
            {
                _interactUI.SetActive(false);
            }
            return;
        }
        else if (col.Length == 0) return;

        // near interact Object
        Collider2D nearObject = col[0];
        float nearObjectDistance = Vector2.Distance(pos, nearObject.gameObject.transform.position);
        float curObjDistance = 0f;
        for (int i = 1; i < col.Length; i++)
        {
            curObjDistance = Vector2.Distance(pos, col[i].gameObject.transform.position);
            if (curObjDistance < nearObjectDistance)
            {
                nearObject = col[i];
                nearObjectDistance = curObjDistance;
            }
        }

        Vector3 uiPos = nearObject.transform.position;
        uiPos.y = nearObject.bounds.max.y + 0.2f;

        if (_interactUI != null && (_isDetectIntractObj == false || uiPos != _lastNearObjPos))
        {
            _lastNearObjPos = uiPos;
            _isDetectIntractObj = true;

            _interactUI.SetActive(true);
            _interactUI.transform.position = uiPos;
        }

        // interact
        if (Input.GetKeyDown(KeyCode.F))
        {
            IInteractable interact;
            if (nearObject.TryGetComponent<IInteractable>(out interact))
            {
                interact.OnInteract();
            }
        }
    }

    private void CheckMovementKeyInput(Rigidbody2D rb2D)
    {
        float x = 0;// = Input.GetAxisRaw("Horizontal");
        float y = 0;// = Input.GetAxisRaw("Vertical");

        if (!GameManager.Instance.InventoryActive.IsOn && !GameManager.Instance.isShopOpen)
        {
            if (Input.GetKey(KeyCode.W))
                y += 1;
            if (Input.GetKey(KeyCode.S))
                y -= 1;
            if (Input.GetKey(KeyCode.D))
                x += 1;
            if (Input.GetKey(KeyCode.A))
                x -= 1;
        }


        MoveDir = new Vector2(x, y).normalized;

        if (MoveDir != Vector2.zero)
        {

            LastMoveDir = MoveDir;

        }

    }


    private void CheckDashKey()
    {

        isDashKeyPressed = Input.GetKeyDown(KeyCode.Space) && !GameManager.Instance.InventoryActive.IsOn;

        // if (isDashKeyPressed && _playerEnerge.ConsumeEnerge(10))
        if (isDashKeyPressed)
        {
            //var module = Particle.main;
            //module.startLifetime = 0.3f;

            OnDashKeyPressed?.Invoke();

        }

    }

    public void Dispose()
    {

        OnDashKeyPressed = null;

    }

}
