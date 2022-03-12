using System;
using System.Collections.Generic;
using Common;
using Games;
using ReinforcementLearning.Common;
using UnityEngine;

namespace ReinforcementLearning {
    public static class DynamicProgramming {
        public static void PolicyIteration(AiAgent player, GameGrid grid) {
            // init
            var possibleGridStates = player.GetAllPossibleStates(grid);
            foreach (var possibleState in possibleGridStates) {
                GridState state = new GridState(possibleState) {
                    value = 0
                };
            }
        }
        public static List<Movement> ValueIteration(AiAgent player, GameGrid grid) {
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
            float theta = 0.01f;

            int maxItterations = 100;
            do {
                maxItterations--;
                delta = 0;
                for (var index = 0; index < possibleStates.Count; ++index) {
                    GridState state = possibleStates[index];
                    float temp = state.value;

                    float max = 0;
                    foreach (Movement actionType in Enum.GetValues(typeof(Movement)))
                    {
                        GameManager gm = GameManager.Instance;
                        StateDelegate stateDelegate = gm.stateDelegate;
                        
                        float reward = stateDelegate.GetReward(state,actionType, possibleStates, out var nextStateTmp);
                        float currentVal = reward + gamma * (nextStateTmp?.value ?? 0);
                        if (max < currentVal) {
                            state.bestAction = actionType;
                            max = currentVal;
                        }
                    }

                    state.value = max;

                    delta = Mathf.Max(delta, Mathf.Abs(temp - state.value));
                }


            } while (delta > theta && maxItterations >= 0);    // until delta < theta
            
            // build end policy
            List<Movement> policy = new List<Movement>();
            var nextState = grid.gridState;
            
            maxItterations = 100;
            do
            {
                maxItterations--;
                var tmp = nextState;
                nextState = possibleStates.Find(state => state.Equals(nextState));
                policy.Add(nextState.bestAction);
                nextState = GameManager.Instance.stateDelegate.GetNextState(nextState, nextState.bestAction, possibleStates, out _, out _);
                if (tmp.Equals(nextState)) break;
            } while (nextState != null && maxItterations >= 0);

            string possibleStatesVals = "Values : ";
            foreach (var state in possibleStates)
            {
                possibleStatesVals += state.value + " ";
            }
            Debug.Log(possibleStatesVals);
            string policyLog = "Policy (" + policy.Count + ") :";
            foreach (var move in policy)
            {
                string dirname = "";
                switch (move)
                {
                    case Movement.Right:
                        dirname = ">";
                        break;
                    case Movement.Down:
                        dirname = "v";
                        break;
                    case Movement.Left:
                        dirname = "<";
                        break;
                    case Movement.Up:
                        dirname = "^";
                        break;
                }
                policyLog += " " + dirname;
            }
            Debug.Log(policyLog);
            return policy;
        }
    }
}