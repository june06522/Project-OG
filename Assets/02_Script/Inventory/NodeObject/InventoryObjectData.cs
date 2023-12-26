using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Inventory/Object")]
public class InventoryObjectData : ScriptableObject
{

    public List<Vector2> bricks = new List<Vector2>();
    [HideInInspector] public List<InventoryObjectRoot> includes = new();

    public void Init(Transform owner)
    {

        for(int i  = 0; i < includes.Count; i++)
        {

            includes[i] = includes[i].Copy();

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].ResetConnect(includes);

        }

        for (int i = 0; i < includes.Count; i++)
        {

            includes[i].Init(owner);

        }

    }

}
