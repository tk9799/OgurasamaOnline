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

    [Header("ドアがX方向に動く距離")]
    [SerializeField] private float doorMoveX = 0.0f;

    [Header("ドアがZ方向に動く距離")]
    [SerializeField] private float doorMoveZ = 0.0f;

    private void Update()
    {
        if (prisonDoorOpenArea.isInArea)
        {

            leftDoorTransform.transform.DOLocalMove(new Vector3(doorMoveX, 0.0f, doorMoveZ), doorOpenCloseSpeed);

            rightDoorTransform.transform.DOLocalMove(new Vector3(doorMoveX, 0.0f, doorMoveZ), doorOpenCloseSpeed);


        }
        else
        {
            leftDoorTransform.transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), doorOpenCloseSpeed);
            rightDoorTransform.transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), doorOpenCloseSpeed);
        }
    }
}
