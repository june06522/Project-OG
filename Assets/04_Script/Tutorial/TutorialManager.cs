using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("플레이어 관련")]
    public Transform player;
    [HideInInspector] public TutorialPlayerController playerController;

    [Header("애니메이션 관련")]
    [SerializeField] Image blackImage;
    public GameObject speechBubble;
    private float fadeOutTime = 5f;

    [Header("퀘스트 관련")]
    public int index = 0;
    public bool isClearQuest = false;
    QuestManager questManager;
    public bool isOpenChest = false;

    [Header("아이템 관련")]
    public GameObject weapon;
    public Transform weaponSpawnPos;

    private void Awake()
    {
        player = GameManager.Instance.player;
        playerController = FindObjectOfType<TutorialPlayerController>();

        blackImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        questManager = QuestManager.Instance;
        FadeOut(fadeOutTime);
        NextQuest();
    }

    public void FadeOut(float time)
    {
        blackImage.DOFade(1, 0f);
        blackImage.DOFade(0, time);
    }

    public void NextQuest() => questManager.GetAction(index++);
}