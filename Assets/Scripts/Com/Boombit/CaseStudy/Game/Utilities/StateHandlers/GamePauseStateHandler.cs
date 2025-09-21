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
            if (_uiView == null)
            {
                GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.GamePauseUIViewPrefab, _sceneReferences.UIViewContainer.transform);
                _uiView = uiViewObject.GetComponent<GamePauseUIView>();
                _uiView.Init(_gameManager);
            }

            _uiView.Show();

            _previousState = fromState;
            
            _gameManager.TimeManager.PauseTimer();
        }
        
        public override void Exit(string toState)
        {
            _uiView.Hide();
            
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