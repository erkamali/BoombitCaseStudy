using System.Collections.Generic;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class StateManager : IStateManager
    {
        //  MEMBERS
        public string CurrentState { get; private set; }
        //      Private
        private Dictionary<string, StateHandler> _handlers;


        //  CONSTRUCTION
        public StateManager()
        {
            CurrentState = "";
            _handlers    = new Dictionary<string, StateHandler>();
        }

        //  METHODS
        public void AddHandler(StateHandler stateHandler)
        {
            _handlers.Add(stateHandler.name, stateHandler);
        }

        public void ChangeState(string state)
        {
            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _handlers[CurrentState].Exit(state);
            }

            string prevState = CurrentState;
            CurrentState = state;

            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _handlers[CurrentState].Enter(prevState);
            }
        }
    }
}