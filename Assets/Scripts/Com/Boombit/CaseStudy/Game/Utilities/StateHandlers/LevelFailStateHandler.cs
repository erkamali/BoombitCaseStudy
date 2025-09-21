using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class LevelFailStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager        _gameManager;
        private SceneReferences     _sceneReferences;
        private ResourceReferences  _resourceReferences;

        private LevelFailUIView     _uiView;
        
        //  METHODS
        public LevelFailStateHandler(IGameManager gameManager, SceneReferences sceneReferences, ResourceReferences resourceReferences) : base("LevelFailState")
        {
            _gameManager        = gameManager;
            _sceneReferences    = sceneReferences;
            _resourceReferences = resourceReferences;
        }
        
        public override void Enter(string fromState)
        {
            if (_uiView == null)
            {
                GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.LevelFailUIViewPrefab, _sceneReferences.UIViewContainer.transform);
                _uiView = uiViewObject.GetComponent<LevelFailUIView>();
                _uiView.Init(_gameManager);
            }

            _uiView.Show();
            
            _gameManager.TimeManager.StopTimer();
            _gameManager.StopAllEnemies();
        }
        
        public override void Exit(string toState)
        {
            _uiView.Hide();
        }
    }
}