using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum QuestName
{
    StartConversation = 0,
    PortalMove,
    UsePortal,
    UseDash,
    EquiementWeapon,
    KillEnemy,
    EatTrigger,
    ConnectSkill,
    UsePortal2,
    KillEnemys,
}

public class QuestManager : MonoSingleton<QuestManager>
{
    TutorialManager tutorialManager;
    TextMeshPro npcText;

    float textTime = 0.1f;
    float waitTextTime = 2f;

    [Header("가이드 텍스트")]
    [TextArea] public string[] questTxt;
    public int questIndex = 0;

    [Header("카메라")]
    public CinemachineVirtualCamera cmvcam;

    [Header("오브젝트")]
    public Transform player;
    public Transform animationObject;
    public Transform npc;
    public TutorialEnemyStateController enemy;

    [Header("NPC대화")]
    private bool isClick = false;
    private float curTime;
    private float delayTime;

    private void Awake()
    {
        npcText = GameObject.Find("NPC/talk/NpcText").GetComponent<TextMeshPro>();
        enemy = FindObjectOfType<TutorialEnemyStateController>();
    }

    private void Start()
    {
        player = GameManager.Instance.player;
        tutorialManager = TutorialManager.Instance;
        SetStringEmpty();

        tutorialManager.inven.tutoCanOpen = false;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && npcText.text != "")
        {
            isClick = true;
        }
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

        isClick = false;
        SetStringEmpty();
        tutorialManager.speechBubble.SetActive(true);
        npcText.DOText(questTxt[questIndex], questTxt[questIndex].Length * textTime);
    }

    private bool TextDelay()
    {
        if (isClick)
        {
            isClick = false;

            if (curTime + waitTextTime <= delayTime)
            {
                SkipText();
                curTime = delayTime - waitTextTime;
            }
            else
            {
                curTime = delayTime;
            }
        }

        if (curTime < delayTime)
        {
            return true;
        }
        else
        {
            SkipText();
            curTime = 0;
            return false;
        }
    }

    private void SkipText()
    {
        DOTween.Kill(npcText);
        npcText.text = questTxt[questIndex];
    }

    private void SetStringEmpty()
    {
        tutorialManager.speechBubble.SetActive(false);
        npcText.text = "";
    }

    //--------------------------------------

    public void StartConversation()
    {
        StartCoroutine(StartConverSationCo());
    }
    IEnumerator StartConverSationCo()
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < 5; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;

            SetStringEmpty();
        }

        cmvcam.Follow = animationObject;

        yield return new WaitForSeconds(2f);

        cmvcam.Follow = player;

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 2; i++)
        {

            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;

            SetStringEmpty();
        }

        npc.DOMoveX(27, 3f);
        yield return new WaitForSeconds(2.5f);
        tutorialManager.playerController.canMove = true;

        tutorialManager.NextQuest();
    }

    public void PortalMove()
    {
        StartCoroutine(PortalMoveCo());
    }
    IEnumerator PortalMoveCo()
    {
        while (Vector2.Distance(player.position, npc.position) >= 4f && player.position.x < npc.position.x)
        {
            yield return null;
        }

        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        player.position = npc.position + new Vector3(-3.58f, 0);
        tutorialManager.FadeOut(5f);
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 2; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;
        }

        tutorialManager.playerController.canMove = true;
        tutorialManager.NextQuest();
    }

    public void UsePortal()
    {
        StartCoroutine(UsePortalCo());
    }
    IEnumerator UsePortalCo()
    {
        while (Vector2.Distance(player.position, npc.position) <= 50f)
        {
            yield return null;
        }

        SetStringEmpty();
        npc.position = player.position + new Vector3(4.58f, 0);
        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        yield return new WaitForSeconds(1.5f);
        tutorialManager.playerController.SetRotation(Quaternion.identity);
        tutorialManager.playerController.Stop();
        PlayerController.InputController.LastMoveDir = (enemy.transform.position - player.position).normalized;

        yield return new WaitForSeconds(1.5f);
        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
        while (TextDelay()) { yield return null; }
        questIndex++;

        SetStringEmpty();

        enemy.Attack();
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 2; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;

            SetStringEmpty();
        }

        cmvcam.Follow = enemy.transform;
        yield return new WaitForSeconds(2f);
        cmvcam.Follow = player;
        yield return new WaitForSeconds(2f);

        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
        while (TextDelay()) { yield return null; }
        questIndex++;
        SetStringEmpty();

        enemy.Attack();
        tutorialManager.NextQuest();
    }

    public void UseDash()
    {
        StartCoroutine(UseDashCo());
    }
    IEnumerator UseDashCo()
    {
        TutorialEnemyBullet[] bullets = FindObjectsOfType<TutorialEnemyBullet>();
        yield return new WaitForSeconds(1.1f);
        foreach (var bullet in bullets)
        {
            bullet.SetSpeed(0f);
        }

        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay())
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield return null;
                isClick = true;
            }
            yield return null;
        }
        questIndex++;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        tutorialManager.playerController.SetEnergy();
        SetStringEmpty();

        PlayerController.dash.Enter();
        for (int i = 0; i < 20; i++)
        {
            PlayerController.dash.Update();
            yield return null;
        }
        PlayerController.dash.Exit();

        foreach (var bullet in bullets)
        {
            bullet.SetSpeed(12f);
        }

        tutorialManager.playerController.Stop();
        tutorialManager.NextQuest();
    }

    public void EquiementWeapon()
    {
        StartCoroutine(EquiementWeaponCo());
    }
    IEnumerator EquiementWeaponCo()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 5; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;
        }
        SetStringEmpty();
        tutorialManager.playerController.canMove = true;
        Instantiate(tutorialManager.weapon, tutorialManager.weaponSpawnPos.position, Quaternion.identity);

        WeaponController weapons = GameManager.Instance.player.GetComponent<WeaponController>();
        while (weapons.GetWeaponCnt() < 1)
        {
            yield return null;
        }

        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay()) { yield return null; }
        questIndex++;
        tutorialManager.NextQuest();
    }

    public void KillEnemy()
    {
        StartCoroutine(KillEnemyCo());
    }
    IEnumerator KillEnemyCo()
    {
        GameObject enemy = GameObject.Find("TutorialStage1/Enemy/TutorialEnemy");
        while (enemy != null && enemy.activeSelf) { yield return null; }

        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        for (int i = 0; i < 2; i++)
        {
            SetString();

            curTime = 0;
            if (i == 0)
                delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            else
                delayTime = questTxt[questIndex].Length * textTime;
            while (TextDelay()) { yield return null; }
            questIndex++;
        }

        GameObject.Find("TutorialStage1").GetComponent<Stage>().AppearChest();

        Transform chest = GameObject.Find("TutorialChest(Clone)").transform;
        chest.GetComponent<StageChest>().SetRateItems(ItemType.Generator);
        while (Vector3.Distance(chest.position, player.position) < 2f)
        {
            chest.Translate(Vector3.left);
        }

        tutorialManager.playerController.Stop();

        cmvcam.Follow = chest;
        yield return new WaitForSeconds(1.5f);
        cmvcam.Follow = player;
        yield return new WaitForSeconds(1.5f);

        tutorialManager.playerController.canMove = true;

        SetString();
        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay()) { yield return null; }
        questIndex++;

        while (!tutorialManager.isOpenChest) { yield return null; }
        tutorialManager.NextQuest();
    }

    public void EatTrigger()
    {
        StartCoroutine(EatTriggerCo());
    }
    IEnumerator EatTriggerCo()
    {
        for (int i = 0; i < 2; i++)
        {
            SetString();

            curTime = 0;
            if (i == 1)
                delayTime -= waitTextTime;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;
        }

        WeaponInventory inven = FindObjectOfType<WeaponInventory>();
        while (inven.GetContainerItemCnt() < 2) { yield return null; }
        yield return null;
        tutorialManager.playerController.Stop();
        tutorialManager.playerController.canMove = false;
        for (int i = 0; i < 3; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay()) { yield return null; }
            questIndex++;
        }

        tutorialManager.inven.tutoCanOpen = true;
        tutorialManager.NextQuest();
    }

    public void ConnectSkill()
    {
        StartCoroutine(ConnectSkillCo());
    }
    IEnumerator ConnectSkillCo()
    {
        InvenBrick[] invenBricks = FindObjectsOfType<InvenBrick>();
        WeaponInventory inven = FindObjectOfType<WeaponInventory>();
        Image trigger = null;
        Image weapon = null;

        foreach (var brick in invenBricks)
        {
            if (brick.Type == ItemType.Generator)
                trigger = brick.GetComponent<Image>();
            else
                weapon = brick.GetComponent<Image>();
        }

        tutorialManager.guideUI1.gameObject.SetActive(true);

        while (GameObject.Find("LineRenderer(Clone)") == null)
        {
            yield return null;
            trigger.raycastTarget = false;

            if (inven.GetContainerItemCnt() == 2)
            {
                if (weapon == null)
                    weapon = FindObjectOfType<WeaponBrick>().GetComponent<Image>();

                tutorialManager.guideUI1.transform.position = weapon.transform.position;
            }
            else
            {
                tutorialManager.guideUI1.rectTransform.anchoredPosition = tutorialManager.guidePos1;
            }
        }

        tutorialManager.guideUI1.gameObject.SetActive(false);

        invenBricks = FindObjectsOfType<InvenBrick>();
        while (!Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (var brick in invenBricks)
            {
                brick.GetComponent<Image>().raycastTarget = false;
            }
            yield return null;
        }
        tutorialManager.inven.tutoCanOpen = false;


        for (int i = 0; i < 2; i++)
        {
            SetString();

            curTime = 0;
            delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
            while (TextDelay())
            {
                yield return null;
            }
            questIndex++;
        }
        SetStringEmpty();

        tutorialManager.portal1.gameObject.SetActive(true);

        cmvcam.Follow = tutorialManager.portal1;
        yield return new WaitForSeconds(1.2f);
        cmvcam.Follow = tutorialManager.player;
        yield return new WaitForSeconds(1.2f);

        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay())
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
                isClick = true;
        }
        questIndex++;

        tutorialManager.playerController.canMove = true;
        tutorialManager.NextQuest();
    }

    public void UsePortal2()
    {
        StartCoroutine(UsePortal2Co());
    }
    IEnumerator UsePortal2Co()
    {
        while (Vector2.Distance(player.position, npc.position) <= 80f) { yield return null; }
        SetStringEmpty();
        npc.position = player.position + new Vector3(4.58f, 0);
        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        yield return new WaitForSeconds(1.5f);
        tutorialManager.playerController.SetRotation(Quaternion.identity);
        tutorialManager.playerController.Stop();
        PlayerController.InputController.LastMoveDir = Vector2.up;

        tutorialManager.NextQuest();
    }

    public void KillEnemys()
    {
        StartCoroutine(KillEnemysCo());
    }
    IEnumerator KillEnemysCo()
    {
        SetString();
        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
        while (TextDelay())
        {
            yield return null;
        }
        questIndex++;
        SetStringEmpty();

        cmvcam.Follow = tutorialManager.enemys2[1];
        yield return new WaitForSeconds(1.5f);
        cmvcam.Follow = player;
        yield return new WaitForSeconds(1.5f);

        SetString();
        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay())
        {
            yield return null;
            if(Input.GetMouseButtonDown(0))
                isClick = true;
        }
        questIndex++;
    }
}