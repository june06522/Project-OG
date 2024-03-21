using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Boss/BossBulletData")]
public class BossBulletDataSO : ScriptableObject
{
    public float Damage;
    public float DestoryTime;
    public bool IfHitWillBreak;
    public string[] HitAbleTag;
}
