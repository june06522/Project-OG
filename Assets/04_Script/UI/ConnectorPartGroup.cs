using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectorPartGroup : MonoBehaviour
{
    List<ConnectorAnimPart> animParts;
    public bool OnConnecting { get; set; }

    private void Awake()
    {
        animParts = GetComponentsInChildren<ConnectorAnimPart>().ToList();
    }

    public void Animating(bool value)
    {
        animParts.ForEach((part) =>
        {
            part.Animating(value);
        });
    }
}
