using Fusion;
using UnityEngine;

/// <summary>
/// Fusion‚Ì“ü—Í\‘¢
/// </summary>
public struct PlayerInputData : INetworkInput
{
    public Vector2 move;
    public Vector2 look;
    public NetworkBool Dash;
}
