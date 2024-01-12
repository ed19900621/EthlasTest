using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class ScoreBehaviour : NetworkBehaviour
{
    [SyncVar(hook = (nameof(HandleP1ScoreUpdated)))]
    int p1Score;
    [SyncVar(hook = (nameof(HandleP2ScoreUpdated)))]
    int p2Score;
    [SerializeField]
    TextMeshProUGUI p1ScoreText;

    [SerializeField]
    TextMeshProUGUI p2ScoreText;
    [Server]
    public void AssignScore(int index)
    {
        if (index == 0)
        {
            p2Score += 1;
        }
        else
        {
            p1Score += 1;
        }
    }

    private void HandleP1ScoreUpdated(int oldValue, int newValue)
    {
        p1ScoreText.text = "P1 Score:" + p1Score.ToString();
    }

    private void HandleP2ScoreUpdated(int oldValue, int newValue)
    {
        p2ScoreText.text = "P2 Score:" + p2Score.ToString();
    }
}
