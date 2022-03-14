using System.Collections.Generic;

namespace ReinforcementLearning.Common
{
    public interface IStateDelegate
    {
        public int GetReward(GridState currentState, Movement action, List<GridState> possibleStates, out GridState nextState);

        public GridState GetNextState(GridState currentState, Movement action, List<GridState> possibleStates, out int playerNewI,
            out int playerNewJ);
    }
}