using System.Collections.Generic;
using Games;
using ReinforcementLearning.Common;

namespace ReinforcementLearning {
    public class TemporalDifferenceLearning {
        public static void Sarsa(AiAgent agent, GameGrid grid) {
            var possibleGridStates = agent.GetAllPossibleStates(grid);
            var possibleStates = new List<GridState>();
            foreach (var possibleState in possibleGridStates) {
                GridState s = new GridState(possibleState) {
                    value = 0
                };
                s.SetArrow();
                possibleStates.Add(s);
            }

            GridState state = grid.gridState;
            do {
                state = grid.gridState; // aller à l'état initial
            } while (!agent.IsFinalState(state.grid, grid.gridState.grid));
        }

        public static void QLearning() {
            
        }
    }
}