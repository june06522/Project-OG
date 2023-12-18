using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerRootState
{
    public PlayerDashState(PlayerController controller) : base(controller)
    {

        myColliders = transform.GetComponentsInChildren<Collider2D>();

    }

    private Vector2 dashEndPos;
    private Vector2 dashDir;
    private bool isDash;
    private Collider2D[] myColliders;

    protected override void EnterState()
    {

        var hitRay = Physics2D.Raycast(transform.position, inputController.LastMoveDir, playerData[PlayerStatsType.DashLenght], playerData.DashObstacleLayer);
        

        if (hitRay) 
        {

            var dashPos = hitRay.point - (inputController.LastMoveDir * 0.6f);

            var hitBox = Physics2D.OverlapBox(dashPos, new Vector2(1, 1), 0, playerData.DashObstacleLayer);

            if (!hitBox)
            {

                dashEndPos = dashPos;

            }
            else
            {


                isDash = false;
                controller.ChangeState(EnumPlayerState.Idle);
                return;


            }


        }
        else
        {


            var dashPos = (Vector2)transform.position + (inputController.LastMoveDir * playerData[PlayerStatsType.DashLenght]);

            var hitBox = Physics2D.OverlapBox(dashPos, new Vector2(1, 1), 0, playerData.DashObstacleLayer);

            if (hitBox)
            {

                dashEndPos = dashPos - (inputController.LastMoveDir / 2);

            }
            else
            {

                dashEndPos = dashPos;

            }

        }

        isDash = true;

        dashDir = inputController.LastMoveDir;
        rigid.velocity = inputController.LastMoveDir * playerData[PlayerStatsType.DashSpeed];

        foreach(var col in myColliders)
        {

            col.enabled = false;

        }

        eventController.OnDashExecute();

    }

    protected override void UpdateState()
    {

        var dir = (dashEndPos - (Vector2)transform.position).normalized;

        if(dir != dashDir || Vector2.Distance(transform.position, dashEndPos) < 0.1f)
        {

            rigid.velocity = Vector2.zero;
            controller.ChangeState(EnumPlayerState.Idle);

        }

    }

    protected override void ExitState()
    {

        if (isDash)
        {

            playerData.SetCoolDown(PlayerCoolDownType.Dash, playerData.DashCoolDown);
            transform.position = dashEndPos;

            foreach (var col in myColliders)
            {

                col.enabled = true;

            }

        }

        isDash = false;


    }

}
