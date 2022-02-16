using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    [SerializeField] private GridPlayer player;

    [SerializeField] private Canvas successCanvas;
    
    private void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void Start() {
        successCanvas.gameObject.SetActive(false);
    }

    public void OnPlayerSuccess() {
        successCanvas.gameObject.SetActive(true);
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
