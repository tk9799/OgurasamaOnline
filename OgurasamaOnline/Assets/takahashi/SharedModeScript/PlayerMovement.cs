using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;

    private ItemInventory itemInventory;

    //カメラのターゲットを設定するためにCamera変数を追加
    public Camera Camera;

    //プレイヤーの現在の速度とデフォルト、ダッシュ時の速度
    public float PlayerSpeed = 0f;
    public float PlayerDefaultSpeed = 2f;
    public float PlayseDashSpeed = 5f;

    //プレイヤーが加速アイテムを使用したときに加算する速度
    private float playerAccelerationSpeed = 5f;

    private void Awake()
    {
        //CharacterControllerとItemInventoryスクリプトを取得
        _controller = GetComponent<CharacterController>();
        itemInventory = GetComponent<ItemInventory>();

        PlayerSpeed = PlayerDefaultSpeed;
    }

    private void Start()
    {
        
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
        }
    }

    public void Update()
    {
        // Update is called once per frame
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            PlayerSpeed = PlayseDashSpeed;
            Debug.Log("ダッシュ中");
        }
        else
        {
            PlayerSpeed = PlayerDefaultSpeed;
        }

        if(itemInventory!=null&& itemInventory.isMoveImprovementItem)
        {
            PlayerSpeed += playerAccelerationSpeed;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority

        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 
            Runner.DeltaTime * PlayerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}
