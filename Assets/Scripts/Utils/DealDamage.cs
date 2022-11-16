using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DealDamage : MonoBehaviourPunCallbacks, IPunObservable
{
    public float damage;
    public bool recurring;
    public bool canDamageLocalPlayer = false;
    public float repeatTime;
    bool canDealDamage = true;
    bool coroutineStarted = false;


    // doesn't work
    private void OnTriggerEnter(Collider other)
    {
        if (recurring) return;
        Damageable target = other.gameObject.GetComponent<Damageable>();
        if (target)
        {
            target.TryTakeDamage(damage, photonView.Owner.UserId);
        }
    }


    // it works nowwww
    private void OnTriggerStay(Collider other)
    {
        if (!recurring || !canDealDamage)
        {
            return;
        }
        Damageable target = other.gameObject.GetComponent<Damageable>();
        if (target)
        {
            target.TryTakeDamage(damage, photonView.Owner.UserId);
            if (!coroutineStarted)
            {
                StartCoroutine("Cooldown");
            }
        }
    }

    IEnumerator Cooldown()
    {
        coroutineStarted = true;
        yield return new WaitForEndOfFrame();
        canDealDamage = false;
        yield return new WaitForSeconds(repeatTime);
        canDealDamage = true;
        coroutineStarted = false;
    }


    // just don't ask
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(coroutineStarted);
        }
        else
        {
            bool newValue = (bool) stream.ReceiveNext();
            if (newValue && !coroutineStarted)
            {
                StartCoroutine("Cooldown");
            }
        }
    }
}
