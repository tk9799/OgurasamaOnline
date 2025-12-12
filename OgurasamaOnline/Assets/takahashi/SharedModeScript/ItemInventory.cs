using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : NetworkBehaviour
{
    [Networked] public float TimeLeft { get; set; }

    public List<ItemData> itemList = new List<ItemData>();

    private ItemData itemData;
    private PlayerMovement playerMovement;

    //移動速度UPする際のSpeed加算する数値
    public float reinforcementMove = 10f;

    //移動速度UPアイテムの効果が有効かを判定する
    public bool isMoveImprovementItem = false;

    public bool isDeleteObject = false;

    private float moveImprovementItemCount = 0f;

    public void AddItem(ItemData item)
    {
        itemList.Add(item);
    }



    private void Start()
    {
        //itemData = GetComponent<ItemData>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(itemList.Count > 0)
            {
                itemData = itemList[0];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (itemList.Count > 1)
            {
                itemData = itemList[1];
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(itemData.itemID);
            DistinctionItem();
            Debug.Log("アイテム使用");
            //itemListCount = itemInventory.itemList.Count;
        }

        if (isMoveImprovementItem)
        {
            moveImprovementItemCount += Runner.DeltaTime;
        }

        if (isDeleteObject)
        {
            itemList.Remove(itemData);
            isDeleteObject = false;
            //アイテム削除
            Debug.Log("アイテム削除");
        }
    }

    /// <summary>
    /// 使用するアイテムを判別するメソッド
    /// </summary>
    private void DistinctionItem()
    {
        if (itemData.itemID == 1)
        {
            MoveImprovementItem();
        }
    }

    /// <summary>
    /// 移動速度UPするアイテム
    /// </summary>
    public void MoveImprovementItem()
    {
        isMoveImprovementItem = true;
        playerMovement.PlayerSpeed += 50;
        Debug.Log($"PlayerSpeed{playerMovement.PlayerSpeed}");

        GetComponent<PlayerTimerManager>().StartTimer(0);
    }
}
