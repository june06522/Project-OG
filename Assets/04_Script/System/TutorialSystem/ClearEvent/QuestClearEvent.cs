using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestClearEvent : MonoBehaviour
{

    private void Awake()
    {
        GuideQuest ownerQuest = GetComponent<GuideQuest>();

        if( ownerQuest != null )
        {
            ownerQuest.OnQuestComplete += ClearEvent;
        }

    }

    protected abstract void ClearEvent();

}
