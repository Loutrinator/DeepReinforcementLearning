using System.Collections.Generic;
using System.Linq;
using Games;
using ReinforcementLearning.Common;
using UnityEngine;

namespace ReinforcementLearning {
    public class Episode {
        public List<GridState> gridStates;
        public List<Movement> actions;
        public List<float> rewards;

        public Episode(AiAgent agent, GridState firstState, List<GridState> possibleStates, int maxEpisodeLength) {
            gridStates = new List<GridState>();
            actions = new List<Movement>();
            rewards = new List<float>();

            int iterations = 0;
            var currentGameState = possibleStates[0];

            while (!agent.IsFinalState(currentGameState.grid, firstState.grid) && iterations < maxEpisodeLength) {
                int index = possibleStates.IndexOf(currentGameState);
                var action = possibleStates[index].BestAction;
                //var nextGameState =

                gridStates.Add(currentGameState);
                actions.Add(action);
                rewards.Add(GameManager.Instance.stateDelegate.GetReward(currentGameState, action, possibleStates,
                    out var nextState));

                currentGameState = nextState;
                ++iterations;
            }
        }
    }
    
    public class MonteCarlo {
        public static Dictionary<GridState, float> FirstVisitMonteCarloPrediction(AiAgent agent, GameGrid grid, int episodesCount, int maxEpisodeLength) {
            var possibleGridStates = agent.GetAllPossibleStates(grid);
            var possibleStates = new List<GridState>();
            foreach (var possibleState in possibleGridStates) {
                GridState state = new GridState(possibleState) {
                    value = 0
                };
                state.SetArrow();
                possibleStates.Add(state);
            }

            var v = new Dictionary<GridState, float>();
            var n = new Dictionary<GridState, float>();
            var returns = new Dictionary<GridState, float>();
            for (int i = possibleStates.Count - 1; i >= 0; --i) {
                n.Add(possibleStates[i], 0);
                returns.Add(possibleStates[i], 0);
            }
            
            float g;
            for (int e = 0; e < episodesCount; ++e) {
                Episode episode = new Episode(agent, grid.gridState, possibleStates, maxEpisodeLength);
                g = 0;
                for (int t = episode.gridStates.Count - 2; t >= 0; --t) {
                    g += episode.rewards[t + 1];
                    if (!episode.gridStates.Take(t - 1).Contains(episode.gridStates[t])) {
                        returns[episode.gridStates[t]] += g;
                        n[episode.gridStates[t]] += 1;
                    }
                }
            }

            for (int i = possibleStates.Count - 1; i >= 0; --i) {
                v[possibleStates[i]] = returns[possibleStates[i]] / n[possibleStates[i]];
            }

            return v;
        }
    }
}