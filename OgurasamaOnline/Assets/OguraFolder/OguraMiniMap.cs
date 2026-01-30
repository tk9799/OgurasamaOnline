using Fusion;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class OguraMiniMap : NetworkBehaviour
{
    // 追従対象オブジェクト
    public Transform Target;

    private OguraController oguraController = null;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        

        if(Input.GetKeyDown(KeyCode.Q))
        {

        }

    }
}
