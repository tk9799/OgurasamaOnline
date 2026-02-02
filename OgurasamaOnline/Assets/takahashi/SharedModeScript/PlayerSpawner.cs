using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    //private void Start()
    //{
    //    var runner =GetComponent<NetworkRunner>();
    //    runner.AddCallbacks(FindObjectOfType<BasicSpawner>());
    //}

    /// <summary>
    /// プレイヤープレハブのインスタンス化
    /// </summary>
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity,player);
        }
    }
}
