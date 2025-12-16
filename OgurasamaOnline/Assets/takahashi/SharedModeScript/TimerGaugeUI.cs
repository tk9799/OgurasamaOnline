using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class TimerGaugeUI : MonoBehaviour
{
    [SerializeField] private Image gaugeUI;

    //タイマーの番号
    //ｎ番目のタイマーを見るときに使う
    [SerializeField] private int timerIndex = 0;

    //タイマーの処理をしているスクリプト
    private PlayerTimerManager playerTimerManager;

    private float maxTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTimerManager = GetComponent<PlayerTimerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTimerManager == null)
        {
            //Debug.Log("playerTimerManagerがnullです");
            return;
        }

        maxTime = playerTimerManager.timerSecond;

        float remaining = playerTimerManager.RemainingTimes[timerIndex];

        gaugeUI.fillAmount = remaining / maxTime;
    }
}
