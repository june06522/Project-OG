using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Inventory/ItemInfoListSO")]
public class ItemInfoListSO : ScriptableObject
{
    public List<ItemInfoSO> ItemInfoList;
}
