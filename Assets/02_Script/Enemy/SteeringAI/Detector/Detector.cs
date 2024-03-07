using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detector
{
    public Transform transform;
    public Detector(Transform ownerTrm)
    {
        transform = ownerTrm;
    }

    public abstract void Detect(AIData aiData);
}
