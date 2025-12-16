using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimerManager : NetworkBehaviour
{
    [Networked,Capacity(5)]
    public NetworkArray<float> RemainingTimes { get; }

    private ItemInventory itemInventory;

    private const int MaxTimers = 5;

    [Header("タイマーの時間")]
    [SerializeField] public float timerSecond = 10f;

    private void Start()
    {
        itemInventory = GetComponent<ItemInventory>();
    }

    /// <summary>
    /// タイマーを毎フレーム減らす処理
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        
        //全てのタイマーをチェック
        for (int i = 0; i < MaxTimers; i++)
        {
            //Debug.Log("処理");
            if (RemainingTimes[i] > 0)
            {
                //経過時間を引く
                RemainingTimes.Set(i, RemainingTimes[i] - Runner.DeltaTime);
                //Debug.Log(RemainingTimes);

                //タイマーの時間が０になった時の処理
                if (RemainingTimes[i] <= 0)
                {
                    itemInventory.isMoveImprovementItem = false;
                    itemInventory.isDeleteObject = true;
                    //OnTimerEnd(i);
                }
            }
        }
    }

    /// <summary>
    /// index番目のタイマー（60秒）を開始する
    /// </summary>
    public void StartTimer(int index)
    {
        if (Object.HasStateAuthority)
        {
            RemainingTimes.Set(index, timerSecond);
        }
    }
}
