using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlKeyValue : MonoBehaviour
{
    public KeyType keyInfo = KeyType.End;

    private void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        switch (keyInfo)
        {
            case KeyType.Up:
                text.text = DataManager.Instance.keyData.up.ToString();
                break;
            case KeyType.Down:
                text.text = DataManager.Instance.keyData.down.ToString();
                break;
            case KeyType.Left:
                text.text = DataManager.Instance.keyData.left.ToString();
                break;
            case KeyType.Right:
                text.text = DataManager.Instance.keyData.right.ToString();
                break;
            case KeyType.Dash:
                text.text = DataManager.Instance.keyData.dash.ToString();
                break;
            case KeyType.Action:
                text.text = DataManager.Instance.keyData.action.ToString();
                break;
            case KeyType.Inven:
                text.text = DataManager.Instance.keyData.inven.ToString();
                break;
            case KeyType.Map:
                text.text = DataManager.Instance.keyData.map.ToString();
                break;
            default:
                Debug.LogError($"{transform.name} : is wrong value!");
                break;
        }
    }
}
