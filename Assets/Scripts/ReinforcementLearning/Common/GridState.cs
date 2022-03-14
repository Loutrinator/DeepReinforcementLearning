using UnityEngine;

namespace ReinforcementLearning.Common {
    public class GridState {
        public int[][] grid;
        public float value;

        private Movement _bestAction = Movement.Down;
        public Movement BestAction {
            get => _bestAction;
            set {
                _bestAction = value;
                if (_gridArrow == null) return;
                switch (value) {
                    case Movement.Down:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, -90);
                        break;
                    case Movement.Left:
                        _gridArrow.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case Movement.Right:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case Movement.Up:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                }
            }
        }

        private Transform _gridArrow;

        public GridState(int[][] gridP) {
            grid = gridP;
        }

        public void SetArrow() {
            int i = 0;
            int j = 0;
            for (i = grid.Length - 1; i >= 0; --i) {
                for (j = grid[i].Length - 1; j >= 0; --j) {
                    if (grid[i][j] == LayerMask.NameToLayer("Player")) {
                        
                        _gridArrow = GameManager.Instance.arrowsManager.GetArrow(i, j);
                        break;
                    }
                }
            }
        }

        public override string ToString() {
            string log = "";

            for (int i = 0; i < grid.Length; ++i) {
                for (int j = 0; j < grid[i].Length; ++j) {
                    log = log + grid[i][j] + " ";
                }

                if(i < grid.Length - 1)
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