using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChildRotator : MonoBehaviour
{
    private List<Transform> children = new List<Transform>();

    [SerializeField]
    private float _rotateSpeed = 30f;

    private void OnEnable()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i).GetComponent<Transform>();
            if (child != null)
            {
                children.Add(child);
            }
        }
        

        if (children.Count > 0 && children[0] == transform)
            children.RemoveAt(0);


    }

    private void Update()
    {

        RotateChildren();
        
    }

    private void RotateChildren()
    {
        for(int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];
            Vector2 dir = child.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x);
            float dist = Vector2.Distance(transform.position, child.position);

            angle = angle + (_rotateSpeed * Time.deltaTime * Mathf.Deg2Rad);
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * dist;

            child.position = transform.position + offset;

        }
    }
}
