using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public abstract class RandomPattern : MonoBehaviour
{

    private List<Action> _patternList = new List<Action>();
    private int _lastIndex = -1;

    protected bool _isEnd = false;
    public bool IsEnd => _isEnd;

    public abstract void OnPattern();

    public abstract void OffPattern();

    public void Play()
    {
        if (_patternList.Count == 0)
            return;

        _isEnd = false;

        if (_patternList.Count == 1)
        {
            _patternList[0]?.Invoke();
            return;
        }

        // 중복 제거
        List<int> indexList = new List<int>();
        for(int i = 0; i <  _patternList.Count; i++)
        {
            if (_lastIndex == i)
                continue;

            indexList.Add(i);
        }

        _lastIndex = indexList[Random.Range(0, indexList.Count)];
        _patternList[_lastIndex]?.Invoke();

    }

    protected void RegisterPattern(Action func)
    {

        _patternList.Add(func);

    }
    
}
