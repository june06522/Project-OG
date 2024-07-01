using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LBRandomPattern : RandomPattern
{

    [SerializeField] protected Animator _bossAnimator;
    [SerializeField] protected Boss _boss;

    private int _bossAnimIsSmileHash = Animator.StringToHash("IsSmile");
    private int _bossAnimIsPowerHash = Animator.StringToHash("IsPower");
    private int _bossAnimIsDieHash = Animator.StringToHash("IsDie");

    public void SetSmile(bool value)
    {
        _bossAnimator.SetBool(_bossAnimIsSmileHash, value);
    }

    public void SetPower(bool value)
    {
        _bossAnimator.SetBool(_bossAnimIsPowerHash, value);
    }

    public void SetDie(bool value)
    {
        _bossAnimator.SetBool(_bossAnimIsDieHash, value);
    }


}
