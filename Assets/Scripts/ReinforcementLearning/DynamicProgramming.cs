namespace ReinforcementLearning {
    // ReSharper disable once InconsistentNaming
    internal class MDPState {
        public Movement action;
        public float value;

        public MDPState(Movement actionP) {
            action = actionP;
        }
    }
    public class DynamicProgramming {
        public static void PolicyIteration(GridPlayer player, GameGrid grid) {
            // init
            var possibleStates = player.GetAllPossibleStates(grid);
            foreach (var possibleState in possibleStates) {
                
            }
        }
        public static void ValueIteration() {
            // init
            
        }
    }
}