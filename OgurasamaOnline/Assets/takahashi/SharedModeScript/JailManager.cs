using UnityEngine;

public class JailManager : MonoBehaviour
{
    public static JailManager Instance{get;private set; }

    public Transform jailTransform;

    private void Awake()
    {
        Instance = this;
    }
}
