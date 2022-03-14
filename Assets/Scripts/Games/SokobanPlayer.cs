using System.Collections.Generic;
using Common;
using ReinforcementLearning.Common;
using UnityEngine;

namespace Games
{
    public class SokobanPlayer : AiAgent, IStateDelegate
    {
        [SerializeField] private LayerMask crateMask;
        private int layerPlayer;
        private int layerCrate;
        private int layerGround;
        private int layerArrival;

        private List<int[]> arrivalPositions = new List<int[]>();

        private int width;
        private int height;

        private void Awake()
        {
            layerPlayer = Layers.IntValue("Player");
            layerCrate = Layers.IntValue("Crate");
            layerGround = Layers.IntValue("Ground");
            layerArrival = Layers.IntValue("Arrival");
        }

        private void Start()
        {
            GameManager.Instance.stateDelegate = this;
        }

        public override void Move(Vector2 direction)
        {
            Vector3 dir = (direction.x * transform.right + direction.y * transform.up) * gridSize;
            Vector3 destination = transform.position + dir;
            
            
            if (Physics.Raycast(destination - transform.forward * 5, transform.forward, out var hit, 10)) {
                if (walkableMask == (walkableMask | (1 << hit.collider.gameObject.layer)))
                {
                    transform.position = destination;
                }
                if(crateMask == (crateMask | (1 << hit.collider.gameObject.layer)))
                {
                    Vector3 crateDestination = hit.transform.position + dir;
                    if (Physics.Raycast(crateDestination - transform.forward * 5, transform.forward, out var hit2, 10))
                    {

                        if (walkableMask == (walkableMask | (1 << hit2.collider.gameObject.layer)))
                        {
                            hit.transform.position += dir;
                            transform.position = destination;
                        }
                    }
                }
            }
        }

        public override int[][][] GetAllPossibleStates(GameGrid grid)
        {
            var cleanedGrid = grid.gridState.grid.CloneGrid();
            
            height = cleanedGrid.Length;
            width = cleanedGrid[0].Length;
            
            /*string originalLog = "Original map:\n";
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    originalLog += cleanedGrid[i][j] + " ";
                }
    
                originalLog += "\n";
            }
            
            Debug.Log(originalLog);*/

            

            int crateCount = 0;
            
            for (int i = 0; i < height; ++i) {
                for (var j = 0; j < width; ++j)
                {
                    if (cleanedGrid[i][j] == layerPlayer) cleanedGrid[i][j] = layerGround;
                    else if (cleanedGrid[i][j] == layerCrate)
                    {
                        cleanedGrid[i][j] = layerGround;
                        crateCount++;
                    }else if (cleanedGrid[i][j]  == layerArrival)
                    {
                        arrivalPositions.Add(new []{i,j});
                    }
                }
            }

            /*Debug.Log("arrivalPositions count : " + arrivalPositions.Count);
            string cleanLog = "Cleaned map:\n";
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    cleanLog += cleanedGrid[i][j] + " ";
                }
    
                cleanLog += "\n";
            }
            Debug.Log(cleanLog);*/
            
            List<int[][]> gridList = new List<int[][]>();
            gridList.Add(cleanedGrid);
            gridList = GenerateAllPossibleStates(gridList, crateCount, width, height);
            
            
            /*string log = "";
            foreach (var res in gridList) {
                for (int i = 0; i < height; ++i)
                {
                    log += "|";
                    for (int j = 0; j < width; ++j) {
                        int gridCase = res[i][j];
                        string caseDesc = "";
                        switch (gridCase)
                        {
                            case 6 :
                                caseDesc = "P";
                                break;
                            case 7 :
                                caseDesc = "X";
                                break;
                            case 8 :
                                caseDesc = "_";
                                break;
                            case 9 :
                                caseDesc = "*";
                                break;
                            case 11 :
                                caseDesc = "#";
                                break;
                        }
                        log += caseDesc + " ";
                    }
    
                    log += "|\n";
                }
    
                log += "\n";
            }
            Debug.Log(log);*/

            return gridList.ToArray();
            
        }

        public override bool IsFinalState(int[][] grid, int[][] firstState) {
            return false;
        }

        private List<int[][]> GenerateAllPossibleStates(List<int[][]> previousGrids, int remainingcrates, int gridWidth, int gridHeight)
        {
            //remaining crates 1
            if (remainingcrates < 0) return previousGrids;

            List<int[][]> newGrid = new List<int[][]>();
            foreach (var grid in previousGrids)
            {
                for (int i = 0; i < gridHeight; ++i) {
                    for (int j = 0; j < gridWidth; ++j)
                    {
                        int layer = grid[i][j];
                        if (layer == layerArrival || layer == layerGround) {
                            grid[i][j] = remainingcrates == 0 ? layerPlayer : layerCrate;
                            newGrid.Add(grid.CloneGrid());  // just a copy
                            grid[i][j] = layer;//TODO: peut etre retirer cette ligne
                        }
                    }
                }    
            }

            return GenerateAllPossibleStates(newGrid, remainingcrates - 1, gridWidth, gridHeight);

        }

        public int GetReward(GridState currentState, Movement action, List<GridState> possibleStates, out GridState nextState)
        {
            nextState = GetNextState(currentState, action, possibleStates, out var playerI, out var playerJ);

            int countProperlyPlacedCrates = 0;
            //TODO: Attention aux height et width peut etre mal set ou inversés
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j)
                {
                    int gridCase = nextState.grid[i][j];
                    if (gridCase == layerCrate) {
                        
                        foreach (var arrival in arrivalPositions)
                        {
                            int arrivalX = arrival[0];
                            int arrivalY = arrival[1];
                            if (arrivalX == i && arrivalY == j)
                            {
                                countProperlyPlacedCrates++;
                            }
                        }
                    }
                }
            }

            return countProperlyPlacedCrates / arrivalPositions.Count;
        }

        public GridState GetNextState(GridState currentState, Movement action, List<GridState> possibleStates, out int playerNewI,
            out int playerNewJ)
        {
            var nextState = new GridState(currentState.grid.CloneGrid());
            playerNewI = -1;
            playerNewJ = -1;
            Vector2Int gridSize = new Vector2Int(currentState.grid[0].Length, currentState.grid.Length);
            for (int i = 0; i < gridSize.y; ++i) {
                for (int j = 0; j < gridSize.x; ++j) {
                    if (currentState.grid[i][j] == layerPlayer) {
                        int playerDestination = 0;
                        
                        (int, int) newPos = MovePosition(i, j, action, width, height);

                        playerNewI = newPos.Item1;
                        playerNewJ = newPos.Item2;

                        playerDestination = nextState.grid[playerNewI][playerNewJ];
                        //SI LE JOUEUR SE DIRIGE VERS UNE CASE VIDE
                        if (playerDestination == layerGround || playerDestination == layerArrival) {
                            bool wasAnArrival = false;
                            foreach (var arrival in arrivalPositions)
                            {
                                if (arrival[0] == i && arrival[1] == j)
                                {
                                    wasAnArrival = true;
                                    break;
                                }
                            }
                            nextState.grid[i][j] = wasAnArrival ? layerArrival : layerGround;
                            nextState.grid[playerNewI][playerNewJ] = layerPlayer;
                            return possibleStates.Find(state => state.Equals(nextState));
                        }
                        //LE JOUEUR SE DIRIGE VERS UNE CAISSE
                        if (playerDestination == layerCrate) {
                            (int, int) crateNewPos = MovePosition(playerNewI, playerNewJ, action, width, height);
                            int crateNewPosI = crateNewPos.Item1;
                            int crateNewPosJ = crateNewPos.Item2;
                            int crateDestination = nextState.grid[crateNewPosI][crateNewPosJ];
                            //si la crate est poussée vers une case vide
                            if (crateDestination == layerGround || crateDestination == layerArrival)
                            {
                                bool wasAnArrival = false;
                                foreach (var arrival in arrivalPositions)
                                {
                                    if (arrival[0] == i && arrival[1] == j)
                                    {
                                        wasAnArrival = true;
                                        break;
                                    }
                                }
                                nextState.grid[i][j] = wasAnArrival ? layerArrival : layerGround;
                                nextState.grid[playerNewI][playerNewJ] = layerPlayer;
                                //TODO: Peut etre inverser Item2 et Item1
                                nextState.grid[crateNewPos.Item1][crateNewPos.Item2] = layerCrate;
                                return possibleStates.Find(state => state.Equals(nextState));
                            }
                        }
                    }
                }
            }
            
            return currentState;
        }
        
    }
}