using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SynergyData
{
    public TriggerID type;
    public List<float> table = new List<float>();
}

public class SynergyManager : MonoBehaviour
{

    private static SynergyManager instance;
    public static SynergyManager Instance => instance;

    public Action OnSynergyChange;

    // 시너지 별 스탯 테이블
    [SerializeField] List<SynergyData> tableList = new List<SynergyData>();

    // 해당 시너지 레벨
    Dictionary<TriggerID, int> synergyLevel = new Dictionary<TriggerID, int>();

    // 현재 시너지 적용치
    Dictionary<PlayerStatsType, float> synergyAmount = new Dictionary<PlayerStatsType, float>();
    public Dictionary<PlayerStatsType, float> SynergyAmount => synergyAmount;

    private void Awake()
    {

        if (instance != null)
        {

            Debug.LogError("Multiple SynergyManager is running");
            Destroy(instance);


        }

        instance = this;

        foreach (TriggerID id in Enum.GetValues(typeof(TriggerID)))
        {
            synergyLevel[id] = 0;
            UpdateSynergyAmount(id);
        }

    }

    public void EquipItem(TriggerID id)
    {

        synergyLevel[id]++;
        UpdateSynergyAmount(id);

    }

    public void RemoveItem(TriggerID id)
    {

        if (synergyLevel.ContainsKey(id))
        {

            if (synergyLevel[id] != 0)
            {

                synergyLevel[id]--;

            }

        }

        UpdateSynergyAmount(id);

    }

    public void UpdateSynergyAmount(TriggerID id)
    {

        PlayerStatsType targetStat = PlayerStatsType.None;
        switch (id)
        {
            case TriggerID.None:
                break;
            case TriggerID.Dash:
                targetStat = PlayerStatsType.RegenEnergePerSec;
                break;
            case TriggerID.NormalAttack:
                targetStat = PlayerStatsType.AttackSpeed;
                break;
            case TriggerID.CoolTime:
                targetStat = PlayerStatsType.Cooltime;
                break;
            case TriggerID.Move:
                targetStat = PlayerStatsType.MoveSpeed;
                break;
            case TriggerID.Idle:
                targetStat = PlayerStatsType.Defence;
                break;
            case TriggerID.RoomClear:
                targetStat = PlayerStatsType.EarningGold;
                break;
            case TriggerID.GetHit:
                targetStat = PlayerStatsType.Bloodsucking;
                break;
            case TriggerID.Kill:
                targetStat = PlayerStatsType.Damage;
                break;
            case TriggerID.UseSkill:
                break;
            case TriggerID.StageClear:
                targetStat = PlayerStatsType.EarningGold;
                break;
            case TriggerID.WaveStart:
                break;
            case TriggerID.Always:
                break;
            default:
                break;
        }

        if (targetStat != PlayerStatsType.None)
        {

            synergyAmount[targetStat] = tableList.FirstOrDefault((x) => x.type == id).table[synergyLevel[id]];
            OnSynergyChange?.Invoke();

        }

    }

}