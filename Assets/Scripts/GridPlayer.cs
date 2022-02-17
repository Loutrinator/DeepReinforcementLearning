using UnityEngine;

public abstract class GridPlayer : MonoBehaviour {
    public float gridSize = 1;
    [SerializeField] protected LayerMask walkableMask;

    public virtual void Move(Vector2 direction) {
        Vector3 destination = transform.position + (direction.x * Vector3.right + direction.y * Vector3.up) * gridSize;
        if (Physics.Raycast(destination + Vector3.back * 5, Vector3.forward, out var hit, 10)) {
            if (walkableMask == (walkableMask | (1 << hit.collider.gameObject.layer))){
                transform.position = destination;
            }
        }
    }

    public abstract int[][][] GetAllPossibleStates(GameGrid grid);
}