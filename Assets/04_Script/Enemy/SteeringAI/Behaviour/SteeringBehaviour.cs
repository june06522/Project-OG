using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SteeringBehaviour
{
    public Transform transform;

    public SteeringBehaviour(Transform ownerTrm)
    {
        this.transform = ownerTrm;
    }

    public abstract (float[] danger, float[] interest) 
        GetSteering(float[] danger, float[] interest, AIData aiData);

    public abstract void OnDestroy();

}
public static class Directions
{
    public static List<Vector2> eightDirections = new List<Vector2>{
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized
    };
}
