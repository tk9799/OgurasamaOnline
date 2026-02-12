using Fusion;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    [SerializeField] private OguraController oguraController = null;
    [SerializeField] private Camera miniMapCamera = null;
    [SerializeField] private Camera secondCamera = null;

    private void Start()
    {
        miniMapCamera.enabled = true;
        secondCamera.enabled = false;
    }

    private void Update()
    {
        if (oguraController.isCameraSwitch)
        {
            miniMapCamera.enabled = false;
            secondCamera.enabled = true;
        }
        else
        {
            miniMapCamera.enabled = true;
            secondCamera.enabled = false;
        }
    }


}
