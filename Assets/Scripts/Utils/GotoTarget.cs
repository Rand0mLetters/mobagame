using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoTarget : Photon.Pun.MonoBehaviourPunCallbacks
{
    public Vector3 target;
    void SetTarget(Vector3 t)
    {
        target = t;
    }
    
    void Update()
    {
        transform.position = target;
    }
}
