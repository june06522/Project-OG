using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public int count = 0;

    public abstract void Excute(Transform owner, Transform target);
}
