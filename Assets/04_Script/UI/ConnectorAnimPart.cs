using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public class ConnectorAnimPart : MonoBehaviour
{
    [SerializeField] UIDirection _direction;

    Vector3 targetScale;
    Vector3 baseScale;
    private void Awake()
    {
        baseScale = transform.localScale;
        switch (_direction)
        {
            case UIDirection.UP:
            case UIDirection.DOWN:
                targetScale = transform.localScale + new Vector3(0, 1, 0);
                break;
            case UIDirection.LEFT:
            case UIDirection.RIGHT:
                targetScale = transform.localScale + new Vector3(0.7f, 0, 0);
                break;
        }
    }


    public void Animating(bool value)
    {
        transform.DOKill();
        if(value)
        {
            transform.DOScale(targetScale, 0.25f);
        }
        else
        {
            transform.DOScale(baseScale, 0.25f);
        }
    }


}
