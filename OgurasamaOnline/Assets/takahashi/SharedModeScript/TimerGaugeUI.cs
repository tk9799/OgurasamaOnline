using UnityEngine;
using Fusion;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;

//SerializableはクラスをInspectorに表示できるようにする機能
/// <summary>
/// アイテムの種類の設定とタイマー用UIを入れているクラス
/// </summary>
[System.Serializable]
public class TimerGaugeSlot
{
    public Image gaugeUI;

    public TimerType timerType;
}

public class TimerGaugeUI : MonoBehaviour
{
    [SerializeField] private List<TimerGaugeSlot> imageList = new List<TimerGaugeSlot>();

    //[SerializeField] private Image gaugeUI;

    //[SerializeField] private Image gaugeUI2;

    // このUIが監視するタイマーの種類
    //[SerializeField] private TimerType timerType;

    //タイマーの番号

    //タイマーの処理をしているスクリプト
    private PlayerTimerManager playerTimerManager;

    private float maxTime = 10.0f;

    void Start()
    {
        //gaugeUI.fillAmount = 1.0f;
    }

    public void SetPlayer(PlayerTimerManager timerManager)
    {
        playerTimerManager = timerManager;
    }

    void Update()
    {
        if(playerTimerManager == null)
        {
            //Debug.Log("playerTimerManagerがnullです");
            return;
        }

        foreach(var slot in imageList)
        {
            int index = (int)slot.timerType;

            float maxTime = GetMaxTime(slot.timerType);
            float remaining = playerTimerManager.RemainingTimes[index];

            float rate = Mathf.Clamp01(remaining / maxTime);
            slot.gaugeUI.fillAmount = rate;
        }

        

        //float maxTime = GetMaxTime();


        
        //Debug.Log($"[{timerType}] remaining={remaining}");
        //Debug.Log($"[{timerType}] index={(int)timerType} remaining={remaining}");
    }

    private float GetMaxTime(TimerType type)
    {
        switch (type)
        {
            case TimerType.moveSpeedUp:
                return playerTimerManager.timerTime;

            case TimerType.sutaminaStrengthening:
                return playerTimerManager.timerTime;

            case TimerType.dash:
                return playerTimerManager.sutaminaTimerTime;

            default:
                return 1f;
        }
    }
}
