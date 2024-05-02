using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpenGuideQuest : GuideQuest
{
    private InventoryActive _inventoryActive;

    private void Awake()
    {
        _inventoryActive = FindObjectOfType<InventoryActive>();
    }

    public override bool IsQuestComplete()
    {
        if (_inventoryActive == null)
            return true;
        return _inventoryActive.IsOn;
    }
}
