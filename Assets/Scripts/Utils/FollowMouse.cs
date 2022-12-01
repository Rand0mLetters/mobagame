using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class FollowMouse : MonoBehaviour {
    public LayerMask mask;
    public Transform aimIndicator;
    System.Action<RaycastHit> cb;
    Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    public void Update() {
        RaycastHit hit;
        Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        Ray ray = cam.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, mask.value, QueryTriggerInteraction.Ignore)) {
            aimIndicator.position = hit.point;
        }
        if (Input.GetMouseButtonDown(0)) {
            Physics.Raycast(ray, out hit, 1000, mask.value, QueryTriggerInteraction.Ignore);
            cb(hit);
        }
    }

    public void BindCallback(System.Action<RaycastHit> action) {
        cb = action;
    }
}