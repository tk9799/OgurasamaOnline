using Fusion;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInventory : NetworkBehaviour
{
    [Networked] public float TimeLeft { get; set; }

    public List<ItemData> itemList = new List<ItemData>();

    //private ItemData itemData;
    private PlayerMovement playerMovement;
    private PlayerTimerManager playerTimerManager;

    //移動速度UPする際のSpeed加算する数値
    public float reinforcementMove = 10f;

    //移動速度UPアイテムの効果が有効かを判定する
    public bool isMoveImprovementItem = false;

    //スタミナ強化アイテムの効果が有効かを判定する
    public bool isSutaminaStrengthening = false;

    public bool isDeleteObject = false;

    private float moveImprovementItemCount = 0f;

    // 今選択中のアイテム
    private int selectedIndex = -1;

    /// <summary>
    /// アイテムを取得してListに追加する
    /// 何も選択していなければ自動で０番目を選択する
    /// </summary>
    public void AddItem(ItemData item)
    {
        itemList.Add(item);

        // 最初に拾ったら自動で選択
        if (selectedIndex == -1)
        {
            selectedIndex = 0;
        }
    }

    private void Start()
    {
        //itemData = GetComponent<ItemData>();
        playerMovement = GetComponent<PlayerMovement>();
        playerTimerManager = GetComponent<PlayerTimerManager>();
    }

    private void Update()
    {
        // 入力権限がなければ何もしない
        //効果を適応するのがStateAuthority
        if (!Object.HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(1);
        }


        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("右クリック入力");
            
            UseSelectItem();
            //playerTimerManager.StartTimer(0);
            Debug.Log("アイテム使用");
            //itemListCount = itemInventory.itemList.Count;
        }

        if (isMoveImprovementItem)
        {
            moveImprovementItemCount += Runner.DeltaTime;
        }

        if (isDeleteObject)
        {
            //itemList.Remove(itemData);
            isDeleteObject = false;
            //アイテム削除
            Debug.Log("アイテム削除");
        }
    }

    /// <summary>
    /// List内をチェック
    /// アイテムを数字キーに割り当てる
    /// </summary>
    private void SelectItem(int index)
    {
        if (index < 0 || index >= itemList.Count)
        {
            Debug.Log("List内にアイテムがない");
            return;
        }

        selectedIndex = index;
        Debug.Log($"選択中: {itemList[selectedIndex].itemName}");
    }

    /// <summary>
    /// アイテムを取得し使用するメソッドを呼び出す処理
    /// </summary>
    private void UseSelectItem()
    {
        if (selectedIndex < 0 || selectedIndex >= itemList.Count)
        {
            Debug.Log("使用できるアイテムがありません");
            return;
        }

        //アイテムを取得
        ItemData item = itemList[selectedIndex];

        //アイテムの効果を呼び出す
        UseItemById(item.itemID);

        // 使用後に削除
        itemList.RemoveAt(selectedIndex);

        //selectedIndexがはみ出さないようにする
        if (itemList.Count == 0)
        {
            selectedIndex = -1;
        }
        else if (selectedIndex >= itemList.Count)
        {
            selectedIndex = itemList.Count - 1;
        }
    }

    /// <summary>
    /// 移動速度UPするアイテム
    /// </summary>
    public void MoveImprovementItem()
    {
        Debug.Log($"PlayerSpeed{playerMovement.PlayerSpeed}");

        //タイマーの処理が書かれている処理を呼び出す
        var timer = GetComponent<PlayerTimerManager>();

        //enumでアイテムの種類を指定しタイマーを起動する
        timer.StartTimer((int)TimerType.moveSpeedUp);
        isMoveImprovementItem = true;
    }

    /// <summary>
    /// スタミナを強化するアイテム
    /// </summary>
    public void SutaminaStrengthening()
    {
        //タイマーの処理が書かれている処理を呼び出す
        var timer = GetComponent<PlayerTimerManager>();

        //enumでアイテムの種類を指定しタイマーを起動する
        timer.StartTimer((int)TimerType.sutaminaStrengthening);
        isSutaminaStrengthening = true;
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //private void RPC_RequestUseItem(int itemId)
    //{
    //    UseItemById(itemId);
    //    playerTimerManager.StartTimer((int)TimerType.moveSpeedUp);
    //    Debug.Log($"アイテム使用 {itemId}");
    //}

    /// <summary>
    /// アイテムの効果を呼び出す処理
    /// </summary>
    private void UseItemById(int itemId)
    {
        switch (itemId)
        {
            case 0: // 移動速度UP
                MoveImprovementItem();
                //playerTimerManager.StartTimer((int)TimerType.moveSpeedUp);
                break;
            case 1:
                SutaminaStrengthening();
                //playerTimerManager.StartTimer((int)TimerType.sutaminaStrengthening);
                break;
        }
    }
}
