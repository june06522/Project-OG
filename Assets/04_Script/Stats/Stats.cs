using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{

    [SerializeField] private float value;

    private List<float> modifys = new();

    public float GetValue()
    {

        float mod = value;

        foreach (var item in modifys)
        {

            mod += item;

        }

        return mod;

    }

    public void AddModify(float value)
    {
        
        modifys.Add(value);

    }

    public void RemoveModify(float value)
    {

        modifys.Remove(value);

    }

}
