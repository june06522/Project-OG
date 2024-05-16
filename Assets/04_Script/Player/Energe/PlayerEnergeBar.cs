using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergeBar : MonoBehaviour
{
    Transform player;
    PlayerEnerge _playerEnerge;


    [SerializeField] RectTransform ui;
    [SerializeField] Image image;

    public Image Image => image;

    [SerializeField] Color ableColor;
    [SerializeField] Color disableColor;

    float targetAmount;

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
        image.fillAmount = Mathf.Lerp(targetAmount, image.fillAmount, 0.6f);

    }

    private void HandleUpdateEnergeUI(int maxEnerge, int currentEnerge)
    {

        targetAmount = (float)currentEnerge / (float)maxEnerge;

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
