using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class GameStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager        _gameManager;
        private SceneReferences     _sceneReferences;
        private ResourceReferences  _resourceReferences;

        private GameUIView          _uiView;
        
        //  METHODS
        public GameStateHandler(IGameManager gameManager, SceneReferences sceneReferences, ResourceReferences resourceReferences) : base("GameState")
        {
            _gameManager        = gameManager;
            _sceneReferences    = sceneReferences;
            _resourceReferences = resourceReferences;
        }
        
        public override void Enter(string fromState)
        {
            if (_uiView == null)
            {
                //GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.GameUIViewPrefab, _sceneReferences.UIViewContainer.transform);
                GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.GameUIViewPrefab);
                _uiView = uiViewObject.GetComponent<GameUIView>();
                _uiView.Init(_gameManager);
            }

            _uiView.Show();
            
            _gameManager.EnableControls();
            
            _gameManager.GameData.ResetCurrentLevelKillCount();
            
            Time.timeScale = 1f;
        }
        
        public override void Exit(string toState)
        {
            _uiView.Hide();

            _gameManager.DisableControls();

            _gameManager.TimeManager.StopTimer();
        }

        public void UpdateTimer(float currentTime, float normalizedTime)
        {
            if (_uiView == null)
            {
                Debug.LogError("This shouldn't happen!");
                return;
            }

            _uiView.UpdateTimer(currentTime, normalizedTime);
        }
    }
}