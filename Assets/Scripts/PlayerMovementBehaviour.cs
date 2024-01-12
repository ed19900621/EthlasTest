using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementBehaviour : NetworkBehaviour
{
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float speed;
    [SerializeField]
    LayerMask layerMaskforGround;
    Rigidbody2D rb2D;
    CapsuleCollider2D capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    [Client]
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CmdJumpIfOnGround();
        }
        float horizontalMovement = Input.GetAxis("Horizontal");
        CmdWalk(horizontalMovement);
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
    [Command]
    void CmdWalk(float axis)
    {
        //Validate logic to check if correct person
        RpcWalk (axis);
    }
    [ClientRpc]
    void RpcWalk(float axis)
    {
        transform.Translate(transform.right * axis * speed);
    }
}
