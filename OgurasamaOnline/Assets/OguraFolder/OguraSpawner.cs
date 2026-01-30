using UnityEngine;
using Fusion;

public class OguraSpawner : SimulationBehaviour, IPlayerJoined
{
    // ドアのスポーンとその挙動の確認
    // Tweenは追加してあるので挙動確認を優先
    // 


    public GameObject oguraPrefab = null;
    //public GameObject mySlideDoor = null;
    public Vector3 oguraSpawnPosition = new Vector3(-174.639f, 10, 99);

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
            Runner.Spawn(oguraPrefab, oguraSpawnPosition, Quaternion.identity);
           // Runner.Spawn(mySlideDoor, transform.position, Quaternion.identity);
        }
    }
}
