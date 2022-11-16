using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostgameController : MonoBehaviour
{
    public TextMeshProUGUI winOrLose;
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI opponentScore;

    void Start()
    {
        if (PostgameData.instance.won)
        {
            winOrLose.text = "Victory!";
        }else if (PostgameData.instance.tie)
        {
            winOrLose.text = "Tie";
        }
        else
        {
            winOrLose.text = "Defeat";
        }
        bool playerOnArcadia = PostgameData.instance.playerOnArcadia;
        if (playerOnArcadia)
        {
            playerScore.text = PostgameData.instance.arcadiaScore.ToString();
            opponentScore.text = PostgameData.instance.xanaduScore.ToString();
        }
        else
        {
            playerScore.text = PostgameData.instance.xanaduScore.ToString();
            opponentScore.text = PostgameData.instance.arcadiaScore.ToString();
        }
    }

    public void DestroyData()
    {
        Destroy(PostgameData.instance.gameObject);
    }
}
