namespace ReinforcementLearning {
    [System.Serializable]
    public class GridState {
        public int[][] grid;
        public float value;

        public GridState(int[][] gridP) {
            grid = gridP;
        }
    }
}