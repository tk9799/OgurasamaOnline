using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 20.0f;

    float yaw;
    float pitch;

    /// <summary>
    /// カーソルを非表示にするとゲーム実行時操作ができなくなるためコメントアウトさせている
    /// </summary>
    //private void Start()
    //{
    //    Cursor.visible = false;

    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    public void SetLookRotation(float y, float p)
    {
        yaw = y;
        pitch = p;
    }

    public void AddLookInput(Vector2 look)
    {
        Debug.Log($"LOOK INPUT: {look}");
        yaw += look.x * MouseSensitivity;
        pitch -= look.y * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
    }

    void LateUpdate()
    {
        if (Target == null)
        {

            var localPlayer = FindFirstObjectByType<PlayerMovement>();
            if (localPlayer != null && localPlayer.Object.HasInputAuthority)
            {
                Target = localPlayer.transform;
            }
            else return;

        }
        else
        {
            var localPlayer = FindFirstObjectByType<PlayerMovement>();
            if (localPlayer != null && localPlayer.Object.HasInputAuthority)
            {
                Target = localPlayer.transform;
            }
            else return;
        }

        transform.position = Target.position;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //transform.position = Target.position;

        //transform.rotation=Quaternion.Euler(verticalRotation, horizontalRotation, 0);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * MouseSensitivity;
        pitch -= mouseY * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, -70f, 70f);

        //horizontalRotation += mouseX * MouseSensitivity;

        //transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}
