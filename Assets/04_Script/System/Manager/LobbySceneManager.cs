using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    public static LobbySceneManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : LobbySceneManager is multiply running!");
            Destroy(gameObject);
        }
    }

    public void Play() => AsyncSceneLoader.LoadScene("Play");
    public void Test() => AsyncSceneLoader.LoadScene("UserTest");
}
