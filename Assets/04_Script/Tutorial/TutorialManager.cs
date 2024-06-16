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
    [HideInInspector] public InventoryActive inven;

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

    [Header("스테이지1")]
    public Transform portal1;
    public Image guideUI1;
    [HideInInspector]
    public Vector3 guidePos1 = new Vector3(-12, -100, 0);

    [Header("스테이지2")]
    public List<TutorialEnemyStateController> enemys2;
    public GameObject connecter2;
    public Transform portal2;
    public Image guideUI2;
    public Image guideUI3;
    public Image guideUI4;
    [HideInInspector]public Vector3 guide2Pos = new Vector3(295, -100);
    [HideInInspector]public Vector3 guide3Pos = new Vector3(195, -100);
    [HideInInspector]public Vector3 guide4Pos = new Vector3(-55, -100);

    [Header("스테이지3")]
    public List<TutorialEnemyStateController> enemys3;
    public Transform portal3;

    private void Awake()
    {
        player = GameManager.Instance.player;
        playerController = FindObjectOfType<TutorialPlayerController>();
        inven = FindObjectOfType<InventoryActive>();

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