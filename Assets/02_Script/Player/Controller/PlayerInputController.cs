using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TwoDirInput(Vector2 dir);
public delegate void OneDirInput(float value);

public class PlayerInputController : IDisposable
{

    public event Action OnDashKeyPressed;
    public Vector2 MoveDir { get; private set; }
    public Vector2 LastMoveDir { get; private set; } = Vector2.right;
    public bool isDashKeyPressed { get; private set; }

    public void Update()
    {

        CheckMovementKeyInput();
        CheckDashKey();
        CheckChestKey();

    }

    private void CheckChestKey()
    {
        Vector2 pos = GameManager.Instance.player.position;
        float radius = 3f;
        Collider2D col = Physics2D.OverlapCircle(pos, radius, LayerMask.GetMask("Chest"));
        Chest chest;
        if(col.TryGetComponent<Chest>(out chest))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                chest.Open();
            }
        }

    }

    private void CheckMovementKeyInput()
    {
        float x = 0;// = Input.GetAxisRaw("Horizontal");
        float y = 0;// = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(KeyCode.W))
            y += 1;
        if (Input.GetKey(KeyCode.S))
            y -= 1;
        if (Input.GetKey(KeyCode.D))
            x += 1;
        if (Input.GetKey(KeyCode.A))
            x -= 1;


        MoveDir = new Vector2(x, y).normalized;

        if (MoveDir != Vector2.zero)
        {

            LastMoveDir = MoveDir;

        }

    }


    private void CheckDashKey()
    {

        isDashKeyPressed = Input.GetKeyDown(KeyCode.Space);

        if (isDashKeyPressed)
        {

            OnDashKeyPressed?.Invoke();

        }

    }

    public void Dispose()
    {

        OnDashKeyPressed = null;

    }

}
