using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;

    private float moveSpeed = 50f;

    // 
    [SerializeField] private Camera playerCamera;

    [SerializeField]public GameObject CameraPivot;

    //public PlayerFPSCamera playerFPSCamera;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }


    public override void Spawned()
    {
        //if (Object.HasInputAuthority)
        //{
        //    playerCamera.enabled = true;
        //}
        //else
        //{
        //    playerCamera.enabled = false;
        //}
        playerCamera.enabled = Object.HasInputAuthority;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //data.direction.Normalize();
            //_cc.Move(5 * data.direction * Runner.DeltaTime);
            Vector3 camForward = CameraPivot.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = CameraPivot.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            //Vector3 move = transform.right * data.direction.x + transform.forward * data.direction.z;
            Vector3 move = camForward * data.direction.z + camRight * data.direction.x;

            _cc.Move(move.normalized * moveSpeed * Runner.DeltaTime);
        }
    }
}