using UnityEngine;

namespace Com.Boombit.CaseStudy.Utilities
{
    public class LevelSuccessStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager _gameManager;
        
        //  METHODS
        public LevelSuccessStateHandler(IGameManager gameManager) : base("LevelSuccessState")
        {
            _gameManager = gameManager;
        }
        
        public override void Enter(string fromState)
        {            
            _gameManager.GameData.CompleteLevel();
            
            _gameManager.StopAllEnemies();

            _gameManager.HideGameUI();

            _gameManager.ShowLevelSuccessUI(_gameManager.GameData.CurrentLevelKills, _gameManager.GameData.TotalKills);
        }
        
        public override void Exit(string toState)
        {
            _gameManager.HideLevelSuccessUI();
        }
    }
}