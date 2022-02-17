using System;
using UnityEngine;

namespace ReinforcementLearning {
    [Serializable]
    public class GameGrid {
        public GridState gridState;

        public void Init(Transform groundPlane) {
            var groundScale = groundPlane.lossyScale;
            var groundSize = new Vector2Int(Mathf.RoundToInt(groundScale.x * 10), Mathf.RoundToInt(groundScale.z * 10));
            var groundPos = groundPlane.position;
            var firstPos = new Vector3(groundPos.x - groundSize.x / 2f, groundPos.y - groundSize.y / 2f, 0);

            var grid = new int[groundSize.y][];
            for (int i = 0; i < grid.Length; ++i) {
                grid[i] = new int[groundSize.x];

                for (int j = 0; j < grid[i].Length; ++j) {
                    Vector3 castPos = firstPos + j * Vector3.right + i * Vector3.up + new Vector3(0.5f, 0.5f, 0);
                    if (Physics.Raycast(castPos + Vector3.back * 5, Vector3.forward, out var hit, 10)) {
                        var objLayer = hit.collider.gameObject.layer;
                        grid[i][j] = objLayer;
                    }
                    else {
                        grid[i][j] = -1;
                    }
                }
            }
            
            gridState = new GridState(grid);

            Debug.Log(gridState.ToString());
        }
    }

    public static class GridUtils {
        public static int[][] CloneGrid(this int[][] source) {
            var result = new int[source.Length][];

            for (int i = 0; i < source.Length; ++i) {
                result[i] = new int[source[i].Length];
                for (int j = 0; j < source[i].Length; ++j) {
                    result[i][j] = source[i][j];
                }
            }
        
            return result;
        }
    }
}