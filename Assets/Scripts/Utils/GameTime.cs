using UnityEngine;

class GameTime : MonoBehaviour {
    public static float time;
    public static float startTime;
    public static bool exists = false;
    private static bool stopped = true;

    public void Awake() {
        if (GameTime.exists) Destroy(this);
    }

    public static void StartGameTime() {
        GameTime.exists = true;
        GameTime.Reset();
        GameTime.startTime = Time.time;
    }

    public static void Kill() {
        GameTime.exists = false;
        GameTime.time = 0;
        GameTime.startTime = Time.time;
        GameTime.stopped = true;
    }

    private void Update() {
        if(!GameTime.stopped) GameTime.time = Time.time - GameTime.startTime;
    }

    public static void Reset() {
        GameTime.time = 0;
        GameTime.startTime = Time.time;
    }
}
