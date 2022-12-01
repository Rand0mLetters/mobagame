using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Damageable : MonoBehaviourPunCallbacks
{
    Entity myself;

    void Start()
    {
        myself = GetComponent<Entity>();
    }
    
    public void TryTakeDamage(float damage, string ownerID)
    {
        if(TeamController.instance == null) {
            gameObject.SendMessage("TakeDamage", damage);
            return;
        }
        bool onArcadia = TeamController.instance.PlayerOnArcadia(ownerID);
        if (myself.isArcadian != onArcadia)
        {
            gameObject.SendMessage("TakeDamage", damage);
        }
    }
}
