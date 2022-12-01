using UnityEngine;

public class PointAtCamera : MonoBehaviour
{
    Camera cam;
    private void Start() {
        cam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}
