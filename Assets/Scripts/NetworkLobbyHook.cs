using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager networkManager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer _lobbyPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        SpritePlayer localPlayer = gamePlayer.GetComponent<SpritePlayer>();

        localPlayer.playerName = _lobbyPlayer.playerName;
        localPlayer.playerColor = _lobbyPlayer.playerColor;
    }
}
