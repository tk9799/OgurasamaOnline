using Fusion;
using TMPro;
using UnityEngine;

public class OguraController : NetworkBehaviour
{
    [Header("スキルのクールタイム")]
    [SerializeField] private float coolTime = 0.0f;

    [Header("スピードアップスキルが使えるかどうかの判定")]
    [SerializeField] private bool isSpeedUpSkill = false;

    [Header("プレイヤー検知スキルが使えるかどうかの判定")]
    [SerializeField] private bool isPlayerDetectionSkill = false;

    [Header("最大スタミナ")]
    [SerializeField] private float maxStamina = 0.0f;

    [Header("現在のスタミナ")]
    [SerializeField] private float currentStamina = 0.0f;

    [Header("プレイヤーとの当たり判定")]
    [SerializeField] public bool isCollisionPlayer = false;


    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        // プレイヤーが走る処理
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 移動速度増加の処理

        }
        else if (currentStamina <= maxStamina / 2)
        {
            // スタミナゲージの色が赤に
        }
        else
        {
            // ダッシュしてないときはスタミナ回復
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime;
            }
        }
    }

    private void SpeedUpSkill()
    {
       // スキルボタンを押したら処理開始
    }

    private void PlayerDetectionSkill()
    {
        // スキルボタンを押したら処理開始
    }


    /// <summary>
    /// 小椋がプレイヤーに当たったらisCollisionPlayerをtrueにするメソッド
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            isCollisionPlayer = true;
        }
    }

    /// <summary>
    /// 小椋がプレイヤーから離れたらisCollisionPlayerをfalseにするメソッド
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            isCollisionPlayer = false;
        }
    }
}