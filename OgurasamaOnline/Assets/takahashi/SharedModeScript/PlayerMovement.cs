using Fusion;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : NetworkBehaviour
{
    private NetworkCharacterController networkCharacterController;

    private CharacterController _controller;

    private ItemInventory itemInventory;

    private PlayerTimerManager playerTimerManager;

    [SerializeField] Transform cameraTransform;

    //カメラのターゲットを設定するためにCamera変数を追加
    public Camera Camera;

    //プレイヤーの現在の速度とデフォルト、ダッシュ時の速度
    public float PlayerSpeed = 0f;
    public float PlayerDefaultSpeed = 2f;
    public float PlayseDashSpeed = 5f;

    //ダッシュできるかの判定をするbool
    public bool isDash = false;

    public bool shiftInput = false;

    //プレイヤーが加速アイテムを使用したときに加算する速度
    private float playerAccelerationSpeed = 5f;
    private float playerAccelerationSpeedDash = 15f;

    // Shiftを押しているか（入力状態）
    public bool IsPressingDash { get; private set; }

    [Networked] public bool IsDashing { get; set; }

    [Networked] float Yaw { get; set; }
    //[Networked] float Pitch { get; set; }


    private void Awake()
    {
        //CharacterControllerとItemInventoryスクリプトを取得
        _controller = GetComponent<CharacterController>();
        itemInventory = GetComponent<ItemInventory>();
        playerTimerManager = GetComponent<PlayerTimerManager>();

        networkCharacterController = GetComponent<NetworkCharacterController>();

        PlayerSpeed = PlayerDefaultSpeed;
    }

    /// <summary>
    /// スポーン時カメラの参照を取得してターゲットを自分に設定する
    /// </summary>
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;
            cameraTransform = Camera.transform;

            var timerManager = GetComponent<PlayerTimerManager>();

            // すべての TimerGaugeUI に Player をセット
            foreach (var ui in FindObjectsByType<TimerGaugeUI>(FindObjectsSortMode.None))
            {
                ui.SetPlayer(timerManager);
            }

        }

        if (Object.HasStateAuthority)
        {
            isDash = true;

            // スタミナを満タンにする
            playerTimerManager.SutaminaTimerStart((int)TimerType.dash);
        }
    }

    /// <summary>
    /// Shiftキーの入力の更新
    /// </summary>
    public void Update()
    {
        // Shiftキーの入力状態を更新して入力があるとtrueにする
        //shiftInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out PlayerInputData playerInputData))
        {
            Debug.Log("入力取れてません");
            return;
        }

        // 向きをネットワークで更新
        //Yaw += playerInputData.look.x * 3f;
        //Pitch -= playerInputData.look.y * 3f;
        //Pitch = Mathf.Clamp(Pitch, -80f, 80f);

        //transform.rotation = Quaternion.Euler(0, Yaw, 0);

        if (Object.HasInputAuthority)
        {
            Vector3 lookDir = Camera.transform.forward;
            lookDir.y = 0f;
            Yaw += playerInputData.look.x * 30f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                transform.forward = lookDir;
            }
        }

        if (Object.HasStateAuthority)
        {
            //Yaw += playerInputData.look.x * 3f;
            transform.rotation = Quaternion.Euler(0, Yaw, 0);
        }

        Vector3 cameraForward = Camera.transform.forward;
        Vector3 cameraRight = Camera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 moveDir = cameraForward.normalized * playerInputData.move.y + cameraRight.normalized * playerInputData.move.x;
        //Vector3 moveDir = forward * playerInputData.move.y + right * playerInputData.move.x;


        IsDashing = playerInputData.Dash && isDash;

        bool shift= UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);

        //// スタミナがあるかどうか
        ////bool canDash = playerTimerManager.CanDash();


        if (IsDashing&& CompareTag("Player"))
        {
            
            if (itemInventory != null && itemInventory.isMoveImprovementItem)
            {
                PlayerSpeed = playerAccelerationSpeedDash;
                //Debug.Log("加速アイテム使用中ダッシュ");
                //Debug.Log(PlayerSpeed);
            }
            else
            {
                PlayerSpeed = PlayseDashSpeed;
                Debug.Log(PlayerSpeed);
            }
        }
        else if(itemInventory != null && itemInventory.isMoveImprovementItem)
        {
            PlayerSpeed = playerAccelerationSpeed;
            //Debug.Log("加速アイテム使用中通常移動");
        }
        else
        {
            isDash = false;
            PlayerSpeed = PlayerDefaultSpeed;
            //Debug.Log("ダッシュ解除");
        }


        //if (itemInventory != null && itemInventory.isMoveImprovementItem)
        //{
        //    PlayerSpeed += playerAccelerationSpeed;
        //}

        //Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) *
        //    Runner.DeltaTime * PlayerSpeed;
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        //Debug.DrawRay(Camera.transform.position, Camera.transform.forward * 30, Color.red);
        //_controller.Move(moveDir * PlayerSpeed * Runner.DeltaTime);
        networkCharacterController.maxSpeed = PlayerSpeed;
        networkCharacterController.Move(moveDir.normalized * /*PlayerSpeed **/ Runner.DeltaTime);
    }
}
