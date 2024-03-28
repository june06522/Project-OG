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

    [SerializeField] GameObject _playerUI;
    [SerializeField] GameObject _weaponExplainPanel;
    public bool IsOn => isOn;

    private void Update()
    {
        if(canOpen)
        {
            if(Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && isOn))
            {
                isOn = !isOn;
                if(isOn)
                    ShowInven();
                else
                    ShowUI();
                _weaponExplainPanel.SetActive(isOn);
                _playerUI.SetActive(!isOn);
            }
        }

        images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {

            image.enabled = isOn;
        }
    }

    private void ShowInven()
    {

    }

    private void ShowUI()
    {

    }
}
