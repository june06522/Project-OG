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
    EatConnecter,
    ConnectConnecter,
    UsePortal3,
    KillEnemys2,
    UsePortal4,
}

public class QuestManager : MonoSingleton<QuestManager>
{
    TutorialManager tutorialManager;
    TextMeshPro npcText;

    float textTime = 0.07f;
    float waitTextTime = 2f;
    [Header("카메라 이동 사운드")]
    public AudioSource cameraMoveSound;

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

        tutorialManager.inven.TutoCanOpen = false;
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
        tutorialManager.textAudio.Play();
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
        if(curTime > delayTime - waitTextTime)
            tutorialManager.textAudio.Stop();
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
        yield return new WaitForSeconds(2.5f);
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
        cameraMoveSound.Play();

        yield return new WaitForSeconds(2f);

        cmvcam.Follow = player;
        cameraMoveSound.Play();

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
        yield return new WaitForSeconds(2f);
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
        cameraMoveSound.Play();
        yield return new WaitForSeconds(2f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
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
        for (int i = 0; i < 40; i++)
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

        SetStringEmpty();
        if(Vector3.Distance(npc.position, player.position) > 12f)
        {
            tutorialManager.FadeOut(5f);
            player.position = npc.position;
            player.transform.Translate(Vector3.left * 6f);
            yield return new WaitForSeconds(2f);

        }

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
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.5f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
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

        tutorialManager.inven.TutoCanOpen = true;
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
        tutorialManager.inven.TutoCanOpen = false;


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
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.2f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
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

        foreach (var enemy in tutorialManager.enemys2)
            enemy.gameObject.SetActive(true);
        tutorialManager.NextQuest();
    }

    public void KillEnemys()
    {
        StartCoroutine(KillEnemysCo());
    }
    IEnumerator KillEnemysCo()
    {
        yield return new WaitForSeconds(1f);

        SetString();
        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
        while (TextDelay())
        {
            yield return null;
        }
        questIndex++;
        SetStringEmpty();

        cmvcam.Follow = tutorialManager.enemys2[1].transform;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.5f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.5f);

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
        foreach (var enemy in tutorialManager.enemys2)
            enemy.AttackLoop();

        while (tutorialManager.enemys2.Count > 0)
        {
            yield return null;
            for (int i = 0; i < tutorialManager.enemys2.Count; i++)
                if (tutorialManager.enemys2[i] == null)
                    tutorialManager.enemys2.Remove(tutorialManager.enemys2[i]);
        }

        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();

        SetStringEmpty();
        if (Vector3.Distance(npc.position, player.position) > 12f)
        {
            tutorialManager.FadeOut(5f);
            player.position = npc.position;
            player.transform.Translate(Vector3.left * 6f);
            yield return new WaitForSeconds(2f);

        }

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
        tutorialManager.connecter2.gameObject.SetActive(true);

        cmvcam.Follow = tutorialManager.connecter2.transform;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1f);

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

    public void EatConnecter()
    {
        StartCoroutine(EatConnecterCo());
    }
    IEnumerator EatConnecterCo()
    {
        WeaponInventory inven = FindObjectOfType<WeaponInventory>();
        while (inven.GetContainerItemCnt() < 3) { yield return null; }

        yield return null;
        tutorialManager.playerController.Stop();
        tutorialManager.playerController.canMove = false;

        SetStringEmpty();
        if (Vector3.Distance(npc.position, player.position) > 12f)
        {
            tutorialManager.FadeOut(5f);
            player.position = npc.position;
            player.transform.Translate(Vector3.left * 6f);
            yield return new WaitForSeconds(2f);

        }

        for (int i = 0; i < 3; i++)
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

        ExpansionManager.Instance.AddTutorial();
        tutorialManager.inven.TutoCanOpen = true;
        tutorialManager.NextQuest();
    }

    public void ConnectConnecter()
    {
        StartCoroutine(ConnectConnecterCo());
    }
    IEnumerator ConnectConnecterCo()
    {
        WeaponInventory inven = FindObjectOfType<WeaponInventory>();
        InvenBrick[] invenBricks = FindObjectsOfType<InvenBrick>();
        Image weapon = null, connecter = null, trigger = null;

        foreach (var brick in invenBricks)
        {
            if (brick.Type == ItemType.Generator)
                trigger = brick.GetComponent<Image>();
            else if (brick.Type == ItemType.Weapon)
                weapon = brick.GetComponent<Image>();
            else if (brick.Type == ItemType.Connector)
                connecter = brick.GetComponent<Image>();
        }

        //칸확장
        tutorialManager.guideUI2.gameObject.SetActive(true);
        tutorialManager.guideUI2.rectTransform.anchoredPosition = tutorialManager.guide2Pos;
        while (ExpansionManager.Instance.leftCnt > 0)
        {
            weapon.raycastTarget = false;
            connecter.raycastTarget = false;
            trigger.raycastTarget = false;
            yield return null;
        }
        tutorialManager.guideUI2.gameObject.SetActive(false);

        //무기 이동
        weapon.raycastTarget = true;
        tutorialManager.guideUI3.gameObject.SetActive(true);
        while (weapon == null || weapon.GetComponent<InvenBrick>().InvenObject.originPos != new Vector2Int(3, 1))
        {
            if (weapon == null)
            {
                invenBricks = FindObjectsOfType<InvenBrick>();
                foreach (var brick in invenBricks)
                {
                    if (brick.Type == ItemType.Weapon)
                        weapon = brick.GetComponent<Image>();
                }
            }
            if (inven.GetContainerItemCnt() == 2)
                tutorialManager.guideUI3.rectTransform.anchoredPosition = tutorialManager.guide3Pos;
            else
                tutorialManager.guideUI3.transform.position = weapon.transform.position;

            connecter.raycastTarget = false;
            trigger.raycastTarget = false;
            yield return null;
        }
        tutorialManager.guideUI3.gameObject.SetActive(false);
        
        //부품 이동
        connecter.raycastTarget = true;
        tutorialManager.guideUI4.gameObject.SetActive(true);
        while (GameObject.Find("LineRenderer(Clone)") == null || GameObject.Find("LineRenderer(Clone)").GetComponent<LineRenderer>().positionCount < 3)
        {
            if (connecter == null)
            {
                invenBricks = FindObjectsOfType<InvenBrick>();
                foreach (var brick in invenBricks)
                {
                    if (brick.Type == ItemType.Connector)
                        connecter = brick.GetComponent<Image>();
                }
            }
            if(inven.GetContainerItemCnt() == 2)
                tutorialManager.guideUI4.rectTransform.anchoredPosition = tutorialManager.guide4Pos;
            else
                tutorialManager.guideUI4.transform.position = connecter.transform.position;

            weapon.raycastTarget = false;
            trigger.raycastTarget = false;
            yield return null;
        }
        tutorialManager.guideUI4.gameObject.SetActive(false);

        //닫기
        while (!Input.GetKeyDown(KeyCode.Tab))
        {
            weapon.raycastTarget = false;
            connecter.raycastTarget = false;
            trigger.raycastTarget = false;
            yield return null;
        }
        tutorialManager.inven.TutoCanOpen = false;

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

        tutorialManager.portal2.gameObject.SetActive(true);

        cmvcam.Follow = tutorialManager.portal2;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.2f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
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

    public void UsePortal3()
    {
        StartCoroutine(UsePortal3Co());
    }
    IEnumerator UsePortal3Co()
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

        foreach (var enemy in tutorialManager.enemys2)
            enemy.gameObject.SetActive(true);



        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        SetString();

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime + waitTextTime;
        while (TextDelay())
        {
            yield return null;
        }
        questIndex++;

        foreach (var enemy in tutorialManager.enemys3)
            enemy.gameObject.SetActive(true);

        cmvcam.Follow = tutorialManager.enemys3[2].transform;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.5f);
        cmvcam.Follow = player;
        cameraMoveSound.Play();
        yield return new WaitForSeconds(1.5f);

        curTime = 0;
        delayTime = questTxt[questIndex].Length * textTime;
        while (TextDelay())
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
                isClick = true;
        }
        questIndex++;
        tutorialManager.NextQuest();
    }

    public void KillEnemys2()
    {
        StartCoroutine(KillEnemys2Co());
    }
    IEnumerator KillEnemys2Co()
    {
        tutorialManager.playerController.canMove = true;
        foreach (var enemy in tutorialManager.enemys3)
            enemy.AttackLoop();

        while (tutorialManager.enemys3.Count > 0)
        {
            yield return null;
            for (int i = 0; i < tutorialManager.enemys3.Count; i++)
                if (tutorialManager.enemys3[i] == null)
                    tutorialManager.enemys3.Remove(tutorialManager.enemys3[i]);
        }

        tutorialManager.playerController.canMove = false;
        tutorialManager.playerController.Stop();
        tutorialManager.portal3.gameObject.SetActive(true);
        SetStringEmpty();
        if (Vector3.Distance(npc.position, player.position) > 12f)
        {
            tutorialManager.FadeOut(5f);
            player.position = npc.position;
            player.transform.Translate(Vector3.left * 6f);
            yield return new WaitForSeconds(2f);

        }

        for (int i = 0; i < 3; i++)
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

    public void UsePortal4()
    {
        StartCoroutine(UsePortal4Co());
    }
    IEnumerator UsePortal4Co()
    {
        while (Vector2.Distance(player.position, npc.position) <= 80f) { yield return null; }

        //DataManager.Instance.tutorialData.isClear = true;
        //DataManager.Instance.SaveTutorialData(); 
        SceneManager.LoadScene("Intro");
    }
} 