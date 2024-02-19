using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum KeyType
{
    Up, Down, Left, Right, Dash, Inven, Action, Map, End
}

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    public KeyCode up { get; private set; }
    public KeyCode down { get; private set; }
    public KeyCode left { get; private set; }
    public KeyCode right { get; private set; }
    public KeyCode dash { get; private set; }
    public KeyCode inven { get; private set; }
    public KeyCode action { get; private set; }
    public KeyCode map { get; private set; }

    private KeyType curType = KeyType.End;
    private GameObject select;
    bool isEncoding = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : KeyManager is Multiple running!");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isEncoding)
        {
            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k))
                {
                    switch (curType)
                    {
                        case KeyType.Up: up = k; break;
                        case KeyType.Down: down = k; break;
                        case KeyType.Left: left = k; break;
                        case KeyType.Right: right = k; break;
                        case KeyType.Dash: dash = k; break;
                        case KeyType.Inven: inven = k; break;
                        case KeyType.Action: action = k; break;
                        case KeyType.Map: map = k; break;
                    }
                    isEncoding = false;
                    select.GetComponent<TextMeshProUGUI>().text = k.ToString();
                    DataManager.Instance.SaveKey();
                    break;
                }
            }
        }
    }

    public void KeyChange(KeyType type,GameObject obj)
    {
        isEncoding = true;
        select = obj;
        curType = KeyType.End;
    }

    public void KeyChange(GameObject obj)
    {
        switch (obj.name)
        {
            case "Up":
                KeyChange(KeyType.Up, obj);
                break;
            case "Down":
                KeyChange(KeyType.Down, obj);
                break;
            case "Left":
                KeyChange(KeyType.Left, obj);
                break;
            case "Right":
                KeyChange(KeyType.Right, obj);
                break;
            case "Dash":
                KeyChange(KeyType.Dash, obj);
                break;
            case "Inven":
                KeyChange(KeyType.Inven, obj);
                break;
            case "Action":
                KeyChange(KeyType.Action, obj);
                break;
            case "Map":
                KeyChange(KeyType.Map, obj);
                break;
            default:
                Debug.LogError($"{obj.name} : is not defined!");
                break;
        }
    }
}