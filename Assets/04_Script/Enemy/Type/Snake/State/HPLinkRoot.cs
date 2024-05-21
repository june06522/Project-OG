using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPLinkRoot : MonoBehaviour
{

    // Link
    private void Awake()
    {
        IHitAble root = GetComponent<IHitAble>();
        if (root == null)
            return;

        var hpLinkObjects = transform.GetComponentsInChildren<HPLinkObject>();
        foreach(var hpLink in hpLinkObjects)
        {
            hpLink.Link(root);
        }
    }

}
