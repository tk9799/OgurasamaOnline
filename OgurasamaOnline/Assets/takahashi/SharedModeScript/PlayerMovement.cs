using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;

    private ItemInventory itemInventory;

    private PlayerTimerManager playerTimerManager;

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

    private void Awake()
    {
        //CharacterControllerとItemInventoryスクリプトを取得
        _controller = GetComponent<CharacterController>();
        itemInventory = GetComponent<ItemInventory>();
        playerTimerManager = GetComponent<PlayerTimerManager>();

        PlayerSpeed = PlayerDefaultSpeed;
    }

    /// <summary>
    /// スポーン時カメラの参照を取得してターゲットを自分に設定する
    /// </summary>
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;

            //var ui = FindFirstObjectByType<TimerGaugeUI>();
            //ui.SetPlayer(GetComponent<PlayerTimerManager>());

            var timerManager = GetComponent<PlayerTimerManager>();

            // すべての TimerGaugeUI に Player をセット
            foreach (var ui in FindObjectsByType<TimerGaugeUI>(FindObjectsSortMode.None))
            {
                ui.SetPlayer(timerManager);
            }

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
        shiftInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority

        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        

        bool shift= Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // スタミナがあるかどうか
        //bool canDash = playerTimerManager.CanDash();

        IsDashing = shift && isDash;

        if (shiftInput && IsDashing)
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

        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) *
            Runner.DeltaTime * PlayerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
