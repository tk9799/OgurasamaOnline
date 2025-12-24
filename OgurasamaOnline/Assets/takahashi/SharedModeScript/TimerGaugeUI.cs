using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class TimerGaugeUI : MonoBehaviour
{
    [SerializeField] private Image gaugeUI;

    // このUIが監視するタイマーの種類
    [SerializeField] private TimerType timerType;

    //タイマーの番号

    //タイマーの処理をしているスクリプト
    private PlayerTimerManager playerTimerManager;

    private float maxTime = 10.0f;

    void Start()
    {
        gaugeUI.fillAmount = 1.0f;
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

        int index = (int)timerType;

        float maxTime = GetMaxTime(timerType);
        float remaining = playerTimerManager.RemainingTimes[index];

        //float maxTime = GetMaxTime();


        float rate = Mathf.Clamp01(remaining / maxTime);
        gaugeUI.fillAmount = rate;
        //Debug.Log($"[{timerType}] remaining={remaining}");
        //Debug.Log($"[{timerType}] index={(int)timerType} remaining={remaining}");
    }

    private float GetMaxTime(TimerType type)
    {
        switch (type)
        {
            case TimerType.moveSpeedUp:
                return playerTimerManager.timerTime;

            case TimerType.dash:
                return playerTimerManager.sutaminaTimerTime;

            default:
                return 1f;
        }
    }
}
