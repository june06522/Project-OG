using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetRouteBehaviour : SteeringBehaviour
{
    float[] interestsTemp;
    public GetRouteBehaviour(Transform ownerTrm) : base(ownerTrm)
    {
        GizmoDrawer.Instance.Add(OnDrawGizmos);
    }

    ~GetRouteBehaviour()
    {
        GizmoDrawer.Instance.Remove(OnDrawGizmos);
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (aiData.PathData.IsExist() == true)
        {
            Vector2 getNextRouteDirNormalized = (aiData.PathData.GetFirst() - (Vector2)transform.position).normalized;
            Debug.Log(getNextRouteDirNormalized);
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector2.Dot(getNextRouteDirNormalized, Directions.eightDirections[i]);
                interest[i] += result;
            }
            interestsTemp = interest;
        }
        else
            interestsTemp = null;
        return (danger, interest);
    }

    public override void OnDestroy()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (transform == null) return;
       
        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                }
            }
        }
    }
}
