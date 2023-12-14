using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatsType
{

    MoveSpeed

}

[CreateAssetMenu(menuName = "SO/Player/Data")]
public class PlayerDataSO : ScriptableObject
{

    [SerializeField] private Stats moveSpeed;

    private Dictionary<PlayerStatsType, Stats> playerStatsContainer;
    private PlayerController owner;

    public float MoveSpeed => moveSpeed.GetValue();

    public void SetOwner(PlayerController controller) 
    {
        
        owner = controller;
        playerStatsContainer.Add(PlayerStatsType.MoveSpeed, moveSpeed);

    }

    public void AddModify(PlayerStatsType type, float modifyValue, float duration = 1)
    {

        owner.StartCoroutine(ApplyModiyfyCo(type, modifyValue, duration));

    }

    private IEnumerator ApplyModiyfyCo(PlayerStatsType type, float modifyValue, float duration)
    {

        playerStatsContainer[type].AddModify(modifyValue);
        yield return new WaitForSeconds(modifyValue);
        playerStatsContainer[type].RemoveModify(modifyValue);

    }

}
