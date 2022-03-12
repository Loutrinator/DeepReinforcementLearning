using System.Collections;
using System.Collections.Generic;
using Common;
using Games;
using ReinforcementLearning;
using ReinforcementLearning.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public StateDelegate stateDelegate;
    
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
    }

    public void SolveGame()
    {
        var moves = DynamicProgramming.ValueIteration(player, gameGrid);
        Debug.Log("moves " + moves.Count);
        StartCoroutine(MovePlayer(moves));
    }

    private IEnumerator MovePlayer(List<Movement> moves) {
        var wait = new WaitForSeconds(0.25f);
        foreach (var move in moves) {
            yield return wait;
            Vector2 dir = GetDirection(move);
            player.Move(dir);
        }
        yield return null;
    }

    private Vector2 GetDirection(Movement move) {
        switch (move) {
            case Movement.Down:
                return new Vector2(0, -1);
            case Movement.Left:
                return new Vector2(-1, 0);
            case Movement.Right:
                return new Vector2(1, 0);
            case Movement.Up:
                return new Vector2(0, 1);
        }

        return Vector2.zero;
    }

    public void OnPlayerSuccess() {
        successCanvas.gameObject.SetActive(true);
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
