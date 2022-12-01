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
    Entity myself;
    public Renderer[] meshes;
    public Material red;
    public Material blue;
    [Header("=======")]
    public SpawnObject[] objs;

    bool mineLastFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        myself = GetComponent<Entity>();
        mineLastFrame = !myself.isTeammate;
        InvokeRepeating("Calculate", 0.3f, 1f);
    }

    void Calculate() {
        if (myself.isTeammate != mineLastFrame) {
            mineLastFrame = myself.isTeammate;
            Material mat = myself.isTeammate ? blue : red;
            foreach (Renderer r in meshes) {
                try {
                    r.material = mat;
                } catch {
                    try {
                        r.materials = new Material[1];
                        r.materials[0] = mat;
                    } catch {
                        Debug.Log("nICe tRY");
                    }
                }
            }
        }
    }
}
