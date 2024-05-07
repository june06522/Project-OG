using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashInvincibleGuideQuest : GuideQuest
{
    public Transform startPos;
    public Transform endPos;

    public List<GameObject> questObject;
    public TouchObject laser;

    private Transform _player;
    private PlayerHP _playerHP;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();

        laser.OnTouchEvent += ResetPlayer;
        _player = GameManager.Instance.player;

        if(startPos != null)
            _player.position = startPos.position;

        foreach(GameObject obj in questObject)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        
    }

    private void ResetPlayer()
    {
        if (_playerHP == null)
            _playerHP = _player.GetComponent<PlayerHP>();

        _playerHP.Hit(20);
        _playerHP.RestoreHP(20);
        _player.position = startPos.position;
    }

    public override bool IsQuestComplete()
    {
        bool endValue = Vector2.Distance(endPos.position, _player.position) < 1f;

        if (endValue)
        {

            foreach (GameObject obj in questObject)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

        }

        return endValue;
    }
}
