using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemExplain : MonoBehaviour
{
    public static ItemExplain Instance;

    [SerializeField] WeaponExplain weaponExplain;
    [SerializeField] GeneratorExplain generatorExplain;

    private InventoryActive inventoryActive;
    public bool isDrag = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform}: Item Explain is multiply running!");
        }


        inventoryActive = FindObjectOfType<InventoryActive>();
    }

    private void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.x += (GameManager.Instance.player.transform.position.x - pos.x < 0) ? -2.5f : 2.5f;


        pos.y += (GameManager.Instance.player.transform.position.y - pos.y < 0) ? -2.8f : 2.8f;
        transform.position = pos;

        if (!inventoryActive.IsOn)
        {
            HoverEnd();
        }
    }

    public void HoverWeapon (Sprite image, string name, float power, string explain, string[] skillList)
    {
        generatorExplain.gameObject.SetActive(false);
        weaponExplain.gameObject.SetActive(true);

        weaponExplain.ON(image,name, power, explain, skillList);
    }

    public void HoverGenerator(Sprite image, string trigger, string[] skillList)
    {
        generatorExplain.gameObject.SetActive(true);
        weaponExplain.gameObject.SetActive(false);

        generatorExplain.ON(image, trigger, skillList);
    }

    public void HoverEnd()
    {
        weaponExplain.gameObject.SetActive(false);
        generatorExplain.gameObject.SetActive(false);
    }
}