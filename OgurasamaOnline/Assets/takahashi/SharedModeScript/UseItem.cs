using UnityEngine;

public class UseItem : MonoBehaviour
{
    public ItemInventory itemInventory;
    private PlayerMovement playerMovement;

    private int itemListCount = 0;
    private int selectionItemNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemInventory = GetComponent<ItemInventory>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (itemInventory == null)
            {
                Debug.LogError("Inventory がありません");
                return;
            }

            if (itemInventory.itemList.Count > 0)
            {
                //ItemData used = itemInventory.itemList(0);
                //if (used != null)
                //{
                //    // 例：移動速度UPの効果を即時適用
                //    //if (playerMovement != null)
                //    //{
                //    //    playerMovement.ApplySpeedBoost(used.moveSpeedBonus, 5f); // 5秒だけブースト（例）
                //    //}
                //}
            }
            else
            {
                Debug.Log("アイテムがありません");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("アイテム使用");
            itemListCount = itemInventory.itemList.Count;
        }
    }
}
