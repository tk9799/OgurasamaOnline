using Fusion;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OguraController : NetworkBehaviour
{
    // MiniMapCameraを小椋プレハブの子にする

    [SerializeField] private Transform oguraTransform = null;

    public Camera camera = null;

    [SerializeField] public bool isSpawned = false;

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

    [SerializeField] private float initialPlayerSpeed = 0.0f;

    [SerializeField] public float playerSpeed = 0.0f;

    [SerializeField] public float playerDashSpeed = 0.0f;

    [SerializeField] public bool isPlayerDash = false;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        initialPlayerSpeed = playerSpeed;

    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && 0 < currentStamina)
        {
            isPlayerDash = true;
            Debug.Log("aaa");
        }
        else
        {
            isPlayerDash = false;
        }
    }

    /// <summary>
    /// 基本的な移動処理メソッド
    /// </summary>
    public override void FixedUpdateNetwork()
    {

        Quaternion cameraRotationY = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * playerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }



        if (isPlayerDash)
        {
            if (0 < currentStamina)
            {
                playerSpeed = playerDashSpeed;
                currentStamina -= Runner.DeltaTime;
            }

            if (currentStamina < maxStamina / 2)
            {
                // スタミナバーの色を赤に変更
            }
            else if (currentStamina <= 0)
            {
                isPlayerDash = false;
                playerSpeed = initialPlayerSpeed;
            }
        }
        else
        {
            playerSpeed = initialPlayerSpeed;
            currentStamina += Runner.DeltaTime;
        }

        if (maxStamina <= currentStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            camera = Camera.main;
            camera.GetComponent<OguraCamera>().Target = transform;

            isSpawned = true;


        }
    }


    private void SpeedUpSkill()
    {
        // スキルボタンを押したら処理開始
        // スキル1
    }

    private void PlayerDetectionSkill()
    {
        // スキルボタンを押したら処理開始
        // スキル2
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