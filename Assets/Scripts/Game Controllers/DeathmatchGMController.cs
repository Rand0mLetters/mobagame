using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AddressableAssets;

public class DeathmatchGMController : MonoBehaviourPunCallbacks, IPunObservable
{
    public AssetReference sceneIndex;
    int arcadiaPoints;
    int xanaduPoints;
    float timeLeft;
    public float lengthTime = 200;
    float startTime;
    public static DeathmatchGMController instance;
    public TMPro.TextMeshProUGUI timeDisplay;
    public TMPro.TextMeshProUGUI playerScore;
    public TMPro.TextMeshProUGUI opponentScore;
    public GameObject postGameData;
    bool isArcadia;

    void Awake() {
        instance = this;
        startTime = Time.time;
        UpdateTimeDisplay();
        Instantiate(postGameData);
        isArcadia = TeamController.instance.PlayerOnArcadia(PhotonNetwork.LocalPlayer.UserId);
    }

    public void RegisterDeath(string playerNumber)
    {
        photonView.RPC("Point", RpcTarget.All, playerNumber);
    }

    void Update()
    {
        timeLeft = lengthTime - (Time.time - startTime);
        UpdateTimeDisplay();
        if(timeLeft <= 0)
        {
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeLeft);
        }
        else
        {
            timeLeft = (float) stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Point(string playerNumber)
    {
        if (TeamController.instance.PlayerOnArcadia(playerNumber))
        {
            xanaduPoints += 1;
            PostgameData.instance.xanaduScore = xanaduPoints;
            if (isArcadia)
            {
                opponentScore.text = xanaduPoints.ToString();
            }
            else
            {
                playerScore.text = xanaduPoints.ToString();
            }
        }
        else
        {
            arcadiaPoints += 1;
            PostgameData.instance.arcadiaScore = arcadiaPoints;
            if (isArcadia)
            {
                playerScore.text = arcadiaPoints.ToString();
            }
            else
            {
                opponentScore.text = arcadiaPoints.ToString();
            }
        }
    }

    void UpdateTimeDisplay()
    {
        int seconds = (int) (timeLeft % 60);
        timeDisplay.text = (timeLeft - (timeLeft % 60))/60 + ":";
        if(seconds < 10)
        {
            timeDisplay.text += "0";
        }
        timeDisplay.text += seconds.ToString();
    }

    [PunRPC]
    public void EndGame()
    {
        PhotonNetwork.LeaveRoom();
        bool playerOnArcadia = TeamController.instance.PlayerOnArcadia(PhotonNetwork.LocalPlayer.UserId);
        PostgameData.instance.playerOnArcadia = playerOnArcadia;
        if ((playerOnArcadia && arcadiaPoints > xanaduPoints) || (!playerOnArcadia && xanaduPoints > arcadiaPoints)){
            PostgameData.instance.won = true;
        }else if(arcadiaPoints == xanaduPoints)
        {
            PostgameData.instance.tie = true;
        }
        sceneIndex.LoadSceneAsync();
    }
}
