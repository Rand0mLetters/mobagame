using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using Photon.Realtime;

public class BuildingHealthController : MonoBehaviourPunCallbacks, IPunObservable
{
    public float health;
    public bool isCapital = false;
    public Slider healthBar;
    public bool isArcadian;

    void Awake()
    {
        if(healthBar) healthBar.maxValue = health;
        if(healthBar) healthBar.value = health;
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(.5f);
        if (TeamController.instance.PlayerOnArcadia(photonView.Owner.UserId)) isArcadian = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;
        if(health <= 0)
        {
            photonView.RPC("TryDestroy", RpcTarget.All);
        }
    }

    [PunRPC]
    public void TryDestroy()
    {
        if (photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId) return;
        if (isCapital)
        {
            // win/lose
            if (ClassicGMController.instance)
            {
                ClassicGMController.instance.RegisterTowerDestroyed(isCapital, isArcadian);
            }
        }
        PhotonNetwork.Destroy(gameObject);
        // effects
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            float streamHealth = (float) stream.ReceiveNext();
            if(streamHealth < health) health = streamHealth;
            if(healthBar) healthBar.value = health;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        // recalculate owner
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if(TeamController.instance.PlayerOnArcadia(newPlayer.UserId) == isArcadian && photonView.IsMine) {
            photonView.TransferOwnership(newPlayer.ActorNumber);
        }
    }
}
