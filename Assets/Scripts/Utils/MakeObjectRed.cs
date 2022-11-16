using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class SpawnObject
{
    public Transform spawnPos;
    public GameObject blue;
    public GameObject red;
}

public class MakeObjectRed : MonoBehaviourPunCallbacks
{
    public Renderer[] meshes;
    public Material material;
    [Header("=======")]
    public SpawnObject[] objs;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        bool objTeam = TeamController.instance ? TeamController.instance.PlayerOnArcadia(photonView.Owner.UserId) : false;
        bool myTeam = TeamController.instance ? TeamController.instance.PlayerOnArcadia(PhotonNetwork.LocalPlayer.UserId) : true;
        if(photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId && objTeam != myTeam)
        {
            foreach(Renderer r in meshes)
            {
                try
                {
                    r.material = material;
                }
                catch
                {
                    try
                    {
                        r.materials = new Material[1];
                        r.materials[0] = material;
                    }
                    catch
                    {
                        Debug.Log("nICe tRY");
                    }
                }
            }
        }


        bool spawnRed = photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId && objTeam != myTeam;
        foreach(SpawnObject obj in objs)
        {
            if (spawnRed) Instantiate(obj.red, obj.spawnPos.position, obj.spawnPos.rotation, obj.spawnPos);
            else Instantiate(obj.blue, obj.spawnPos.position, obj.spawnPos.rotation, obj.spawnPos);
        }
    }
}
