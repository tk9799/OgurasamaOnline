using Fusion;
using UnityEngine;

public class JailTeleport : NetworkBehaviour
{
    //牢屋の座標
    private Transform jailTransform;

    private bool isCaught = false;

    public override void Spawned()
    {
        //JailManagerから牢屋のTransformを取得
        jailTransform = JailManager.Instance.jailTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCaught)
        {
            return;
        }

        if(other.CompareTag("Enemy"))
        {
            //Debug.Log("hit");
            RPC_RequestJail();
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestJail()
    {
        //Debug.Log("hit");
        if (isCaught)
        {
            return;
        }

        isCaught = true;
        TeleportToJail();
        Debug.Log(isCaught);
    }

    /// <summary>
    /// プレイヤーを牢屋にテレポートするメソッド
    /// </summary>
    public void TeleportToJail()
    {
        //StateAuthorityを持っている場合のみ処理を行う
        //このオブジェクトが状態を変更する権限があるか確認
        if (!Object.HasStateAuthority)
        {
            return;
        }

        //プレイヤーを牢屋の位置にテレポートさせる
        transform.position = jailTransform.position;
    }
}
