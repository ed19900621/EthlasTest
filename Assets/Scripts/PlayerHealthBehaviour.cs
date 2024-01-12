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
    [SerializeField]
    Slider slider;
    [SerializeField]
    PlayerControlBehaviour playerControlBehaviour;
    [SerializeField]
    Vector3 deadPosition;
    Vector3 startPosition;
    ScoreBehaviour scoreBehaviour;
    public int index;
    public void Start()
    {
        startPosition = transform.position;
        playerHealth = maxHealth;
        scoreBehaviour = GameObject.FindObjectOfType<ScoreBehaviour>();
    }
    [Server]
    public void CmdTakeDamage(int i)
    {
        playerHealth = Mathf.Clamp(playerHealth - i, 0, maxHealth);
        if (playerHealth == 0)
        {
            StartCoroutine(Respawn());
            scoreBehaviour.AssignScore(index);
        }
    }

    private void HandleHealthUpdated(int oldValue, int newValue)
    {
        slider.value = Mathf.Clamp((float)playerHealth / (float)maxHealth,0,1.0f);
    }

    [Server]
    IEnumerator Respawn()
    {
        playerControlBehaviour.enabled = false;
        gameObject.transform.position = deadPosition;
        yield return new WaitForSeconds(1.0f);
        gameObject.transform.position = startPosition;
        playerHealth = maxHealth;
        playerControlBehaviour.enabled = true;

    }
}
