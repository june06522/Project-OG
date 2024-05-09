using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoSingleton<GameManager>
{
    #region 인벤토리 관련
    [SerializeField] InventoryActive inventoryActive;
    public InventoryActive InventoryActive => inventoryActive;

    [SerializeField] WeaponInventory inventory;
    public WeaponInventory Inventory => inventory;

    [SerializeField] WeaponController weaponController;
    public WeaponController WeaponController => weaponController;

    public InvenBrickAddType invenAddType;
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
    [HideInInspector] public bool isPlay = false;
    #endregion

    [NonSerialized] public Transform player;

    private void Awake()
    {

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

        if(invenAddType == null)
        {
            invenAddType = FindObjectOfType<InvenBrickAddType>();
            if(invenAddType == null)
                Debug.LogError("invenAddType is null! You should add Component InvenBrickAddType in Inven Parent!");
        }
        #endregion
    }
}
