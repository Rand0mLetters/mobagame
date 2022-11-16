using UnityEngine;

public class PostgameData : MonoBehaviour
{
    public bool won = false;
    public bool tie = false;
    public int arcadiaScore = 0;
    public int xanaduScore = 0;
    public bool playerOnArcadia;
    public static PostgameData instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
