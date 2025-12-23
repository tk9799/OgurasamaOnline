using DG.Tweening;
using Fusion;
using UnityEngine;

public class PrisonDoor : NetworkBehaviour
{
    [Header("PrisonDoorOpenAreaを取得")]
    [SerializeField] private PrisonDoorOpenArea prisonDoorOpenArea;

    [Header("ドアの開閉速度")]
    [SerializeField] private float doorOpenCloseSpeed = 0.0f;

    [Header("左ドアのTransform")]
    [SerializeField] private Transform leftDoorTransform = null;

    [Header("右ドアのTransform")]
    [SerializeField] private Transform rightDoorTransform = null;

    [Header("右ドアの初期位置")]
    [SerializeField] private Vector3 rightDoorInitialPosition = Vector3.zero;

    [Header("左ドアの初期位置")]
    [SerializeField] private Vector3 leftDoorInitialPosition = Vector3.zero;

    [Header("右ドアがX方向に動く距離")]
    [SerializeField] private float rightDoorMoveX = 0.0f;

    [Header("右ドアがZ方向に動く距離")]
    [SerializeField] private float rightDoorMoveZ = 0.0f;

    [Header("左ドアがX方向に動く距離")]
    [SerializeField] private float leftDoorMoveX = 0.0f;

    [Header("左ドアがZ方向に動く距離")]
    [SerializeField] private float leftDoorMoveZ = 0.0f;

    private void Start()
    {

    }

    private void Update()
    {
        if (prisonDoorOpenArea.isInArea)
        {

            leftDoorTransform.transform.DOLocalMove(new Vector3(leftDoorMoveX, leftDoorInitialPosition.y, leftDoorMoveZ), doorOpenCloseSpeed);

            rightDoorTransform.transform.DOLocalMove(new Vector3(rightDoorMoveX, rightDoorInitialPosition.y, rightDoorMoveZ), doorOpenCloseSpeed);


        }
        else
        {
            leftDoorTransform.transform.DOLocalMove(new Vector3(leftDoorInitialPosition.x, leftDoorInitialPosition.y, leftDoorInitialPosition.z), doorOpenCloseSpeed);
            rightDoorTransform.transform.DOLocalMove(new Vector3(rightDoorInitialPosition.x, rightDoorInitialPosition.y, rightDoorInitialPosition.z), doorOpenCloseSpeed);
        }
    }
}
