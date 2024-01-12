using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponBehaviour : NetworkBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform bulletSpawnPoint;
    [SerializeField]
    float coolDown;
    float timer;
    [SerializeField]
    Color baseColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FireWeapon()
    {
        Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
    }
    public void ChangeToBaseColor(bool b)
    {
        spriteRenderer.color = b ? baseColor : Color.clear;
    }
}
