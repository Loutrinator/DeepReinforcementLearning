using System.Collections.Generic;
using Common;
using UnityEngine;

namespace ReinforcementLearning.Common {
    public class GridState {
        public int[][] grid;
        public float value;
        public Movement bestAction = Movement.Down;

        public GridState(int[][] gridP) {
            grid = gridP;
        }

        public override string ToString() {
            string log = "";

            for (int i = 0; i < grid.Length; ++i) {
                for (int j = 0; j < grid[i].Length; ++j) {
                    log = log + grid[i][j] + " ";
                }

                log += "\n";
            }
            
            return log;
        }

        public int GetReward(Movement action, List<GridState> possibleStates, out GridState nextState) {
            var layerArrival = Layers.IntValue("Arrival");
            nextState = GetNextState(action, possibleStates, out var playerI, out var playerJ);
            if (grid[playerI][playerJ] == layerArrival) {
                return 1;
            }

            return 0;
        }

        public GridState GetNextState(Movement action, List<GridState> possibleStates, out int playerNewI, out int playerNewJ) {
            var layerPlayer = Layers.IntValue("Player");
            var layerGround = Layers.IntValue("Ground");
            var layerArrival = Layers.IntValue("Arrival");
            var nextState = new GridState(grid.CloneGrid());
            playerNewI = -1;
            playerNewJ = -1;
            Vector2Int gridSize = new Vector2Int(grid[0].Length, grid.Length);
            for (int i = 0; i < gridSize.y; ++i) {
                for (int j = 0; j < gridSize.x; ++j) {
                    if (grid[i][j] == layerPlayer) {
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
            
            return this;
        }

        public override bool Equals(object other) {
            return Equals((GridState)other);
        }

        protected bool Equals(GridState other) {
            if (other == null) return false;
            Vector2Int gridSize = new Vector2Int(grid[0].Length, grid.Length);
            for (int i = 0; i < gridSize.y; ++i) {
                for (int j = 0; j < gridSize.x; ++j) {
                    if (grid[i][j] != other.grid[i][j]) return false;
                }
            }
            return true;
        }
    }
}