using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    [HideInInspector]
    public Image[] images;

    public bool canOpen = true;
    bool isOn = false;
    public bool IsOn => isOn;

    private void Update()
    {
        if(canOpen)
        {
            if(Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.Escape) && isOn))
            {
                isOn = !isOn;
            }
        }

        images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {

            image.enabled = isOn;
        }
    }
}
