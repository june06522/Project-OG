using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindEffect : MonoBehaviour
{
    public void EndAnim()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
