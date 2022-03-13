using UnityEngine;

namespace ReinforcementLearning.Common {
    public class ArrowsFromGrid : MonoBehaviour {
        public GameObject arrowPrefab;

        private Transform[][] arrowGrid;
        public void Init(int[][] grid, Vector3 firstPos) {
            arrowGrid = new Transform[grid.Length][];
            for (int i = 0; i < grid.Length; ++i) {
                arrowGrid[i] = new Transform[grid[i].Length];
                for (int j = 0; j < grid[i].Length; ++j) {
                    Vector3 arrowPos = firstPos + j * Vector3.right + i * Vector3.up + new Vector3(0.5f, 0.5f, 0);
                    var go = Instantiate(arrowPrefab, transform);
                    go.transform.position = arrowPos;
                    arrowGrid[i][j] = go.transform;
                }
            }
        }

        public Transform GetArrow(int i, int j) {
            Debug.Log("i : " + i + " ; j : " + j + " ; size : " + arrowGrid.Length);
            return arrowGrid[i][j];
        }
    }
}
