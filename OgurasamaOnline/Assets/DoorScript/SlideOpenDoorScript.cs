using Fusion;
using UnityEngine;
using DG.Tweening;

public class SlideOpenDoorScript : NetworkBehaviour
{
    [Header("どのキーで反応させるかの指定")]
    [SerializeField] private KeyCode inputKey = KeyCode.None;

    [Header("ドアのRigidbody")]
    [SerializeField] private Rigidbody doorRigidbody = null;


    [Header("プレイヤーのTransform")]
    [SerializeField] private Transform playerTransform = null;

    [Header("ドアがX方向に吹っ飛ぶ最小値")]
    [SerializeField] private float minFlyDoorX = 0.0f;

    [Header("ドアがX方向に吹っ飛ぶ最大値")]
    [SerializeField] private float maxFlyDoorX = 0.0f;

    [Header("ドアがZ方向に吹っ飛ぶ距離")]
    [SerializeField] private float flyDoorZ = 0.0f;

    [Header("ドアのX方向へ動く距離")]
    [SerializeField] private float moveDoorX = 0.0f;

    [Header("ドアのZ方向へ動く距離")]
    [SerializeField] private float moveDoorZ = 0.0f;

    [Header("ドアを開閉する速さ")]
    [SerializeField] private float doorOpenSpeed = 0.0f;

    [Header("ドアが開いた判定")]
    [SerializeField] private bool isDoorOpen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputKey) && !isDoorOpen)
        {
            transform.DOLocalMove(new Vector3(moveDoorX, 0.0f, moveDoorZ), doorOpenSpeed);
            isDoorOpen = true;
        }
        else if (Input.GetKeyDown(inputKey) && isDoorOpen)
        {
            transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), doorOpenSpeed);
            isDoorOpen = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ogura"))
        {
            Quaternion playerFrontRotation = Quaternion.Euler(0.0f, playerTransform.eulerAngles.y, 0.0f);

            transform.DORotate(new Vector3(0, playerTransform.eulerAngles.y, 0), 0.1f);

            doorRigidbody.AddForce(playerFrontRotation * new Vector3(Random.Range(minFlyDoorX, maxFlyDoorX), 0.0f, flyDoorZ), ForceMode.Impulse);

            Destroy(gameObject, 2.0f);
        }
    }

}
