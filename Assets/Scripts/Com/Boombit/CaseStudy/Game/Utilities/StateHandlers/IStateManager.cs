namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public interface IStateManager
    {
        //  MEMBERS
        string CurrentState { get; }

        //  METHODS
        void AddHandler(StateHandler stateHandler);
        void ChangeState(string state);
    }
}