using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeFeedback : Feedback
{
    [SerializeField]
    private float _shakeValue = 5f;
    [SerializeField]
    private float _shakeTime = 0.2f;

    public override void Play(float damage)
    {
        StartCoroutine(DelayCo());
        CameraManager.Instance.CameraShake(_shakeValue, _shakeTime);
    }

    IEnumerator DelayCo()
    {
        GameManager.Instance.InventoryActive.CanOpen = false;
        yield return new WaitForSeconds(_shakeTime);
        GameManager.Instance.InventoryActive.CanOpen = true;
    }
}
