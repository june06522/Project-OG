using FSM_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class TutorialPlayerController : PlayerController
{
    [HideInInspector]
    public bool canMove = false;
    protected override void Update()
    {
        if(canMove)
            base.Update();

    }
}
