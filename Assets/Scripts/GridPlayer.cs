using UnityEngine;

public class GridPlayer : MonoBehaviour {
    public float gridSize = 1;

    public virtual void Move(Vector2 direction) {
        Vector3 destination = transform.position + direction.x * Vector3.right + direction.y * Vector3.up;
        if (Physics.Raycast(destination + Vector3.back * 5, Vector3.forward, out var hit, 10)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                transform.position = destination;
            }
        }
    }
}