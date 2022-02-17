using System;
using System.Collections.Generic;
using ReinforcementLearning;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Sokoban
{
    public class SokobanPlayer : GridPlayer
    {
        [SerializeField] private LayerMask crateMask;
        private int layerPlayer;
        private int layerCrate;
        private int layerGround;
        private int layerArrival;

        private void Start()
        {
            layerPlayer = LayerMask.NameToLayer("Player");
            layerCrate = LayerMask.NameToLayer("Crate");
            layerGround = LayerMask.NameToLayer("Ground");
            layerArrival = LayerMask.NameToLayer("Arrival");
        }
        public void Move(Vector2 direction)
        {
            Vector3 dir = (direction.x * transform.right + direction.y * transform.up) * gridSize;
            Vector3 destination = transform.position + dir;
            if (Physics.Raycast(destination - transform.forward * 5, transform.forward, out var hit, 10)) {
                if (walkableMask == (walkableMask | (1 << hit.collider.gameObject.layer)))
                {
                    Debug.Log("Move player");
                    transform.position = destination;
                }
                if(crateMask == (crateMask | (1 << hit.collider.gameObject.layer)))
                {
                    Vector3 crateDestination = hit.transform.position + dir;
                    if (Physics.Raycast(crateDestination - transform.forward * 5, transform.forward, out var hit2, 10))
                    {

                        if (walkableMask == (walkableMask | (1 << hit2.collider.gameObject.layer)))
                        {
                            Debug.Log("Crate moves");
                            hit.transform.position += dir;
                            transform.position = destination;
                        }
                    }
                }
            }
        }

        public override int[][][] GetAllPossibleStates(GameGrid grid)
        {
            //throw new NotImplementedException();

            var cleanedGrid = grid.gridState.grid.CloneGrid();
            int gridHeight = cleanedGrid.Length;
            int gridWidth = cleanedGrid[0].Length;

            int crateCount = 0;
            
            for (int i = 0; i < gridHeight; ++i) {
                for (var j = 0; j < gridWidth; ++j) {
                    if (cleanedGrid[i][j] == layerPlayer) cleanedGrid[i][j] = layerGround;
                    else if (cleanedGrid[i][j] == layerCrate) crateCount++;
                }
            }

            string cleanLog = "";
            for (int i = 0; i < gridHeight; ++i) {
                for (int j = 0; j < gridWidth; ++j) {
                    cleanLog += cleanedGrid[i][j] + " ";
                }
    
                cleanLog += "\n";
            }
            Debug.Log(cleanLog);
            
            List<int[][]> gridList = new List<int[][]>();
            gridList.Add(cleanedGrid);
            gridList = GenerateAllPossibleStates(gridList, crateCount, gridWidth, gridHeight);
            

            string log = "";
            foreach (var res in gridList) {
                for (int i = 0; i < gridHeight; ++i) {
                    for (int j = 0; j < gridWidth; ++j) {
                        log += res[i][j] + " ";
                    }
    
                    log += "\n";
                }
    
                log += "\n\n";
            }
            Debug.Log(log);

            return gridList.ToArray();
            
        }

        private List<int[][]> GenerateAllPossibleStates(List<int[][]> previousGrids, int remainingcrates, int gridWidth, int gridHeight)
        {
            if (remainingcrates < 0) return previousGrids;

            List<int[][]> newGrid = new List<int[][]>();
            foreach (var grid in previousGrids)
            {
                for (int i = 0; i < gridHeight; ++i) {
                    for (int j = 0; j < gridWidth; ++j) {
                        if (grid[i][j] == layerGround) {
                            grid[i][j] = remainingcrates == 0 ? layerPlayer : layerCrate;
                            newGrid.Add(grid.CloneGrid());  // just a copy
                            grid[i][j] = layerGround;
                        }
                    }
                }    
            }

            return GenerateAllPossibleStates(newGrid, remainingcrates - 1, gridWidth, gridHeight);

        }
    }
}