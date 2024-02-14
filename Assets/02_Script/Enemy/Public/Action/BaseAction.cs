using System;
using System.Collections;
using UnityEngine;

public abstract class BaseAction<T> where T : Enum
{

    protected BaseFSM_Controller<T> controller;
    protected EnemyDataSO _data => controller.EnemyData;

    protected BaseAction(BaseFSM_Controller<T> controller)
    {
        this.controller = controller;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

 
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
