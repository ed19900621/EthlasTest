using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponBehaviour : NetworkBehaviour
{
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform bulletSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Command]
    void CmdFireWeapon()
    {
        //Validate logic to check if correct person
        RpcFireWeapon();
    }
    [ClientRpc]
    public void RpcFireWeapon()
    {
        Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
    }
}
