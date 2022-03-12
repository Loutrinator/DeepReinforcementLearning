using System.Collections.Generic;
using Common;
using ReinforcementLearning.Common;
using UnityEngine;

namespace GridWorld {
    public class GridWorldAgent : AiAgent
    {
        public override int[][][] GetAllPossibleStates(GameGrid grid) {
            int layerPlayer = Layers.IntValue("Player");
            int layerGround = Layers.IntValue("Ground");
            
            var result = new List<int[][]>();

            var cleanedGrid = grid.gridState.grid.CloneGrid();
            int gridHeight = cleanedGrid.Length;
            int gridWidth = cleanedGrid[0].Length;
            foreach (var row in cleanedGrid) {
                for (var j = 0; j < gridWidth; ++j) {
                    if (row[j] == layerPlayer) row[j] = layerGround;
                }
            }
            
            // select all possible states
            for (int i = 0; i < gridHeight; ++i) {
                for (int j = 0; j < gridWidth; ++j) {
                    if (cleanedGrid[i][j] == layerGround) {
                        cleanedGrid[i][j] = layerPlayer;
                        result.Add(cleanedGrid.CloneGrid());  // just a copy
                        cleanedGrid[i][j] = layerGround;
                    }
                }
            }

            return result.ToArray();
        }

        public override int GetReward(GridState state, Movement action, List<GridState> possibleStates, out GridState nextState) {
            var layerArrival = Layers.IntValue("Arrival");
            nextState = GetNextState(state, action, possibleStates, out var playerI, out var playerJ);
            if (state.grid[playerI][playerJ] == layerArrival) {
                return 1;
            }

            return 0;
        }

        public override GridState GetNextState(GridState state, Movement action, List<GridState> possibleStates, out int playerNewI, out int playerNewJ) {
            var layerPlayer = Layers.IntValue("Player");
            var layerGround = Layers.IntValue("Ground");
            var layerArrival = Layers.IntValue("Arrival");
            var nextState = new GridState(state.grid.CloneGrid());
            playerNewI = -1;
            playerNewJ = -1;
            Vector2Int gridSize = new Vector2Int(state.grid[0].Length, state.grid.Length);
            for (int i = 0; i < gridSize.y; ++i) {
                for (int j = 0; j < gridSize.x; ++j) {
                    if (state.grid[i][j] == layerPlayer) {
                        int testCell = 0;
                        int newI = i, newJ = j;
                        switch (action) {
                            case Movement.Up:
                                newI = i + 1 < gridSize.y ? i + 1 : i;
                                break;
                            case Movement.Right:
                                newJ = j + 1 < gridSize.x ? j + 1 : j;
                                break;
                            case Movement.Down:
                                newI = i - 1 >= 0 ? i - 1 : i;
                                break;
                            case Movement.Left:
                                newJ = j - 1 >= 0 ? j - 1 : j;
                                break;
                        }

                        playerNewI = newI;
                        playerNewJ = newJ;
                        testCell = nextState.grid[newI][newJ];
                        if (testCell == layerGround || testCell == layerArrival) {
                            nextState.grid[i][j] = layerGround;
                            nextState.grid[newI][newJ] = layerPlayer;

                            return possibleStates.Find(state => state.Equals(nextState));
                        }
                    }
                }
            }
            
            return state;
        }
    }
}
