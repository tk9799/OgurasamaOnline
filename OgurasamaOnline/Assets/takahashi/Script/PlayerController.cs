using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rigidbody;
    //private float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ˆÚ“®“ü—Í‚ÌŽæ“¾
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        //ƒJƒƒ‰‚ÌŒü‚«‚ðŠî€‚É‚µ‚½ˆÚ“®•ûŒü‚ÌŒvŽZ
        Quaternion horizontalRotation = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up);

        //ˆÚ“®•ûŒü‚ÌŒvŽZ
        Vector3 direction = horizontalRotation * input;

        if (direction.sqrMagnitude > 0.01f)
        {
            playerTransform.rotation = Quaternion.LookRotation(direction);
        }

        if (direction.sqrMagnitude < 1.0f)
        {
            direction = direction.normalized;
        }

        direction.y = rigidbody.angularVelocity.y;

        rigidbody.linearVelocity = direction;//‘¬“x‚ÌŒvŽZŒ‹‰Ê‚ðÅŒã‚É‘ã“ü‚·‚é
    }
}
