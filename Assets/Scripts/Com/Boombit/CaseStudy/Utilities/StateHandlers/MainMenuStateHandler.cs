
using UnityEngine;

namespace Com.Boombit.CaseStudy.Utilities
{
    public class MainMenuStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager _gameManager;
    
        //  METHODS
        public MainMenuStateHandler(IGameManager gameManager) : base("MainMenuState")
        {
            _gameManager = gameManager;
        }
        
        public override void Enter(string fromState)
        {
            _gameManager.ShowMainMenuUI();
            _gameManager.HideGameUI();
            
            _gameManager.TimeManager.UnfreezeGame();
        }
        
        public override void Exit(string toState)
        {
            Debug.Log($"Exiting Main Menu to {toState}");

            _gameManager.HideMainMenuUI();
        }
    }
}