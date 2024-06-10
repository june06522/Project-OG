using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("플레이어 관련")]
    Transform player;
    [HideInInspector] public TutorialPlayerController playerController;

    [Header("애니메이션 관련")]
    [SerializeField] Transform BlackImage;

    [Header("퀘스트 관련")]
    QuestManager questManager;
    public int index = 0;
    public bool isClearQuest = false;

    private void Awake()
    {
        player = GameManager.Instance.player;
        playerController = FindObjectOfType<TutorialPlayerController>();
    }

    private void Start()
    {
        questManager = QuestManager.Instance;
        NextQuest();
    }

    private void NextQuest() => questManager.GetAction(index++);
}