using Fusion;
using System.Collections.Generic;
using UnityEngine;

public enum TimerType
{
    moveSpeedUp=0,
    dash=4
}

public class PlayerTimerManager : NetworkBehaviour
{
    [Networked,Capacity(5)]
    public NetworkArray<float> RemainingTimes { get; }

    private ItemInventory itemInventory;

    private PlayerMovement playerMovement;

    private const int MaxTimers = 5;

    [Header("タイマーの時間")]
    [SerializeField] public float timerTime = 10f;

    [Header("ダッシュできる時間")]
    [SerializeField] public float sutaminaTimerTime = 10f;
    [SerializeField] public float sutaminaRecoveryTime = 20f;

    [Header("スタミナ消費速度（1秒あたり）")]
    [SerializeField] private float dashConsumeSpeed = 2f;

    [Header("スタミナ回復速度（消費の2倍）")]
    [SerializeField] private float dashRecoverSpeed = 0.5f;

    /// <summary>
    /// 主に初期化をするメソッド
    /// </summary>
    public override void Spawned()
    {
        itemInventory = GetComponent<ItemInventory>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// タイマーを毎フレーム減らす処理
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // 効果時間タイマーだけ処理
        for (int i = 0; i < MaxTimers; i++)
        {
            // dash は除外する
            if (i == (int)TimerType.dash) continue;

            if (RemainingTimes[i] > 0)
            {
                RemainingTimes.Set(i, RemainingTimes[i] - Runner.DeltaTime);
                Debug.Log(RemainingTimes);

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

    private void HandleDashStamina()
    {
        int dashIndex = (int)TimerType.dash;

        // ダッシュ中（Shift押しっぱなし & ダッシュ可能）
        if (playerMovement.shiftInput/* && playerMovement.isDash*/)
        {
            //playerMovement.isDash = true;の時のみスタミナ消費
            if (playerMovement.IsDashing)
            {
                RemainingTimes.Set(dashIndex,RemainingTimes[dashIndex] - Runner.DeltaTime * dashConsumeSpeed);
                //Debug.Log(RemainingTimes[dashIndex]);

                if (RemainingTimes[dashIndex] <= 0)
                {
                    RemainingTimes.Set(dashIndex, 0);
                    playerMovement.isDash = false; // スタミナ切れ
                    Debug.Log("スタミナ切れ");
                }
            }

        }
        // Shiftを離している → 回復
        else
        {
            RemainingTimes.Set(dashIndex,Mathf.Min(sutaminaRecoveryTime, RemainingTimes[dashIndex] + Runner.DeltaTime * dashRecoverSpeed));
            //Debug.Log(RemainingTimes);

            // 一定以上回復したらダッシュ可能に戻す
            if (RemainingTimes[dashIndex] > 0.5f)
            {
                playerMovement.isDash = true;
            }
        }
    }

    public bool CanDash()
    {
        return RemainingTimes[(int)TimerType.dash] > 0;
    }

    private void OnTimerEnd(int index)
    {
        switch ((TimerType)index)
        {
            case TimerType.moveSpeedUp:
                itemInventory.isMoveImprovementItem = false;
                itemInventory.isDeleteObject = true;
                break;
            case TimerType.dash:
                playerMovement.isDash = false;
                break;

        }
    }

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
