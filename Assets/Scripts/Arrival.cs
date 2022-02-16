using UnityEngine;

public class Arrival : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.OnPlayerSuccess();
        }
    }
}