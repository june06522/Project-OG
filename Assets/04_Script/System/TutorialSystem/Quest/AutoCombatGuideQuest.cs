using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatGuideQuest : GuideQuest
{
    [Header("AutoCombat Info")]
    public Sandbag sandbag;
    public Transform playerStartPos;
    public int hitCount = 3;
    public int checkDamage = 0;

    private bool _isEndQuest = false;
    private int _currentHitCount = 0;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();
        GameManager.Instance.player.position = playerStartPos.position;
        sandbag.gameObject.SetActive(true);
        sandbag.OnHit += HandleHit;
    }

    private void HandleHit(float damage)
    {
        _currentHitCount++;
        if(_currentHitCount >= hitCount && damage >= checkDamage)
        {
            _isEndQuest = true;
            sandbag.OnHit -= HandleHit;
            sandbag.gameObject.SetActive(false);
        }
    }

    public override bool IsQuestComplete()
    {
        return _isEndQuest;
    }
}
