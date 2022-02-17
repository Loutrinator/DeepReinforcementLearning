namespace ReinforcementLearning {
    public class GridState {
        public int[][] grid;
        public float value;

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
    }
}