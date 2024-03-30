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

    private void Start()
    {
        if(DataManager.Instance == null)
        {
            Debug.LogError($"DataManager is null!");
            Destroy(gameObject);
        }
        else
        {
            up = DataManager.Instance.keyData.up;
            down = DataManager.Instance.keyData.down;
            left = DataManager.Instance.keyData.left;
            right = DataManager.Instance.keyData.right;
            dash = DataManager.Instance.keyData.dash;
            inven = DataManager.Instance.keyData.inven;
            action = DataManager.Instance.keyData.action;
            map = DataManager.Instance.keyData.map;
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
                        case KeyType.Up:
                            up = k;
                            DataManager.Instance.keyData.up = k; break;
                        case KeyType.Down:
                            down = k;
                            DataManager.Instance.keyData.down = k; break;
                        case KeyType.Left:
                            left = k;
                            DataManager.Instance.keyData.left = k; break;
                        case KeyType.Right:
                            right = k;
                            DataManager.Instance.keyData.right = k; break;
                        case KeyType.Dash:
                            dash = k;
                            DataManager.Instance.keyData.dash = k; break;
                        case KeyType.Inven:
                            inven = k;
                            DataManager.Instance.keyData.inven = k; break;
                        case KeyType.Action:
                            action = k;
                            DataManager.Instance.keyData.action = k; break;
                        case KeyType.Map:
                            map = k;
                            DataManager.Instance.keyData.map = k; break;
                    }
                    isEncoding = false;
                    select.GetComponent<TextMeshProUGUI>().text = k.ToString();
                    DataManager.Instance.SaveKey();
                    break;
                }
            }
        }
    }

    public void KeyChange(KeyType type, GameObject obj)
    {
        isEncoding = true;
        select = obj;
        curType = type;
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