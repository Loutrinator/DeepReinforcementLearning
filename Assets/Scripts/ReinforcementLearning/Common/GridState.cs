using System.Collections.Generic;
using Common;
using UnityEngine;

namespace ReinforcementLearning.Common {
    public class GridState {
        public int[][] grid;
        public float value;
        public Movement bestAction = Movement.Down;

        public GridState(int[][] gridP) {
            grid = gridP;
        }

        public override string ToString() {
            string log = "";

            for (int i = 0; i < grid.Length; ++i) {
                for (int j = 0; j < grid[i].Length; ++j) {
                    log = log + grid[i][j] + " ";
                }

                log += "\n";
            }
            
            return log;
        }

        public override bool Equals(object other) {
            return Equals((GridState)other);
        }

        protected bool Equals(GridState other) {
            if (other == null) return false;
            Vector2Int gridSize = new Vector2Int(grid[0].Length, grid.Length);
            for (int i = 0; i < gridSize.y; ++i) {
                for (int j = 0; j < gridSize.x; ++j) {
                    if (grid[i][j] != other.grid[i][j]) return false;
                }
            }
            return true;
        }
    }
}