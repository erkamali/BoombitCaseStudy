using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class GameStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager _gameManager;
        
        //  METHODS
        public GameStateHandler(IGameManager gameManager) : base("GameState")
        {
            _gameManager = gameManager;
        }
        
        public override void Enter(string fromState)
        {
            _gameManager.ShowGameUI();
            
            _gameManager.EnableControls();
            
            _gameManager.GameData.ResetCurrentLevelKillCount();
            
            Time.timeScale = 1f;
        }
        
        public override void Exit(string toState)
        {
            _gameManager.DisableControls();

            _gameManager.TimeManager.StopTimer();
        }
    }
}