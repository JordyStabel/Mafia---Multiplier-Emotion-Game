using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager networkManager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer _lobbyPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        Player localPlayer = gamePlayer.GetComponent<Player>();

        localPlayer.playerName = _lobbyPlayer.playerName;
        localPlayer.playerColor = _lobbyPlayer.playerColor;

        Debug.Log("Fired");
    }
}
