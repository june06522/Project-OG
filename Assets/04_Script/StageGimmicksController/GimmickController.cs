using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _gimmicksList;

    [SerializeField] protected Stage _stage;

    protected bool _isEnded;

    protected virtual void Start()
    {
        _isEnded = false;
        _stage.OnStageClearEvent += Ended;
    }

    private void Ended()
    {
        _isEnded = true;
        foreach (GameObject obj in _gimmicksList)
        {
            obj.SetActive(false);
        }
    }
}
