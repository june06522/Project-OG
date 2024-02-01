using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActiveFalse : MonoBehaviour
{
    [SerializeField] private GameObject inven;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inven.SetActive(!inven.activeSelf);
        }
    }
}
