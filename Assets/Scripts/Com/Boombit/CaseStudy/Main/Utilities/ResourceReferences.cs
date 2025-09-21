using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Utilities
{
    public class ResourceReferences : MonoBehaviour
    {
        //  MEMBERS
        //      From Editor
        [SerializeField] private GameObject _mainMenuUIViewPrefab;
        [SerializeField] private GameObject _gameUIViewPrefab;
        [SerializeField] private GameObject _levelSuccessUIViewPrefab;
        [SerializeField] private GameObject _levelFailUIViewPrefab;
        [SerializeField] private GameObject _gamePauseUIViewPrefab;

        //      Properties
        public GameObject   MainMenuUIViewPrefab        { get { return _mainMenuUIViewPrefab; } }
        public GameObject   GameUIViewPrefab            { get { return _gameUIViewPrefab; } }
        public GameObject   LevelSuccessUIViewPrefab    { get { return _levelSuccessUIViewPrefab; } }
        public GameObject   LevelFailUIViewPrefab       { get { return _levelFailUIViewPrefab; } }
        public GameObject   GamePauseUIViewPrefab       { get { return _gamePauseUIViewPrefab; } }
        

    }
}