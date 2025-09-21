using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class GamePauseStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager    _gameManager;
        private string          _previousState;
        
        //  METHODS
        public GamePauseStateHandler(IGameManager gameManager) : base("GamePauseState")
        {
            _gameManager = gameManager;
        }
        
        public override void Enter(string fromState)
        {
            _previousState = fromState;
            
            _gameManager.ShowGamePauseUI();
            
            _gameManager.TimeManager.PauseTimer();
        }
        
        public override void Exit(string toState)
        {
            _gameManager.HideGamePauseUI();
            
            if (toState == "GameState")
            {
                _gameManager.TimeManager.ResumeTimer();
            }
            else
            {
                _gameManager.TimeManager.UnfreezeGame();
            }
        }
        
        public string GetPreviousState()
        {
            return _previousState;
        }
    }
}