using UnityEngine;
using Fusion;

public class OguraSpawner : SimulationBehaviour, IPlayerJoined
{
    // ドアのスポーンとその挙動の確認
    // Tweenは追加してあるので挙動確認を優先
    // 


    public GameObject oguraPrefab = null;
    public GameObject mySlideDoor = null;

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
            Runner.Spawn(mySlideDoor, transform.position, Quaternion.identity);
        }
    }

}
