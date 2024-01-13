using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EthlasNetworkManagerBehaviour : NetworkRoomManager
{
    /*[SerializeField]
    Transform leftSpawn;
    [SerializeField]
    Transform rightSpawn;
    */
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        PlayerHealthBehaviour healthBehaviour = gamePlayer.GetComponent<PlayerHealthBehaviour>();
        healthBehaviour.SetIndex(roomPlayer.GetComponent<NetworkRoomPlayer>().index);
        return true;
    }
    protected override void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Debug.Log($"NetworkRoom SceneLoadedForPlayer scene: {SceneManager.GetActiveScene().path} {conn}");

        if (Utils.IsSceneActive(RoomScene))
        {
            // cant be ready in room, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            return;
        }

        GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);
        if (gamePlayer == null)
        {
            // get start position from base class
            Transform startPos = startPositions[roomPlayer.GetComponent<NetworkRoomPlayer>().index];
            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }
    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        if (Utils.IsHeadless())
        {
            base.OnRoomServerPlayersReady();
        }
        else
        {
            showStartButton = true;
        }
    }

    public void BackToRoom()
    {
        ServerChangeScene(RoomScene);
    }


    public override void OnGUI()
    {
        if (!showRoomGUI)
            return;

        if (Utils.IsSceneActive(RoomScene))
            GUI.Box(new Rect(10f, 180f, 520f, 150f), "PLAYERS");

        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
}
