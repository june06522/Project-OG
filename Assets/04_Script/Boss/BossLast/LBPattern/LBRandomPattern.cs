using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LBRandomPattern : RandomPattern
{

    [SerializeField] protected Animator _bossAnimator;
    [SerializeField] protected Boss _boss;

    private int _bossAnimIsSmileHash = Animator.StringToHash("IsSmile");
    private int _bossAnimIsPowerHash = Animator.StringToHash("IsPower");

    public void SetSmile(bool value)
    {
        _bossAnimator.SetBool(_bossAnimIsSmileHash, value);
    }

    public void SetPower(bool value)
    {
        _bossAnimator.SetBool(_bossAnimIsPowerHash, value);
    }


}
