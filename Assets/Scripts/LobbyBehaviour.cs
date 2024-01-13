using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LobbyBehaviour : MonoBehaviour
{

    NetworkManager manager;
    // Start is called before the first frame update
    void Awake()
    {

        manager = GameObject.FindObjectOfType<NetworkManager>();
    }

    public void Host()
    {
        manager.StartHost();
    }

    public void SetIP(string ip)
    {
        manager.networkAddress = ip;
    }

    public void Join()
    {
        manager.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
