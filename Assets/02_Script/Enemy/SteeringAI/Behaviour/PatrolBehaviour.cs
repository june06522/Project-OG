using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : SteeringBehaviour
{
    float xCoord, yCoord;
    Vector2 startPoint;
    float patrolRadius;

    public PatrolBehaviour(Transform ownerTrm, float patrolRadius) : base(ownerTrm)
    {
        this.patrolRadius = patrolRadius;
    }

    public void SetStartPoint(Vector2 startPoint)
    {
        this.startPoint = startPoint;
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        float perlinValue = Mathf.PerlinNoise(Time.time, Time.time) * 2 - 1;
        Vector2 directionToTarget = new Vector2(perlinValue, perlinValue);
        for (int i = 0; i < interest.Length; i++)
        {
            float distance = Vector2.Distance((Vector2)transform.position 
                                           + Directions.eightDirections[i], startPoint);
            float weight = 0;
            if (distance > patrolRadius)
                weight = 1;

            float result = Vector2.Dot(directionToTarget, Directions.eightDirections[i]) - weight;
            Debug.Log($"Result {result}");
            //accept only directions at the less than 90 degrees to the target direction
            //if (result > 0)
            //{
            //    float valueToPutIn = result;
            //    if (valueToPutIn > interest[i])
            //    {
            //    }

            //}
            interest[i] = result;
        }
        return (danger, interest);
    }
}
