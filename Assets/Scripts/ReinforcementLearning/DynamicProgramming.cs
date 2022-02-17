using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReinforcementLearning {
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

            float gamma = 0.9f; // facteur de dévaluation
            float delta;
            float theta = 0.7f;
            int iterations = 0;
            do {
                ++iterations;
                delta = 0;
                for (var index = 0; index < possibleStates.Count; index++) {
                    var state = possibleStates[index];
                    float temp = state.value;

                    float max = 0;
                    foreach (Movement actionType in Enum.GetValues(typeof(Movement))) {
                        float reward = state.GetReward(actionType, possibleStates, out var nextStateTmp);
                        float currentVal = reward + gamma * nextStateTmp.value;
                        if (max < currentVal) {
                            state.bestAction = actionType;
                            max = currentVal;
                        }
                    }

                    state.value = max;

                    delta = Mathf.Max(delta, Mathf.Abs(temp - state.value));
                }
            } while (delta > theta);
            Debug.Log(iterations + " iterations");
            
            // build end policy
            List<Movement> policy = new List<Movement>();
            var currentState = grid.gridState;
            policy.Add(grid.gridState.bestAction);
            var nextState = currentState.GetNextState(currentState.bestAction, possibleStates, out _, out _);
            policy.Add(nextState.bestAction);

            for (int i = 0; i < policy.Count; ++i) {
                Debug.Log(i + " => " + policy[i]);
            }
        }
    }
}