using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SyncVar(hook = (nameof(HandleTimerUpdated)))]
    float timer;
    [SerializeField]
    Color baseColor;
    [SerializeField]
    Color onCoolDownColor;
    [SerializeField]
    Image cooldownUI;
    // Start is called before the first frame update
    void Start()
    {

    }
    [Server]
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }

    public void FireWeapon(bool b)
    {
        if (timer <= 0)
        {
            timer = coolDown;
            GameObject bulletGO = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
            BulletBehaviour bulletBehaviour = bulletGO.GetComponent<BulletBehaviour>();
            bulletBehaviour.SetIsFlyingLeft(b);
            NetworkServer.Spawn(bulletGO);
        }
    }
    public void ChangeToBaseColor(bool b)
    {
        spriteRenderer.color = b ? baseColor : Color.clear;
    }

    private void HandleTimerUpdated(float oldValue, float newValue)
    {
        cooldownUI.fillAmount = Mathf.Clamp((coolDown - timer) / coolDown, 0, 1.0f);
        cooldownUI.color = cooldownUI.fillAmount < 1 ? onCoolDownColor : baseColor;
    }
}
