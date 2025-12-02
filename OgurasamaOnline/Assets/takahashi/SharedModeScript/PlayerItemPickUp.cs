using Fusion;
using UnityEngine;

public class PlayerItemPickUp : NetworkBehaviour
{
    public Camera Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
            if(Physics.Raycast(ray,out RaycastHit hit, 30.0f))
            {
                Debug.Log("ƒAƒCƒeƒ€‰ñŽû");
            }
        }
    }
}
