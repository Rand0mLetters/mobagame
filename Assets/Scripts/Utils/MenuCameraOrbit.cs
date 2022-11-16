using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCameraOrbit : MonoBehaviour
{
    
    void Update()
    {
        Vector2 mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, .1f));
        transform.LookAt(worldPos.normalized);
    }
}
