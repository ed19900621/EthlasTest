using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomBehaviour : MonoBehaviour
{

    NetworkManager manager;
    // Start is called before the first frame update

    void Awake()
    {
        manager = GameObject.FindObjectOfType<NetworkManager>();
    }

    public void QuitRoom()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        else if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }
}
