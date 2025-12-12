using Fusion;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// アイテムを収集したり管理したりするスクリプト
/// </summary>
public class PlayerItemPickUp : NetworkBehaviour
{
    public Camera Camera;
    private List<NetworkObject> pickUpOojectList=new List<NetworkObject>();
    public ItemInventory itemInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Camera = GetComponent<Camera>();
        Camera = Camera.main;
        Camera.GetComponent<FirstPersonCamera>().Target = transform;

        itemInventory = GetComponent<ItemInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f,0));
            if(Physics.Raycast(ray,out RaycastHit hit, 3000.0f))
            {
                GameObject hitObject = hit.collider.gameObject;
                //Runner.Despawn(hitObject);
                Debug.Log(hitObject);

                //rayに当たったオブジェクトはNetworkObjectを取得する
                NetworkObject networkObject = hitObject.GetComponent<NetworkObject>();

                if (networkObject != null)
                {
                    //if (Object.HasStateAuthority)
                    if (hit.collider.TryGetComponent<Item>(out var item))
                    {
                        //デスポーンする前にアイテムとしてリストに登録
                        //pickUpOojectList.Add(networkObject);

                        //アイテムをインベントリに追加する
                        //データだけ保存するという意味でもある
                        itemInventory.AddItem(item.data);
                        Debug.Log(item.data);

                        //オブジェクトをデスポーンする
                        Runner.Despawn(networkObject);
                        Debug.Log("オブジェクトをデスポーン");
                    }
                    else
                    {
                        Debug.Log("StateAuthorityを持っていないためデスポーンできません");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(Camera == null) return;

        Ray ray =Camera.ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f,0));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
