using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    int damage;
    bool isFlyingLeft;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [Server]
    // Update is called once per frame
    void Update()
    {
        if (isFlyingLeft)
        {
            transform.Translate(transform.right * speed * -1);
        }
        else
        {
            transform.Translate(transform.right * speed);
        }
    }

    public void SetIsFlyingLeft(bool b)
    {
        isFlyingLeft = b;
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Hit Player. Take damage");
            }
        }
        NetworkServer.Destroy(this.gameObject);
    }
}
