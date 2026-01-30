using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("全体マップのオブジェクト取得")]
    [SerializeField] private GameObject fullMapCamera = null;

    [Header("ミニマップを取得")]
    [SerializeField] private GameObject miniMapCamera = null;

    [Header("マップ切り替えの判定")]
    [SerializeField] private bool isFullMap = false;

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
                fullMapCamera.SetActive(true);
                miniMapCamera.SetActive(false);
            }
            else
            {
                isFullMap = false;
                fullMapCamera.SetActive(false);
                miniMapCamera.SetActive(true);
            }
        }
    }
}
