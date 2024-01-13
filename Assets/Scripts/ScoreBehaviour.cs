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

    public string DetermineWinner()
    {
        if (p1Score > p2Score)
        {
            return "P1 Wins. ";
        }
        else if (p2Score > p1Score)
        {

            return "P2 Wins. ";
        }
        else
        {
            return "It's a Draw. ";
        }
    }

    private void HandleP1ScoreUpdated(int oldValue, int newValue)
    {
        p1ScoreText.text = "P1 Score: " + p1Score.ToString();
    }

    private void HandleP2ScoreUpdated(int oldValue, int newValue)
    {
        p2ScoreText.text = "P2 Score: " + p2Score.ToString();
    }
}
