using UnityEngine;

public class CameraController : MonoBehaviour
{
    //プレイヤーのトランスフォーム
    [SerializeField] private Transform playerTransform;

    //カメラのトランスフォーム
    [SerializeField] private Transform cameraTransform;
    [Header("カメラ感度")]
    [SerializeField] private float mouseSensitivity = 1.0f;

    //カメラの回転量
    private float yaw = 0f;
    private float pitch = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //カーソル非表示
        Cursor.visible = false;

        //カーソルが画面買いに出ないようにする
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Updateが終わった後に処理するUpdate
    /// </summary>
    void LateUpdate()
    {
        //カメラをプレイヤーの位置に合わせる
        cameraTransform.position = playerTransform.position;

        //マウスの移動距離を取得してカメラの回転量に加算する
        //yawは左右の回転量、pitchは上下の回転量
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        //角度の制限
        //一定の角度を超えなくするため制限する
        pitch = Mathf.Clamp(pitch, -60.0f, 60.0f);

        //マウスの移動量に応じてカメラを回転させる
        cameraTransform.eulerAngles=new Vector3(pitch, yaw, 0.0f);
    }
}
