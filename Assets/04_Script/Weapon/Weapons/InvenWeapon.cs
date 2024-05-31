using System.Collections.Generic;
using UnityEngine;

public abstract class InvenWeapon : Weapon
{
    [SerializeField] protected AudioClip _attackSoundClip;

    public abstract void GetSignal(object signal);
}
