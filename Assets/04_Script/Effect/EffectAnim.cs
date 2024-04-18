using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnim : MonoBehaviour
{
    public virtual void OnEndEvent()
    {
        Destroy(this.gameObject);
    }
}
