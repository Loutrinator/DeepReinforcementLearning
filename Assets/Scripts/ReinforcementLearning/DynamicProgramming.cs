using System.Collections.Generic;
using UnityEngine;

namespace ReinforcementLearning {
    // ReSharper disable once InconsistentNaming
    internal class MDPState {
        public Movement action;
        public float value;

        public MDPState(Movement actionP) {
            action = actionP;
        }
    }
    public static class DynamicProgramming {
        public static void PolicyIteration(GridPlayer player, GameGrid grid) {
            // init
            var possibleGridStates = player.GetAllPossibleStates(grid);
            foreach (var possibleState in possibleGridStates) {
                GridState state = new GridState(possibleState) {
                    value = 0
                };
            }
        }
        public static void ValueIteration(GridPlayer player, GameGrid grid) {
            // init
            var possibleGridStates = player.GetAllPossibleStates(grid);
            var possibleStates = new List<GridState>();
            foreach (var possibleState in possibleGridStates) {
                GridState state = new GridState(possibleState) {
                    value = 0
                };
                possibleStates.Add(state);
            }

            float delta;
            float theta = 0.7f;
            do {
                delta = 0;
                foreach (var state in possibleStates) {
                    float temp = state.value;
                    
                    // todo

                    delta = Mathf.Max(delta, Mathf.Abs(temp - state.value));
                }
            } while (delta < theta);
        }
    }
}