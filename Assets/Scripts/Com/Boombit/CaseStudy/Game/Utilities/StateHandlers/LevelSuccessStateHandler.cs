using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class LevelSuccessStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager        _gameManager;
        private SceneReferences     _sceneReferences;
        private ResourceReferences  _resourceReferences;

        private LevelSuccessUIView  _uiView;
        
        //  METHODS
        public LevelSuccessStateHandler(IGameManager gameManager, SceneReferences sceneReferences, ResourceReferences resourceReferences) : base("LevelSuccessState")
        {
            _gameManager        = gameManager;
            _sceneReferences    = sceneReferences;
            _resourceReferences = resourceReferences;
        }
        
        public override void Enter(string fromState)
        {
            if (_uiView == null)
            {
                GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.LevelSuccessUIViewPrefab, _sceneReferences.UIViewContainer.transform);
                _uiView = uiViewObject.GetComponent<LevelSuccessUIView>();
                _uiView.Init(_gameManager);
            }

            _uiView.Show();

            _gameManager.GameData.CompleteLevel();
            
            _gameManager.StopAllEnemies();
        }
        
        public override void Exit(string toState)
        {
            _uiView.Hide();
        }
    }
}