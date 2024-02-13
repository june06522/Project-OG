using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActive : MonoBehaviour
{
    [HideInInspector]
    public GameObject inven;

    private void Awake()
    {
        inven = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        inven.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inven.SetActive(!inven.activeSelf);
        }
    }
}
