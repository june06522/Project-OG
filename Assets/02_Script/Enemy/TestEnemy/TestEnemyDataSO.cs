using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Enemy/TestEnemy")]
public class TestEnemyDataSO : EnemyDataSO
{
    [field: Space]
    [field: Header("점프")]
    [field: SerializeField] public float JumpRange { get; private set; }
    [field: SerializeField] public float JumpPower { get; private set; }
    [field: SerializeField] public float JumpDuration { get; private set; }
    [field: SerializeField] public float JumpCoolDown { get; private set; }
    [field: SerializeField] public float LandBulletCount { get; private set; }

    [field: Space]
    [field: Header("대쉬")]
    [field: SerializeField] public float DashCoolDown { get; private set; }
    [field: SerializeField] public float DashRange { get; private set; }
    [field: SerializeField] public float DashSpeed { get; private set; }
    [field: SerializeField] public float DashBulletCount { get; private set; }


    public bool IsJumpCoolDown { get; private set; }
    public bool IsDashCoolDown { get; private set; }

    public void SetJumpCoolDown()
    {

        if (IsJumpCoolDown) return;
        IsJumpCoolDown = true;

        FAED.InvokeDelay(() =>
        {

            IsJumpCoolDown = false;

        }, JumpCoolDown);

    }

    public void SetDashCoolDown()
    {
        if (IsDashCoolDown) return;
        IsDashCoolDown = true;

        FAED.InvokeDelay(() =>
        {

            IsDashCoolDown = false;

        }, DashCoolDown);

    }
}
