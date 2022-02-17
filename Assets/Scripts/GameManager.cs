using ReinforcementLearning;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    [SerializeField] private GridPlayer player;
    [SerializeField] private Transform groundPlane;

    [SerializeField] private Canvas successCanvas;

    private GameGrid gameGrid;
    
    private void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start() {
        successCanvas.gameObject.SetActive(false);

        gameGrid = new GameGrid();
        gameGrid.Init(groundPlane);

        DynamicProgramming.PolicyIteration(player, gameGrid);
    }

    public void OnPlayerSuccess() {
        successCanvas.gameObject.SetActive(true);
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
