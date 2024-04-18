using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CheckConnectorUse : MonoBehaviour
{
    private bool isConnectorUse;
    public bool IsConnectorUse 
    {
        get { return isConnectorUse; }
        set
        {
            isConnectorUse = value;
            if(group != null)
            {
                group.Animating(value);
            }
        }
    }
    ConnectorPartGroup group;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            IsConnectorUse = !IsConnectorUse;
        }
    }

    private void Awake()
    {
        isConnectorUse = false;
        group = GetComponent<ConnectorPartGroup>();
    }
}
