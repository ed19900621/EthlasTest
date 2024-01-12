using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerHealthBehaviour : NetworkBehaviour
{
    [SyncVar(hook = (nameof(HandleHealthUpdated)))]
    int playerHealth;
    [SerializeField]
    int maxHealth;
    public Slider slider;

    public void Start()
    {

        playerHealth = maxHealth;
        //base.OnStartServer();
    }
    [Server]
    public void CmdTakeDamage(int i)
    {
        playerHealth -= i;
        //slider.value = (float)playerHealth / (float)maxHealth;
    }

    private void HandleHealthUpdated(int oldValue, int newValue)
    {
        slider.value = (float)playerHealth / (float)maxHealth;
    }
}
