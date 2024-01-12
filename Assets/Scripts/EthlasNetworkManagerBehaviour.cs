using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;

public class EthlasNetworkManagerBehaviour : NetworkManager
{
    [SerializeField]
    Transform leftSpawn;
    [SerializeField]
    Transform rightSpawn;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftSpawn : rightSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        PlayerHealthBehaviour healthBehaviour = player.GetComponent<PlayerHealthBehaviour>();
        healthBehaviour.index = numPlayers;
        NetworkServer.AddPlayerForConnection(conn, player);

    }
}
