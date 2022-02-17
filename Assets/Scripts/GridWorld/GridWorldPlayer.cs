using System.Collections.Generic;
using ReinforcementLearning;
using UnityEngine;

namespace GridWorld {
    public class GridWorldPlayer : GridPlayer
    {
        public override int[][][] GetAllPossibleStates(GameGrid grid) {
            int layerPlayer = LayerMask.NameToLayer("Player");
            int layerGround = LayerMask.NameToLayer("Ground");
            
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
    }
}
