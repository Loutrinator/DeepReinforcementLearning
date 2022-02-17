using System.Collections.Generic;
using System.Linq;
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
    public class DynamicProgramming {
        public static void PolicyIteration(int[][] grid) {
            // init
            var possibleStates = GetAllPossibleStates(grid);
            foreach (var possibleState in possibleStates) {
                
            }
        }
        public static void ValueIteration() {
            // init
            
        }

        public static int[][][] GetAllPossibleStates(int[][] grid) {
            var result = new List<int[][]>();

            var cleanedGrid = grid;
            int gridHeight = grid.Length;
            int gridWidth = grid[0].Length;
            foreach (var row in grid) {
                for (var j = 0; j < gridWidth; ++j) {
                    if (row[j] == 2) row[j] = 0;
                }
            }
            
            // select all possible states
            for (int i = 0; i < gridHeight; ++i) {
                for (int j = 0; j < gridWidth; ++j) {
                    if (cleanedGrid[i][j] == 0) {
                        cleanedGrid[i][j] = 2;
                        result.Add(cleanedGrid.CloneGrid());  // just a copy
                        cleanedGrid[i][j] = 0;
                    }
                }
            }

            /*string log = "";
            foreach (var res in result) {
                for (int i = 0; i < gridHeight; ++i) {
                    for (int j = 0; j < gridWidth; ++j) {
                        log += res[i][j] + " ";
                    }

                    log += "\n";
                }

                log += "\n\n";
            }
            Debug.Log(log);*/

            return result.ToArray();
        }
    }
}