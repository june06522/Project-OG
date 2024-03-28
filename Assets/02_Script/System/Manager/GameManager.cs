using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    #region 인벤토리 관련
    [SerializeField] InventoryActive inventoryActive;
    public InventoryActive InventoryActive => inventoryActive;

    [SerializeField] WeaponInventory inventory;
    public WeaponInventory Inventory => inventory;

    [SerializeField] WeaponController weaponController;
    public WeaponController WeaponController => weaponController;
    #endregion

    #region 라이트 관련
    [SerializeField]
    private Light2D _globalLight;
    public Light2D GlobalLight => _globalLight;
    public void ResetGlobalLight() { _globalLight.intensity = _defaultLightValue; }

    private float _defaultLightValue = 0.9f;
    public float DefaultLightValue => _defaultLightValue;

    #endregion

    #region 상태 관련
    [HideInInspector] public bool isShopOpen = false;
    #endregion

    [NonSerialized] public Transform player;

    private void Awake()
    {
        #region 싱글톤
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"{transform} : GameManager is multiply running!");
            Destroy(this);
        }
        #endregion

        #region 객체 할당
        player = GameObject.Find("Player").GetComponent<Transform>();

        if (inventory == null)
        {
            inventory = FindObjectOfType<WeaponInventory>();
            if (inventory == null)
                Debug.LogError("Inventory is null! You should Inventory setActive true");
        }

        if (weaponController == null)
        {
            weaponController = FindObjectOfType<WeaponController>();
            if (weaponController == null)
                Debug.LogError("weaponController is null! You should weaponController setActive true");
        }

        if (inventoryActive == null)
        {
            inventoryActive = FindObjectOfType<InventoryActive>();
            if (inventoryActive == null)
                Debug.LogError("inventoryActive is null! You should inventoryActive setActive true");
        }
        #endregion
    }
}
