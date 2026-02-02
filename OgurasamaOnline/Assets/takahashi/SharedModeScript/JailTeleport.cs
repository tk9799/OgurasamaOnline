using Fusion;
using UnityEngine;

public class JailTeleport : NetworkBehaviour
{
    //牢屋の座標
    private Transform jailTransform;

    //牢屋に捕まったかどうかの判定
    private bool isCaught = false;

    private NetworkCharacterController networkCharacterController;

    public override void Spawned()
    {
        //Debug.Log($"JailManager.Instance = {JailManager.Instance}");

        //牢屋のTransformを取得
        //staticを使ったクラスであるためクラス名を書いただけでアクセス可能
        jailTransform = JailManager.Instance.jailTransform;

        //NetworkCharacterControllerがプレイヤーの子オブジェクトにあるためGetComponentInChildrenで取得する
        networkCharacterController = GetComponentInChildren<NetworkCharacterController>();

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCaught)
        {
            return;
        }

        //敵に触れたら牢屋にテレポートするRPCを呼び出す
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("hit");
            RPC_RequestJail();
        }
        
    }

    /// <summary>
    /// HasStateAuthority側で実行するRPCメソッド
    /// オブジェクト権限が持っているものしか実行できなく、
    /// Fusionでの唯一の位置変更できる方法
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
    /// 牢屋の座標を確保するメソッド
    /// </summary>
    private void EnsureJailTransform()
    {
        if (jailTransform != null) return;

        if (JailManager.Instance != null)
            jailTransform = JailManager.Instance.jailTransform;
    }

    /// <summary>
    /// プレイヤーを牢屋にテレポートするメソッド
    /// </summary>
    public void TeleportToJail()
    {
        if (!Object.HasStateAuthority)
        {
            return;
        }

        Debug.Log("メソッド呼び出し");
        //StateAuthorityを持っている場合のみ処理を行う
        //このオブジェクトが状態を変更する権限があるか確認
        if (!Object.HasStateAuthority)
        {
            Debug.Log("権限なし");
            return;
        }

        EnsureJailTransform();

        if (networkCharacterController == null)
        {
            Debug.LogError("NetworkCharacterControllerが見つからない！");
            return;
        }

        if (jailTransform == null)
        {
            Debug.LogError("jailTransformが取得できていない！");
            return;
        }

        //プレイヤーを牢屋の位置にテレポートさせる
        //CharacterControllerで物理的な当たり判定、NetworkCharacterController
        //でネットワーク同期と移動計算
        networkCharacterController.Teleport(jailTransform.position);
    }
}
