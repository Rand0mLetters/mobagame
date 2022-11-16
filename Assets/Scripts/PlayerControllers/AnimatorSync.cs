using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimatorSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator anim;
    public bool isMine = false;
    public string lastTrigger;
    AnimatorControllerParameter[] parameters;

    void Awake() {
        isMine = photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId;
        parameters = anim.parameters;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting && isMine)
        {
            stream.SendNext(lastTrigger);
        }else if(!stream.IsWriting && !isMine)
        {
            SetTrigger((string)stream.ReceiveNext());
        }
    }

    public void SetTrigger(string trigger)
    {
        if (trigger.Length == 0) return;
        ResetAllTriggers();
        anim.SetTrigger(trigger);
        lastTrigger = trigger;
    }

    public void SetFloat(string varName, float val) {
        anim.SetFloat(varName, val, .1f, Time.deltaTime);
    }

    private void ResetAllTriggers()
    {
        foreach (AnimatorControllerParameter param in parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
}
