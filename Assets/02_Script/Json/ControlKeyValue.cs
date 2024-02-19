using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlKeyValue : MonoBehaviour
{
    public 

    private void Start()
    {
        transform.name = DataManager.Instance.keyData.left.ToString();
    }
}
