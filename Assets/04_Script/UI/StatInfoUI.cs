using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _statInfoText;

    SynergyManager _synergeManager;

    private void Awake()
    {
        _synergeManager = SynergyManager.Instance;
    }

    private void OnEnable()
    {

        SetStatInfoUI();
    }

    private void SetStatInfoUI()
    {

        StringBuilder statSB = new StringBuilder();

        TriggerID[] triggerInfoOrder = {
            TriggerID.Dash,
            TriggerID.NormalAttack,
            TriggerID.CoolTime,
            TriggerID.Move,
            TriggerID.Idle,
            TriggerID.StageClear,
            TriggerID.Kill };

        string[] triggerStatText = {
            "에너지 재생속도 증가",
            "공격속도 증가",
            "쿨타임 감소",
            "이동속도 증가",
            "데미지 감소",
            "골드 획득량 증가",
            "공격력 증가" };


        // Synerge
        for (int i = 0; i < triggerInfoOrder.Length; i++)
        {

            statSB.Append(GetSynergeInfoText(triggerInfoOrder[i]));

        }

        statSB.Append("\n");

        // Current Plus Stat
        for (int i = 0; i < triggerInfoOrder.Length; i++)
        {

            statSB.Append($"{triggerStatText[i]} - ");
            statSB.Append(GetStatInfoText(triggerInfoOrder[i]));

        }

        _statInfoText.text = statSB.ToString();

    }

    private string GetSynergeInfoText(TriggerID id)
    {

        return $"{WeaponExplainManager.triggerName[id]} - {(_synergeManager.level.ContainsKey(id) ? _synergeManager.level[id] : 0)}\n";

    }
    private string GetStatInfoText(TriggerID id)
    {

        return $"{_synergeManager.GetStatFactor(id) * 100f}%\n";

    }

}
