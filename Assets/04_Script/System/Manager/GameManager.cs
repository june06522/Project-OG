using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
    [HideInInspector] public bool canControl = true;
    #endregion

    [NonSerialized] public Transform player;
    private PlayerController playerController;
    public PlayerController PlayerController => playerController;

    // Item Probablity
    private float _normalProbability;
    private float _rareProbability = 33f;
    private float _epicProbability = 15f;
    private float _legendaryProbability = 2f;

    private Dictionary<ItemRate, float> _itemRateProbability;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Intance is multiply");
            Destroy(gameObject);
        }
        #region 객체 할당
        player = GameObject.Find("Player").GetComponent<Transform>();

        if (player != null)
        {

            playerController = player.GetComponent<PlayerController>();

        }

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

        _normalProbability = 100f - _rareProbability - _epicProbability - _legendaryProbability;
        _itemRateProbability = new Dictionary<ItemRate, float>()
        {
            { ItemRate.NORMAL, _normalProbability },
            { ItemRate.RARE, _rareProbability },
            { ItemRate.EPIC, _epicProbability },
            { ItemRate.LEGEND, _legendaryProbability }
        };

    }

    public void PlayerTeleport(Vector3 pos)
    {
        
        if (playerController != null)
        {

            playerController.ChangeState(EnumPlayerState.Idle);

        }

        player.position = pos;

    }

    public float GetRateProbability(ItemRate rate)
    {
        return _itemRateProbability[rate];
    }

}
