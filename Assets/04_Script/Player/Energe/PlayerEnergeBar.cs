using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergeBar : MonoBehaviour
{
    Transform player;
    PlayerEnerge _playerEnerge;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Material mat;

    public SpriteRenderer Sprite => sprite;

    [SerializeField] Color ableColor;
    [SerializeField] Color disableColor;

    float targetAmount;

    private void Start()
    {
        _playerEnerge = GameManager.Instance.player.GetComponent<PlayerEnerge>();
        _playerEnerge.OnChangeEnergeEvent += HandleUpdateEnergeUI;
        HandleUpdateEnergeUI(_playerEnerge.MaxEnerge, _playerEnerge.CurrentEnerge);
        player = GameManager.Instance.player;

        mat.SetFloat("_Fill", 1);
        mat.SetColor("_Color", new Color(ableColor.r, ableColor.g, ableColor.b));
    }

    private void Update()
    {

        //ui.transform.position = Camera.main.WorldToScreenPoint(player.position);
        mat.SetFloat("_Fill", Mathf.Lerp(targetAmount, mat.GetFloat("_Fill"), 0.6f));

    }

    private void HandleUpdateEnergeUI(int maxEnerge, int currentEnerge)
    {

        targetAmount = (float)currentEnerge / (float)maxEnerge;

        if (mat.GetFloat("_Fill") < 0.2f)
        {

            mat.SetColor("_Color", new Color(disableColor.r, disableColor.g, disableColor.b));

        }
        else
        {

            mat.SetColor("_Color", new Color(ableColor.r, ableColor.g, ableColor.b));

        }

    }

}
