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
    }
}