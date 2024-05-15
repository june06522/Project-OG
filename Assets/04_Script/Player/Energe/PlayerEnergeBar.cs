using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergeBar : MonoBehaviour
{
    Transform player;
    PlayerEnerge _playerEnerge;

    [SerializeField] RectTransform ui;
    [SerializeField] Image image;

    [SerializeField] Color ableColor;
    [SerializeField] Color disableColor;

    private void Start()
    {
        _playerEnerge = GameManager.Instance.player.GetComponent<PlayerEnerge>();
        _playerEnerge.OnChangeEnergeEvent += HandleUpdateEnergeUI;
        HandleUpdateEnergeUI(_playerEnerge.MaxEnerge, _playerEnerge.CurrentEnerge);
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        ui.transform.position = Camera.main.WorldToScreenPoint(player.position);
    }

    private void HandleUpdateEnergeUI(int maxEnerge, int currentEnerge)
    {
        image.fillAmount = (float)currentEnerge / (float)maxEnerge;
        if (image.fillAmount < 0.2f)
        {
            image.color = disableColor;
        }
        else
        {
            image.color = ableColor;
        }
    }

}
