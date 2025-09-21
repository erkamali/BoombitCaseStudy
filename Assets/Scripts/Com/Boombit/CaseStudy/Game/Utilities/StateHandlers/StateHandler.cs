namespace Com.Boombit.CaseStudy.Utilities
{
    public abstract class StateHandler
    {
        //  MEMBERS
        public readonly string name;
        

        //  CONSTRUCTORS
        public StateHandler(string name)
        {
            this.name = name;
        }


        //  METHODS
        public abstract void Enter(string fromState);
        public abstract void Exit(string toState);
    }
}