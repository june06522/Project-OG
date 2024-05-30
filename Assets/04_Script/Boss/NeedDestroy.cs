using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedDestroy : MonoBehaviour
{
    void Start()
    {
        Object.FindAnyObjectByType<Boss>().DieEvt += DestroyThis;
    }

    private void DestroyThis()
    {
        gameObject.SetActive(false);
    }
}
