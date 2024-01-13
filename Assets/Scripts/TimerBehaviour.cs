using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class TimerBehaviour : NetworkBehaviour
{
    [SerializeField]
    float startGameTimerInitialValue;
    [SerializeField]
    float actualGameTimerInitialValue;
    [SerializeField]
    float endGameTimerInitialValue;
    [SyncVar(hook = (nameof(HandleStartGameTimerUpdated)))]
    float startGameTimer;
    [SyncVar(hook = (nameof(HandleActualGameTimerUpdated)))]
    float actualGameTimer;
    [SyncVar(hook = (nameof(HandleEndGameTimerUpdated)))]
    float endGameTimer;
    [SerializeField]
    TextMeshProUGUI startGameTimerText;
    [SerializeField]
    TextMeshProUGUI actualGameTimerText;
    [SerializeField]
    TextMeshProUGUI endGameTimerText;
    [SyncVar]
    bool playersCanMove;
    ScoreBehaviour scoreBehaviour;
    EthlasNetworkManagerBehaviour ethlasNetworkManagerBehaviour;
    [Server]
    // Start is called before the first frame update
    void Start()
    {
        startGameTimer = startGameTimerInitialValue;
        actualGameTimer = actualGameTimerInitialValue;
        endGameTimer = endGameTimerInitialValue;
        ethlasNetworkManagerBehaviour = FindObjectOfType<EthlasNetworkManagerBehaviour>();
    }
    [Server]
    // Update is called once per frame
    void Update()
    {
        if (startGameTimer > 0)
        {
            startGameTimer -= Time.deltaTime;
            playersCanMove = false;
        }
        if (startGameTimer <= 0 && actualGameTimer > 0)
        {
            actualGameTimer -= Time.deltaTime;
            playersCanMove = true;
        }
        if (actualGameTimer <= 0 && endGameTimer > 0)
        {
            endGameTimer -= Time.deltaTime;
            playersCanMove = false;
        }
        if (endGameTimer <= 0)
        {
            //Reset Room
            ethlasNetworkManagerBehaviour.BackToRoom();
            this.enabled = false;
        }
    }

    private void HandleStartGameTimerUpdated(float oldValue, float newValue)
    {
        startGameTimerText.gameObject.SetActive(startGameTimer > 0);
        float floor = Mathf.Floor(startGameTimer);
        if (floor < 1)
        {
            startGameTimerText.text = "START!";
        }
        else
        {
            startGameTimerText.text = floor.ToString();
        }
    }
    private void HandleActualGameTimerUpdated(float oldValue, float newValue)
    {
        actualGameTimerText.gameObject.SetActive(startGameTimer <= 0 && actualGameTimer > 0);
        float ciel = Mathf.Clamp(Mathf.Ceil(actualGameTimer), 0, actualGameTimerInitialValue);
        actualGameTimerText.text = ciel.ToString();
    }
    private void HandleEndGameTimerUpdated(float oldValue, float newValue)
    {

        endGameTimerText.gameObject.SetActive(actualGameTimer <= 0 && endGameTimer > 0);
        float ciel = Mathf.Clamp(Mathf.Ceil(endGameTimer), 0, endGameTimerInitialValue);
        if (scoreBehaviour == null)
        {
            scoreBehaviour = FindObjectOfType<ScoreBehaviour>();
        }
        if (scoreBehaviour != null)
        {
            endGameTimerText.text = scoreBehaviour.DetermineWinner() + "\n\nReturning to Room in " + ciel.ToString();
        }
    }

    public bool GetPlayersCanMove()
    {
        return playersCanMove;
    }
}
