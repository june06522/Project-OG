using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver
{
    Vector2 resultDirection;

    public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        //Loop through each behaviour
        foreach (SteeringBehaviour behaviour in behaviours)
        {
            float[] tempDanger = new float[8];
            float[] tempInterest = new float[8];

            (tempDanger, tempInterest) = behaviour.GetSteering(danger, interest, aiData);

            for (int i = 0; i < 8; i++)
            {
                danger[i] += tempDanger[i];
                interest[i] += tempInterest[i];
            }
        }

        //foreach (SteeringBehaviour behaviour in behaviours)
        //{
        //    (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
        //}


        //subtract danger values from interest array
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        //get the average direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }

        outputDirection.Normalize();

        resultDirection = outputDirection;

        //return the selected movement direction
        return resultDirection;
    }
}
