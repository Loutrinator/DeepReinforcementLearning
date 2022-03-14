using Games;
using ReinforcementLearning.Common;
using UnityEngine;

namespace ReinforcementLearning {
    public class TemporalDifferenceLearning {
        public static void Sarsa(AiAgent agent, GameGrid grid) {
            var possibleGridStates = agent.GetAllPossibleStates(grid);

            GridState state;
            do {
                state = grid.gridState; // aller à l'état initial
            } while (state.value != 1);
        }

        public static void QLearning() {
            
        }
    }
}