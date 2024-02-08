using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SO/Enemy/Ungon")]
public class UngonDataSO : EnemyDataSO
{
    [field: Space]
    [field: Header("Á¡ÇÁ")]
    [field: SerializeField] public float JumpRange { get; private set; }
    [field: SerializeField] public float JumpPower { get; private set; }
    [field: SerializeField] public float JumpDuration { get; private set; }
    [field: SerializeField] public float JumpCoolDown { get; private set; }
    [field: SerializeField] public float LandBulletCount { get; private set; }

    [field: Space]
    [field: Header("ÃÑ¾Ë")]
    [field: SerializeField] public float FireCoolDown { get; private set; }
    [field: SerializeField] public float FireRange { get; private set; }


    public bool IsJumpCoolDown { get; private set; }
    public bool IsFireCoolDown { get; private set; }

    public void SetJumpCoolDown()
    {

        if (IsJumpCoolDown) return;
        IsJumpCoolDown = true;

        FAED.InvokeDelay(() =>
        {

            IsJumpCoolDown = false;

        }, JumpCoolDown);

    }

    public void SetFireCoolDown()
    {

        if (IsFireCoolDown) return;
        IsFireCoolDown = true;

        FAED.InvokeDelay(() =>
        {

            IsFireCoolDown = false;

        }, FireCoolDown);

    }
}
