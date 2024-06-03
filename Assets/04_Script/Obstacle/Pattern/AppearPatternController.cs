using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearPatternController : MonoBehaviour
{
    Stage stage;

    [SerializeField] float patternDelay = 1f;

    AppearPattern[] patterns;
    
    Coroutine patternCoroutine;
    AppearPattern curPattern;

    private void Awake()
    {
        stage = GetComponent<Stage>();
    }

    private void Start()
    {
        stage.OnStageStartEvent += StartPattern;
        stage.OnStageClearEvent += EndPattern;

        patterns = transform.Find("Patterns").GetComponentsInChildren<AppearPattern>();

        StartPattern();
    }

    private void StartPattern()
    {
        if (patternCoroutine != null)
            StopCoroutine(patternCoroutine);
        patternCoroutine = StartCoroutine(PlayPattern());
    }

    private void EndPattern()
    {
        if(patternCoroutine != null)
            StopCoroutine(patternCoroutine);
    }

    IEnumerator PlayPattern()
    {
        while(true)
        {
            curPattern = ChoosePattern();
            curPattern.HandleStartPattern();
            yield return new WaitUntil(() => curPattern.IsEnd);
            curPattern.Init();
            yield return new WaitForSeconds(patternDelay);
        }
    }

    private AppearPattern ChoosePattern()
    {
        return patterns[UnityEngine.Random.Range(0, patterns.Length)];
    }

    private void OnDestroy()
    {
        EndPattern();
    }
}
