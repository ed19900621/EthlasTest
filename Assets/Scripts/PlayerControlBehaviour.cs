using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControlBehaviour : NetworkBehaviour
{
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float speed;
    [SerializeField]
    LayerMask layerMaskforGround;
    Rigidbody2D rb2D;
    CapsuleCollider2D capsuleCollider;
    bool isFacingLeft;
    [SerializeField]
    WeaponBehaviour[] weaponBehaviours;
    [SyncVar(hook = (nameof(HandleMainWeaponIndexUpdated)))]
    int mainWeaponIndex;
    TimerBehaviour timerBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>(); 
        for (int i = 0; i < weaponBehaviours.Length; i++)
        {
            weaponBehaviours[i].ChangeToBaseColor(mainWeaponIndex == i);
        }
        timerBehaviour = GameObject.FindObjectOfType<TimerBehaviour>();
    }
    [Client]
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (timerBehaviour.GetPlayersCanMove() == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CmdJumpIfOnGround();
        }
        float horizontalMovement = Input.GetAxis("Horizontal");
        CmdWalk(horizontalMovement);
        if (Input.GetKeyDown(KeyCode.I))
        {
            CmdSwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            CmdSwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            CmdSwitchWeapon(2);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            CmdFireWeapon();
        }
    }
    [Command]
    void CmdJumpIfOnGround()
    {
        //Validate logic to check if correct person
        RpcJumpIfOnGround();
    }
    [ClientRpc]
    void RpcJumpIfOnGround()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -transform.up, (capsuleCollider.size.y/2 + 0.01f), layerMaskforGround.value);
        
        Debug.Log(raycastHit2D);
        if (raycastHit2D.collider != null)
        {
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void SetisFacingLeft(bool b)
    {
        isFacingLeft = b;
        if (isFacingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
    [Command]
    void CmdWalk(float axis)
    {
        //Validate logic to check if correct person
        RpcWalk (axis);
    }
    [ClientRpc]
    void RpcWalk(float axis)
    {
        if (axis > 0)
        {
            isFacingLeft = false;
        }
        else if (axis < 0)
        {
            isFacingLeft = true;
        }
        if (isFacingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        transform.Translate(transform.right * axis * speed);
    }
    [Command]
    void CmdSwitchWeapon(int weaponIndex)
    {
        mainWeaponIndex = weaponIndex;
        //Validate logic to check if correct person
        RpcSwitchWeapon(mainWeaponIndex);
    }
    [ClientRpc]
    void RpcSwitchWeapon(int weaponIndex)
    {
        //mainWeaponIndex = weaponIndex;
        /*for (int i = 0; i < weaponBehaviours.Length; i++)
        {
            weaponBehaviours[i].ChangeToBaseColor(mainWeaponIndex == i);
        }*/
    }
    [Command]
    void CmdFireWeapon()
    {
        weaponBehaviours[mainWeaponIndex].FireWeapon(isFacingLeft);
        //Validate logic to check if correct person
        //RpcFireWeapon();
    }
    [ClientRpc]
    void RpcFireWeapon()
    {
        //weaponBehaviours[mainWeaponIndex]
    }

    private void HandleMainWeaponIndexUpdated(int oldValue, int newValue)
    {
        for (int i = 0; i < weaponBehaviours.Length; i++)
        {
            weaponBehaviours[i].ChangeToBaseColor(mainWeaponIndex == i);
        }
    }
}
