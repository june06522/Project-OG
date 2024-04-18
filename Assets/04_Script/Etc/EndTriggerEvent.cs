using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTriggerEvent : MonoBehaviour
{
    public virtual void EndEvent() =>
        Destroy(this.gameObject);
}
