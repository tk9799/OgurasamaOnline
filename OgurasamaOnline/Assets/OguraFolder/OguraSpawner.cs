using UnityEngine;
using Fusion;

public class OguraSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject oguraPrefab = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(oguraPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

}
