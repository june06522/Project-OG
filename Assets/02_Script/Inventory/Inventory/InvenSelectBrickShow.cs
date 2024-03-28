using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSelectBrickShow : MonoBehaviour
{
    private InventoryRaycast invenRaycast;
    [SerializeField] Image _showUI;

    private void Awake()
    {
        if(_showUI ==  null)
        {
            Debug.LogError($"{transform} : _showUI is null!");
        }
    }

    private void LateUpdate()
    {
        
    }
}
