using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAction<T> where T : Enum
{

    protected BaseFSM_Controller<T> controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;
    protected Rigidbody2D _rigidbody => controller.Enemy.Rigidbody;

    protected BaseAction(BaseFSM_Controller<T> controller)
    {
        this.controller = controller;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();

 
    public Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return controller.StartCoroutine(coroutine);
    }

    public void StopCoroutine(Coroutine coroutine)
    {
        if(coroutine != null)
            controller.StopCoroutine(coroutine);
    }

    protected IEnumerator DelayCor(float delay, Action afterAct, Action beforeAct = null)
    {
        beforeAct?.Invoke();
        yield return new WaitForSeconds(delay);
        afterAct?.Invoke();
    }
}
