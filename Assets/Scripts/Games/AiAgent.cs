using ReinforcementLearning.Common;
using UnityEngine;

namespace Games
{
    
    public abstract class AiAgent : MonoBehaviour {
        public float gridSize = 1;
        [SerializeField] protected LayerMask walkableMask;

        protected int stateCount = 0;

        public abstract void Move(Vector2 direction);

        public abstract int[][][] GetAllPossibleStates(GameGrid grid);

        protected (int, int) MovePosition(int i, int j, Movement action, int gridWidth, int gridHeight)
        {
            int newI = i;
            int newJ = j;
            switch (action) {
                case Movement.Up:
                    newI = i + 1 < gridHeight ? i + 1 : i;
                    break;
                case Movement.Right:
                    newJ = j + 1 < gridWidth ? j + 1 : j;
                    break;
                case Movement.Down:
                    newI = i - 1 >= 0 ? i - 1 : i;
                    break;
                case Movement.Left:
                    newJ = j - 1 >= 0 ? j - 1 : j;
                    break;
            }

            return (newI, newJ);
        }

        public abstract bool IsFinalState(int[][] grid, int[][] firstState);

    }
}