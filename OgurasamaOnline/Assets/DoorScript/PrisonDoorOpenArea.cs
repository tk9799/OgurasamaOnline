using Fusion;
using UnityEngine;

public class PrisonDoorOpenArea : NetworkBehaviour
{
    [Header("ƒGƒŠƒA“à‚É‚¢‚é‚©‚Ç‚¤‚©‚Ì”»’è")]
    [SerializeField] public bool isInArea = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInArea = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInArea = false;
        }
    }
}
