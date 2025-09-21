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
            GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.GameUIViewPrefab, _sceneReferences.UIViewContainer.transform);
            _uiView = uiViewObject.GetComponent<GameUIView>();

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