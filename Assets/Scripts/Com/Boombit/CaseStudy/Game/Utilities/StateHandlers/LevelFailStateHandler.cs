using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class LevelFailStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager _gameManager;
        
        //  METHODS
        public LevelFailStateHandler(IGameManager gameManager) : base("LevelFailState")
        {
            _gameManager = gameManager;
        }
        
        public override void Enter(string fromState)
        {
            _gameManager.HideGameUI();

            _gameManager.ShowLevelFailUI();
            
            _gameManager.TimeManager.StopTimer();
            _gameManager.StopAllEnemies();
        }
        
        public override void Exit(string toState)
        {
            _gameManager.HideLevelFailUI();
        }
    }
}