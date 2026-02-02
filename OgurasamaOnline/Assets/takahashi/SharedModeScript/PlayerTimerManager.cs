using Fusion;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの種類を文字列から数値に変える
/// </summary>
public enum TimerType
{
    moveSpeedUp = 0,
    sutaminaStrengthening = 1,
    dash = 4
}

/// <summary>
/// プレイヤーにかかっているすべてのタイマー（アイテム効果・スタミナ）を管理するクラス
/// </summary>
public class PlayerTimerManager : NetworkBehaviour
{
    //各タイマーの残り時間をネットワーク同期で持つ配列
    [Networked, Capacity(5)]
    public NetworkArray<float> RemainingTimes { get; }

    private ItemInventory itemInventory;

    private PlayerMovement playerMovement;

    private const int MaxTimers = 5;


    [Header("タイマーの時間")]
    [SerializeField] public float timerTime = 10f;

    [Header("ダッシュできる時間")]
    [SerializeField] public float sutaminaTimerTime = 10.0f;
    [SerializeField] public float sutaminaRecoveryTime = 20.0f;

    [Header("スタミナ消費速度（1秒あたり）")]
    [SerializeField] private float dashConsumeSpeed = 1.0f;

    [Header("スタミナ回復速度（消費の2倍）")]
    [SerializeField] private float dashRecoverSpeed = 0.5f;

    /// <summary>
    /// 主に初期化をするメソッド
    /// </summary>
    public override void Spawned()
    {
        itemInventory = GetComponent<ItemInventory>();
        playerMovement = GetComponent<PlayerMovement>();

        //ネットワーク上で値を変更できるのは管理者だけなのでStateAuthorityをチェックする
        if (Object.HasStateAuthority)
        {
            RemainingTimes.Set((int)TimerType.dash, sutaminaTimerTime);
        }
    }

    /// <summary>
    /// タイマーを毎フレーム減らす処理
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // 効果時間タイマーだけ処理
        for (int i = 0; i < MaxTimers; i++)
        {
            // dash は専用のタイマー処理があるため除外する
            if (i == (int)TimerType.dash) continue;

            if (RemainingTimes[i] > 0)
            {
                RemainingTimes.Set(i, RemainingTimes[i] - Runner.DeltaTime);
                //Debug.Log(RemainingTimes);

                //タイマーが０になったら効果終了の処理を呼び出す
                if (RemainingTimes[i] <= 0)
                {
                    RemainingTimes.Set(i, 0);
                    OnTimerEnd(i);
                }
            }
        }
        //Debug.Log($"ItemTimer = {RemainingTimes[(int)TimerType.moveSpeedUp]}");
        // スタミナ専用処理
        HandleDashStamina();
    }

    /// <summary>
    /// ダッシュ中はスタミナを減らし、してない時は回復させる処理
    /// 消費回復速度は[SerializeField]で調整
    /// </summary>
    private void HandleDashStamina()
    {
        int dashIndex = (int)TimerType.dash;

        //playerMovement.isDash = true;の時のみスタミナ消費
        if (playerMovement.IsDashing)
        {
            //スタミナ強化アイテムを使っている場合減らす速度を遅らせる
            if (itemInventory.isSutaminaStrengthening)
            {
                dashConsumeSpeed = 0.5f;
            }
            else
            {
                dashConsumeSpeed = 1.0f;
            }

            //スタミナを減らす
            RemainingTimes.Set(dashIndex, RemainingTimes[dashIndex] - Runner.DeltaTime * dashConsumeSpeed);
            Debug.Log(RemainingTimes[dashIndex]);

            //スタミナがなくなったらダッシュできない
            if (RemainingTimes[dashIndex] <= 0f)
            {
                RemainingTimes.Set(dashIndex, 0f);
                playerMovement.isDash = false; // スタミナ切れ
                Debug.Log("スタミナ切れ");
            }
            else
            {
                playerMovement.isDash = true;
            }
        }
        // Shiftを離している → 回復
        else if (!playerMovement.shiftInput)
        {
            //スタミナ回復
            RemainingTimes.Set(dashIndex, Mathf.Min(sutaminaTimerTime, RemainingTimes[dashIndex] + Runner.DeltaTime * dashRecoverSpeed));
            //Debug.Log(RemainingTimes);

            // 一定以上回復したらダッシュ可能に戻す
            if (RemainingTimes[dashIndex] >= 0.5f)
            {
                playerMovement.isDash = true;
            }
        }
        //Debug.Log($"Stamina={RemainingTimes[dashIndex]}, Exhausted={isExhausted}");
//        Debug.Log(
//    $"shift={playerMovement.shiftInput} " +
//    $"IsDashing={playerMovement.IsDashing} " +
//    $"stamina={RemainingTimes[(int)TimerType.dash]}"
//);
    }

    public bool CanDash()
    {
        return RemainingTimes[(int)TimerType.dash] > 0;
    }

    /// <summary>
    /// タイマーの時間が切れたときに行う処理
    /// </summary>
    private void OnTimerEnd(int index)
    {
        switch ((TimerType)index)
        {
            //効果をOFFにしてアイテムを削除するboolをtrueにする
            case TimerType.moveSpeedUp:
                itemInventory.isMoveImprovementItem = false;
                itemInventory.isDeleteObject = true;
                break;
            case TimerType.sutaminaStrengthening:
                itemInventory.isSutaminaStrengthening = false;
                break;
            case TimerType.dash:
                playerMovement.isDash = false;
                break;

        }
    }

    //スタミナゲージの割合計算
    public float GetDashRate()
    {
        return RemainingTimes[(int)TimerType.dash] / sutaminaTimerTime;
    }

    /// <summary>
    /// index番目のタイマー（60秒）を開始する
    /// </summary>
    public void StartTimer(int index)
    {
        if (Object.HasStateAuthority)
        {
            RemainingTimes.Set(index, timerTime);
        }
    }

    public void SutaminaTimerStart(int index)
    {
        if (Object.HasStateAuthority)
        {
            RemainingTimes.Set(index, sutaminaTimerTime);
        }
    }
}
