using System.Collections.Generic;
using ReinforcementLearning.Common;

namespace Common
{
    public interface StateDelegate
    {
        public int GetReward(GridState currentState, Movement action, List<GridState> possibleStates, out GridState nextState);

        public GridState GetNextState(GridState currentState, Movement action, List<GridState> possibleStates, out int playerNewI,
            out int playerNewJ);
    }
}