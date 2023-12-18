using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatsType
{

    MoveSpeed,
    DashCoolDown

}

public enum PlayerCoolDownType
{

    Dash,

}

[CreateAssetMenu(menuName = "SO/Player/Data")]
public class PlayerDataSO : ScriptableObject
{

    [SerializeField] private Stats moveSpeed;
    [SerializeField] private Stats dashCoolDown;

    private Dictionary<PlayerStatsType, Stats> playerStatsContainer = new();
    private Dictionary<PlayerCoolDownType, bool> playerCoolDownContainer = new();
    private PlayerController owner;

    public float MoveSpeed => moveSpeed.GetValue();
    public float DashCoolDown => dashCoolDown.GetValue();

    public bool this[PlayerCoolDownType type]
    {

        get 
        { 

            return playerCoolDownContainer[type];
            
        }

    }

    public float this[PlayerStatsType type]
    {

        get
        {

            return playerStatsContainer[type].GetValue();

        }

    }

    public void SetOwner(PlayerController controller) 
    {
        
        owner = controller;
        playerStatsContainer.Add(PlayerStatsType.MoveSpeed, moveSpeed);
        playerStatsContainer.Add(PlayerStatsType.DashCoolDown, dashCoolDown);

    }

    public void SetCoolDown(PlayerCoolDownType type, float duration)
    {

        if (playerCoolDownContainer[type] == true) return;

        owner.StartCoroutine(SetCoolDownCo(type, duration));

    }

    public void AddModify(PlayerStatsType type, float modifyValue, float duration = 1)
    {

        owner.StartCoroutine(ApplyModiyfyCo(type, modifyValue, duration));

    }

    private IEnumerator ApplyModiyfyCo(PlayerStatsType type, float modifyValue, float duration)
    {

        playerStatsContainer[type].AddModify(modifyValue);
        yield return new WaitForSeconds(duration);
        playerStatsContainer[type].RemoveModify(modifyValue);

    }

    private IEnumerator SetCoolDownCo(PlayerCoolDownType type, float duration)
    {

        playerCoolDownContainer[type] = true;
        yield return new WaitForSeconds(duration);
        playerCoolDownContainer[type] = false;

    }

}
