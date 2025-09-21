using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class GamePauseStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager        _gameManager;
        private SceneReferences     _sceneReferences;
        private ResourceReferences  _resourceReferences;
        private string              _previousState;

        private GamePauseUIView     _uiView;
        
        //  METHODS
        public GamePauseStateHandler(IGameManager gameManager, SceneReferences sceneReferences, ResourceReferences resourceReferences) : base("GamePauseState")
        {
            _gameManager        = gameManager;
            _sceneReferences    = sceneReferences;
            _resourceReferences = resourceReferences;
        }
        
        public override void Enter(string fromState)
        {
            GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.GamePauseUIViewPrefab, _sceneReferences.UIViewContainer.transform);
            _uiView = uiViewObject.GetComponent<GamePauseUIView>();

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