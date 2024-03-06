using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

    Animator animator;

    readonly int loop = Animator.StringToHash("Loop");
    readonly int end = Animator.StringToHash("End");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLoop()
    {
        
        animator?.SetTrigger(loop);

    }
    
    public void SetEnd()
    {

        animator?.SetTrigger(end);

    }

    public void Destory()
    {

        Destroy(gameObject);

    }
}
