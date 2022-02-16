using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour {
    public UnityEvent<Vector2> onMove;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            onMove.Invoke(Vector2.up);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            onMove.Invoke(Vector2.right);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            onMove.Invoke(Vector2.down);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            onMove.Invoke(Vector2.left);
        }
    }
}
