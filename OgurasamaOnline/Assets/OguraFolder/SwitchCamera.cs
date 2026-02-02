using Fusion;
using UnityEngine;

public class SwitchCamera : NetworkBehaviour
{
    [SerializeField] private OguraController oguraController = null;



    [Header("全体マップのオブジェクト取得")]
    [SerializeField] private Camera fullMapCamera = null;

    

    [Header("マップ切り替えの判定")]
    [SerializeField] public bool isFullMap = false;

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isFullMap)
            {
                isFullMap = true;
                fullMapCamera.enabled = true;
                oguraController.miniMapCamera.enabled = false;
            }
            else
            {
                isFullMap = false;
                fullMapCamera.enabled = false;
                oguraController.miniMapCamera.enabled = true;
            }
        }
    }
}
