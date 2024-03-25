using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergeUI : MonoBehaviour
{
    PlayerEnerge _playerEnerge;

    [Header("PlayerEnerge UI Info")]
    [SerializeField]
    TextMeshProUGUI _energeText;
    [SerializeField]
    Slider _energeSlider;

    private void Start()
    {
        _playerEnerge = GameManager.Instance.player.GetComponent<PlayerEnerge>();
        _playerEnerge.OnChangeEnergeEvent += HandleUpdateEnergeUI;
        HandleUpdateEnergeUI(_playerEnerge.MaxEnerge, _playerEnerge.CurrentEnerge);
    }

    private void HandleUpdateEnergeUI(int maxEnerge, int currentEnerge)
    {
        _energeText.text = $"{currentEnerge}/{maxEnerge}";
        _energeSlider.value = currentEnerge / (float)maxEnerge;
    }
}
