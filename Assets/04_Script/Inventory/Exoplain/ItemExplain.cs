using UnityEngine;
using UnityEngine.UI;

public class ItemExplain : MonoBehaviour
{
    public static ItemExplain Instance;
        
    [SerializeField] Image panelImage;
    [SerializeField] WeaponExplain weaponExplain;
    [SerializeField] GeneratorExplain generatorExplain;

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

    private void Start()
    {
        weaponExplain.gameObject.SetActive( false );
        generatorExplain.gameObject.SetActive( false ); 
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

    public void HoverWeapon(Vector2 invenPoint, Sprite image, string name, float power, string explain, string[] skillList,string evaluation)
    {
        if (invenPoint == curInvenPoint) return;
        if (weaponExplain.gameObject.activeSelf == false)
        {
            generatorExplain.gameObject.SetActive(false);
            weaponExplain.gameObject.SetActive(true);
        }
        
        curInvenPoint = invenPoint;
        weaponExplain.ON(invenPoint, image, name, power, explain, skillList,evaluation);

        //weaponExplain.ON(image, name, power, explain, skillList);
    }

    public void HoverGenerator(Vector2 invenPoint, Sprite image, string trigger, string explain, string evaluation, string name)
    {
        //generatorExplain.gameObject.SetActive(true);
        //weaponExplain.gameObject.SetActive(false);

        if (invenPoint == curInvenPoint) return;
        if (generatorExplain.gameObject.activeSelf == false)
        {
            generatorExplain.Active(true);
            weaponExplain.Active(false);
        }

        curInvenPoint = invenPoint;
        generatorExplain.ON(invenPoint, image, trigger, explain, evaluation, name);

        //generatorExplain.ON(image, trigger, skillList);
    }

    public void HoverEnd()
    {
        weaponExplain.Active(false);
        generatorExplain.Active(false);   
    }


    public bool IsOn() => weaponExplain.gameObject.activeSelf;
}