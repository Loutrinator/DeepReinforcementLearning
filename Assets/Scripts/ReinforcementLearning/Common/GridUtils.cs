namespace ReinforcementLearning.Common {
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

        public static int[] Find(this int[][] grid, int value) {
            for (int i = grid.Length - 1; i >= 0; --i) {
                for (int j = grid[0].Length - 1; j >= 0; --j) {
                    if (grid[i][j] == value) return new []{ i, j };
                }
            }
            return null;
        }
    }
}