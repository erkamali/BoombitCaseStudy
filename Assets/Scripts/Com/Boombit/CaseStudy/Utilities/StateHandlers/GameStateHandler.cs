using UnityEngine;

namespace Com.Boombit.CaseStudy.Utilities
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
            
            _gameManager.TimeManager.Init(180f);
            _gameManager.TimeManager.StartTimer();
            
            _gameManager.EnableControls();
            
            _gameManager.StartEnemySpawning();
            
            _gameManager.GameData.ResetCurrentLevelKillCount();
            
            Time.timeScale = 1f;
        }
        
        public override void Exit(string toState)
        {
            _gameManager.DisableControls();
            
            _gameManager.StopEnemySpawning();

            _gameManager.TimeManager.StopTimer();
        }
    }
}