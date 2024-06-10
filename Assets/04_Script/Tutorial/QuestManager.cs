using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum QuestName
{
    StartConversation = 0,
    PortalMove,
    UsePortal,
}

public class QuestManager : MonoSingleton<QuestManager>
{
    TutorialManager tutorialManager;
    TextMeshProUGUI npcText;

    float textTime = 0.1f;

    [Header("가이드 텍스트")]
    public string[] questTxt;
    public int questIndex = 0;

    private void Awake()
    {
        npcText = GameObject.Find("TutorialCanvas/Text/NpcText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        tutorialManager = TutorialManager.Instance;
    }

    public Action GetAction(int index)
    {
        return CallMethodByName(((QuestName)index).ToString());
    }

    private Action CallMethodByName(string methodName)
    {
        MethodInfo methodInfo = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (methodInfo != null)
        {
            return (Action)methodInfo.Invoke(this, null);
        }
        else
        {
            Debug.LogWarning($"Method {methodName} not found in {GetType().Name}");
            return null;
        }
    }

    private void SetString()
    {
        if (questIndex >= questTxt.Length)
            Debug.LogError($"{transform} : Index out of range!");

        SetStringEmpty();
        npcText.DOText(questTxt[questIndex], questTxt[questIndex].Length * textTime);
    }

    private void SetStringEmpty() => npcText.DOText("", 0f);

    public void StartConversation()
    {
        StartCoroutine(StartConverSationCo());
    }

    IEnumerator StartConverSationCo()
    {
        SetString();
        yield return new WaitForSeconds(questTxt[questIndex++].Length * textTime);
        tutorialManager.playerController.canMove = true;
        SetStringEmpty();
    }
}