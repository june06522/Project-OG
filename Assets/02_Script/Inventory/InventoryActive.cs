using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActive : MonoBehaviour
{
    [HideInInspector]
    public Image[] images;

    bool isOn = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            isOn = !isOn;
        }

        images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {

            image.enabled = isOn;
        }
    }
}
