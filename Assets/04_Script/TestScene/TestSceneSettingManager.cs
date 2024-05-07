using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneSettingManager : MonoBehaviour
{
    public static TestSceneSettingManager Instance;

    PlayerEnerge energe;
    PlayerHP php;

    bool hp = false;
    bool dash = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : TestSceneSettingManager is Multiply running!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        php = FindObjectOfType<PlayerHP>();
        energe = FindObjectOfType<PlayerEnerge>();
    }

    private void Update()
    {
        if (hp)
            php.RestoreHP(php.MaxHP);
        if (dash)
            energe.RestoreEnerge(energe.MaxEnerge);
    }

    public void Invincibility() => hp = !hp;

    public void DashInfinite() => dash = !dash;

    public void InvenAdd() => ExpansionManager.Instance.AddSlotcnt(5);

    public void InitScene() => SceneManager.LoadScene("UserTest");

    public void LobbyScene() => SceneManager.LoadScene("Lobby");
}