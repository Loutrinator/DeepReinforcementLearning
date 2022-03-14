using System.Collections.Generic;
using Common;
using UnityEngine;

namespace ReinforcementLearning.Common {
    public class GridObject {
        public int Key;
        public Vector2Int Value;

        public GridObject(int k, Vector2Int v) {
            Key = k;
            Value = v;
        }

        public GridObject Clone() {
            return new GridObject(Key, Value);
        }
    }
    public class GridState {
        //public int[][] grid;
        public List<GridObject> objectsPositions;    // layer -> (i, j)
        public float value;

        private Movement _bestAction = Movement.Down;
        public Movement BestAction {
            get => _bestAction;
            set {
                _bestAction = value;
                if (_gridArrow == null) return;
                switch (value) {
                    case Movement.Down:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, -90);
                        break;
                    case Movement.Left:
                        _gridArrow.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    case Movement.Right:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case Movement.Up:
                        _gridArrow.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                }
            }
        }

        private Transform _gridArrow;

        public GridState(int[][] gridP) {
            objectsPositions = new List<GridObject>();
            int layerGround = Layers.IntValue("Ground");
            for (int i = gridP.Length - 1; i >= 0; --i) {
                for (int j = gridP[i].Length - 1; j >= 0; --j) {
                    if (gridP[i][j] == layerGround)
                        continue;
                    objectsPositions.Add(new GridObject(gridP[i][j], new Vector2Int(i, j)));
                }
            }
        }

        public GridState(GridState source) {
            objectsPositions = new List<GridObject>();
            for (int i = source.objectsPositions.Count - 1; i >= 0; --i) {
                objectsPositions.Add(source.objectsPositions[i].Clone());
            }
        }

        public void SetArrow() {
            /*int i = 0;
            int j = 0;
            for (i = grid.Length - 1; i >= 0; --i) {
                for (j = grid[i].Length - 1; j >= 0; --j) {
                    if (grid[i][j] == LayerMask.NameToLayer("Player")) {
                        
                        _gridArrow = GameManager.Instance.arrowsManager.GetArrow(i, j);
                        break;
                    }
                }
            }*/
        }

        public override string ToString() {
            string log = "";

            /*for (int i = 0; i < grid.Length; ++i) {
                for (int j = 0; j < grid[i].Length; ++j) {
                    log = log + grid[i][j] + " ";
                }

                if(i < grid.Length - 1)
                    log += "\n";
            }*/
            
            return log;
        }

        public override bool Equals(object other) {
            return Equals((GridState)other);
        }

        protected bool Equals(GridState other) {
            if (other == null) return false;
            for (int i = objectsPositions.Count - 1; i >= 0; --i) {
                if (!other.objectsPositions.Exists(v => v.Key == objectsPositions[i].Key && v.Value == objectsPositions[i].Value)) return false;
            }
            return true;
        }
    }
}