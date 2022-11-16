using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolData
{
    public GameObject go;
    public int numberToSpawn;
    public List<GameObject> spawned;

    public GameObject getGameObject()
    {
        foreach(GameObject g in spawned)
        {
            if(g.activeSelf == false)
            {
                g.SetActive(true);
                return g;
            }
        }
        return MonoBehaviour.Instantiate(go);
    }
}

public class GameObjectPooler : MonoBehaviour
{
    public static GameObjectPooler instance;
    public PoolData[] poolDatas;

    private void Awake()
    {
        instance = this;
        foreach(PoolData pd in poolDatas)
        {
            for(int i = 0; i < pd.numberToSpawn; i++)
            {
                GameObject g = Instantiate(pd.go, Vector3.zero, Quaternion.identity);
                g.SetActive(false);
                pd.spawned.Add(g);
            }
        }
    }

    public GameObject Instantiate(GameObject g)
    {
        foreach(PoolData poolData in poolDatas)
        {
            if(poolData.go.name == g.name) return poolData.getGameObject();
        }
        return Instantiate(g);
    }

    public void Destroy(GameObject g)
    {
        g.SetActive(false);
    }

    public void DestroyAfterTime(GameObject g, float time)
    {
        StartCoroutine("DestroyG", new object[] {g, time});
    }

    private IEnumerator DestroyG(object[] p)
    {
        yield return new WaitForSeconds((float) p[1]);
        this.Destroy((GameObject) p[0]);
    }
}
