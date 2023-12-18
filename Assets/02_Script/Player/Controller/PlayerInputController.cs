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

    }

    private void CheckMovementKeyInput()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        MoveDir = new Vector2(x, y).normalized;

        if(MoveDir != Vector2.zero)
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
