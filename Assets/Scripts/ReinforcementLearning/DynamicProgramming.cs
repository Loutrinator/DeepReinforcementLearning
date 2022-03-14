using System;
using System.Collections.Generic;
using Common;
using Games;
using ReinforcementLearning.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReinforcementLearning {
    public static class DynamicProgramming {
        /*public static List<Movement> PolicyIteration(AiAgent agent, GameGrid grid) {
            // init
            var possibleGridStates = agent.GetAllPossibleStates(grid);
            //Generation des etats de départ avec direction random
            var possibleStates = new List<GridState>();
            foreach (var possibleState in possibleGridStates) {
                int random = Random.Range(0, 4);
                GridState state = new GridState(possibleState) {
                    BestAction = (Movement)0,
                    value = 0
                };
                state.SetArrow();
                possibleStates.Add(state);
            }

            float gamma = 0.9f;
            float theta = 0.01f;

            PolicyEvaluation(possibleStates, gamma, theta);

            GridState nextState = grid.gridState;
            List<Movement> policy = new List<Movement>();
            int maxIterations = 100;
            do {
                maxIterations--;
                var tmp = nextState;
                nextState = possibleStates.Find(state => state.Equals(nextState));
                policy.Add(nextState.BestAction);
                nextState = GameManager.Instance.stateDelegate.GetNextState(nextState, nextState.BestAction,
                    possibleStates, out _, out _);
                if (tmp.Equals(nextState)) break;
            } while (nextState != null && maxIterations >= 0);
            
            string possibleStatesVals = "Values : ";
            foreach (var state in possibleStates) {
                possibleStatesVals += state + " -> " + state.value + " ; best action" + state.BestAction + "\n";
            }

            Debug.Log(possibleStatesVals);
            string policyLog = "Policy (" + policy.Count + ") :";
            foreach (var move in policy) {
                string dirname = "";
                switch (move) {
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

        private static void PolicyEvaluation(List<GridState> possibleStates, float gamma, float theta) {
            int maxIterations = 100000;


            Debug.Log("AFTER INITIALIZATION : ");
            string possibleStatesVals = "Values : ";
            foreach (var state in possibleStates) {
                possibleStatesVals += state + " -> " + state.value + " ; best action" + state.BestAction + "\n";
            }
            Debug.Log(possibleStatesVals);
            
            float delta;
            do {
                delta = 0;
                maxIterations--;
                foreach (GridState gridState in possibleStates) {
                    float tmp = gridState.value; //on récupère la valeur actuelle de l'état
                    
                    //TODO: Maybe nextState est pas null chez moi mais null chez Clément

                    float reward = GameManager.Instance.stateDelegate.GetReward(gridState, gridState.BestAction,
                        possibleStates, out var nextState);
                    float currentValue = reward + gamma * nextState.value;

                    gridState.value = currentValue;
                    delta = Mathf.Max(delta, Mathf.Abs(tmp - currentValue));
                }
            } while (delta > theta && maxIterations >= 0);
            
            Debug.Log("AFTER EVALUATION : ");
            possibleStatesVals = "Values : ";
            foreach (var state in possibleStates) {
                possibleStatesVals += state + " -> " + state.value + " ; best action" + state.BestAction + "\n";
            }
            Debug.Log(possibleStatesVals);

            bool stable = true;
            foreach (GridState state in possibleStates) {
                if (state != null) {
                    Movement currentMovement = state.BestAction;
                    float actionValue = 0;
                    for (int i = 0; i < 4; i++) {
                        Movement move = (Movement)i;

                        float reward = GameManager.Instance.stateDelegate.GetReward(state, state.BestAction,
                            possibleStates, out var nextState);

                        if (nextState == null || nextState.Equals(state)) continue;

                        float tmpStateValue = reward + gamma * nextState.value;
                        if (actionValue < tmpStateValue) {
                            actionValue = tmpStateValue;
                            state.BestAction = move;
                        }
                    }

                    if (state.BestAction != currentMovement) {
                        stable = false;
                    }
                    if(stable) return;
                }
            }
            
            Debug.Log("AFTER IMPROVEMENT : ");
            possibleStatesVals = "Values : ";
            foreach (var state in possibleStates) {
                possibleStatesVals += state + " -> " + state.value + " ; best action" + state.BestAction + "\n";
            }
            Debug.Log(possibleStatesVals);
            
            PolicyEvaluation(possibleStates, gamma, theta);
        }*/

        public static List<Movement> ValueIteration(AiAgent agent, int[][] grid) {
            // init
            var firstState = new GridState(grid);
            var possibleGridStates = agent.GetAllPossibleStates(grid);
            var possibleStates = new List<GridState>();
            foreach (var possibleState in possibleGridStates) {
                GridState state = new GridState(possibleState) {
                    value = 0
                };
                state.SetArrow();
                possibleStates.Add(state);
            }

            float gamma = 0.9f; // facteur de dévaluation
            float delta;
            float theta = 0.01f;

            int maxIterations = 100;
            do {
                maxIterations--;
                delta = 0;
                for (var index = 0; index < possibleStates.Count; ++index) {
                    GridState state = possibleStates[index];
                    float temp = state.value;

                    float max = 0;
                    foreach (Movement actionType in Enum.GetValues(typeof(Movement))) {
                        GameManager gm = GameManager.Instance;
                        IStateDelegate stateDelegate = gm.stateDelegate;

                        float reward = stateDelegate.GetReward(state, actionType, possibleStates, out var nextStateTmp);
                        float currentVal = reward + gamma * nextStateTmp.value;
                        if (max < currentVal) {
                            state.BestAction = actionType;

                            max = currentVal;
                        }
                    }

                    state.value = max;

                    delta = Mathf.Max(delta, Mathf.Abs(temp - state.value));
                }
            } while (delta > theta && maxIterations >= 0); // until delta < theta

            // build end policy
            List<Movement> policy = new List<Movement>();
            var nextState = firstState;

            maxIterations = 100;
            do {
                maxIterations--;
                var tmp = nextState;
                nextState = possibleStates.Find(state => state.Equals(nextState));
                policy.Add(nextState.BestAction);
                nextState = GameManager.Instance.stateDelegate.GetNextState(nextState, nextState.BestAction,
                    possibleStates, out _, out _);
                if (tmp.Equals(nextState)) break;
            } while (nextState != null && maxIterations >= 0);
            
            return policy;
        }
    }
}