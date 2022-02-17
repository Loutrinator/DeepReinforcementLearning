using UnityEngine;

namespace Sokoban
{
    public class SokobanPlayer : GridPlayer
    {
        [SerializeField] private LayerMask crateMask;
        public void Move(Vector2 direction)
        {
            Vector3 dir = (direction.x * transform.right + direction.y * transform.up) * gridSize;
            Vector3 destination = transform.position + dir;
            if (Physics.Raycast(destination - transform.forward * 5, transform.forward, out var hit, 10)) {
                if (walkableMask == (walkableMask | (1 << hit.collider.gameObject.layer)))
                {
                    Debug.Log("Move player");
                    transform.position = destination;
                }
                if(crateMask == (crateMask | (1 << hit.collider.gameObject.layer)))
                {
                    Vector3 crateDestination = hit.transform.position + dir;
                    if (Physics.Raycast(crateDestination - transform.forward * 5, transform.forward, out var hit2, 10))
                    {

                        if (walkableMask == (walkableMask | (1 << hit2.collider.gameObject.layer)))
                        {
                            Debug.Log("Crate moves");
                            hit.transform.position += dir;
                            transform.position = destination;
                        }
                    }
                }
            }
        }
    }
}