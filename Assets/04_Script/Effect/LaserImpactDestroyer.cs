using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserImpactDestroyer : MonoBehaviour
{
    public void EndOfAnimation()
    {
        Destroy(gameObject);
    }
}
