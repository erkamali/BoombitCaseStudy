
using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class MainMenuStateHandler : StateHandler
    {
        //  MEMBERS
        //      Private
        private IGameManager        _gameManager;
        private SceneReferences     _sceneReferences;
        private ResourceReferences  _resourceReferences;

        private MainMenuUIView     _uiView;
    
        //  METHODS
        public MainMenuStateHandler(IGameManager gameManager, SceneReferences sceneReferences, ResourceReferences resourceReferences) : base("MainMenuState")
        {
            _gameManager        = gameManager;
            _sceneReferences    = sceneReferences;
            _resourceReferences = resourceReferences;
        }
        
        public override void Enter(string fromState)
        {
            if (_uiView == null)
            {
                GameObject uiViewObject = GameObject.Instantiate(_resourceReferences.MainMenuUIViewPrefab, _sceneReferences.UIViewContainer.transform);
                _uiView = uiViewObject.GetComponent<MainMenuUIView>();
                _uiView.Init(_gameManager);
            }

            _uiView.Show();
            
            _gameManager.TimeManager.UnfreezeGame();
        }
        
        public override void Exit(string toState)
        {
            _uiView.Hide();
        }
    }
}