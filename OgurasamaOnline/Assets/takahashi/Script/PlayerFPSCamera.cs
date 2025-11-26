using UnityEngine;
using Fusion;

public class PlayerFPSCamera : NetworkBehaviour
{
    //カメラの親オブジェクト
    [SerializeField] private Transform cameraRoot;

    [SerializeField] private float sensitivity = 3.0f;

    private float pitch = 0.0f;//上下
    private float yaw = 0.0f;//左右

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            //他のプレイヤーはカメラを操作できない
            enabled = false;
        }
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

        //左右回転（キャラ本体）
        //transform.Rotate(Vector3.up * mouseX);

        //上下回転

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        //rotationX = Mathf.Clamp(rotationY, -80f, 80f);

        cameraRoot.localRotation=Quaternion.Euler(pitch, 0f, 0f);
    }
}
