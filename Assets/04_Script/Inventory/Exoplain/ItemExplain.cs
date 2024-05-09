using UnityEngine;
using UnityEngine.UI;

public class ItemExplain : MonoBehaviour
{
    public static ItemExplain Instance;
        
    [SerializeField] Image panelImage;
    [SerializeField] WeaponExplain weaponExplain;
    [SerializeField] GeneratorExplain generatorExplain;
    [SerializeField] Tooltip tooltip;

    private InventoryActive inventoryActive;
    public bool isDrag = false;

    public bool useMove = false;

    Vector2 curInvenPoint;
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
        if (useMove)
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

        if (panelImage.enabled == false)
        {
            HoverEnd();
        }
    }


    public void HoverEvent(Vector2 invenPoint)
    {
        if (invenPoint == curInvenPoint) return;
        if (tooltip.gameObject.activeSelf == false)
            tooltip.Active(true);

        curInvenPoint = invenPoint;
        tooltip.On(invenPoint, null, null, null);
        
    }


    public void HoverWeapon(Vector2 invenPoint, Sprite image, string name, float power, string explain, string[] skillList)
    {
        //generatorExplain.gameObject.SetActive(false);
        //weaponExplain.gameObject.SetActive(true);
        if (invenPoint == curInvenPoint) return;
        if (tooltip.gameObject.activeSelf == false)
            tooltip.Active(true);

        curInvenPoint = invenPoint;
        tooltip.On(invenPoint, null, null, null);

        //weaponExplain.ON(image, name, power, explain, skillList);
    }

    public void HoverGenerator(Vector2 invenPoint, Sprite image, string trigger, string skillList)
    {
        //generatorExplain.gameObject.SetActive(true);
        //weaponExplain.gameObject.SetActive(false);

        if (invenPoint == curInvenPoint) return;
        if (tooltip.gameObject.activeSelf == false)
            tooltip.Active(true);

        curInvenPoint = invenPoint;
        tooltip.On(invenPoint, null, null, null);

        //generatorExplain.ON(image, trigger, skillList);
    }

    public void HoverEnd()
    {
        //weaponExplain.gameObject.SetActive(false);
        //generatorExplain.gameObject.SetActive(false);
        tooltip.Active(false);
    }


    public bool IsOn() => weaponExplain.gameObject.activeSelf;
}