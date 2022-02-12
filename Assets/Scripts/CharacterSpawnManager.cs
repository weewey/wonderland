using Authentication;
using Opsive.UltimateCharacterController.AddOns.Multiplayer.PhotonPun.Game;
using UnityEngine;
using Photon.Realtime;

/// <summary>
/// Manages the character instantiation within a PUN room.
/// </summary>
public class CharacterSpawnManager : SpawnManagerBase
{
    [Tooltip(
        "A reference to the character that PUN should spawn. This character must be setup using the PUN Multiplayer Manager.")]
    [SerializeField]
    protected GameObject m_Character1;

    [SerializeField]
    protected GameObject m_Character2;

    private readonly PlayFabAuthService _authService = PlayFabAuthService.Instance;

    private const string Wallet1 = "4Lz2j3Ga6NeNfiUWfu3FakkVppcfuLsAYMLwq2ELbL6B";
    private const string Wallet2 = "Eqi3aPp3EdTAs9puh4Ze4E5mJ2onzSSr9F94uuZz95qX";

    /// <summary>
    /// Abstract method that allows for a character to be spawned based on the game logic.
    /// </summary>
    /// <param name="newPlayer">The player that entered the room.</param>
    /// <returns>The character prefab that should spawn.</returns>
    protected override GameObject GetCharacterPrefab(Player newPlayer)
    {
        switch (_authService.GetWalletAddress())
        {
            case Wallet1:
                return m_Character1;
            case Wallet2:
                return m_Character2;
        }

        // Default
        return m_Character2;
    }
}