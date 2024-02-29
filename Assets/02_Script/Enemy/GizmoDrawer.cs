using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    public static GizmoDrawer Instance;

    public event Action GizmoAct;

    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    public void Add(Action act) => GizmoAct += act;
    public void Minus(Action act) => GizmoAct -= act;
    private void OnDrawGizmos()
    {
        GizmoAct?.Invoke();
    }
}
