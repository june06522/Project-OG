using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveClearEvent : QuestClearEvent
{
    [SerializeField]
    private GameObject _activeObject;
    [SerializeField]
    private bool _activeValue;

    protected override void ClearEvent()
    {
        _activeObject.SetActive(_activeValue);
    }
}
