using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerGraveStone : BreakableObject, IStageObject
{
    public Stage Stage { get; set; }

    [SerializeField]
    private List<Transform> _spawnList = new List<Transform>();
    [SerializeField]
    private MonsterSpawnInfo _monsterSpawnInfo;

    protected override void BrakingObject()
    {
        Stage.DiscountMonster();


        Destroy(gameObject, 1f);
    }

    public bool Get_IsNeedRemove() => true;
}
