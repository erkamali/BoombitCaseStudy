using Com.Boombit.CaseStudy.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Utilities
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        //  MEMBERS
        //      Properties
        public TimeManager  TimeManager { get { return _timeManager; } }
        public IGameData    GameData    { get { return _gameData; } }
        //      Editor
        [SerializeField] private TimeManager _timeManager;
        
        [Header("UI References")]
        [SerializeField] private GameObject _mainMenuUIView;
        [SerializeField] private GameObject _gameUIView;
        [SerializeField] private GameObject _levelSuccessUIView;
        [SerializeField] private GameObject _levelFailUIView;
        [SerializeField] private GameObject _gamePauseUIView;
        
        // Private
        private StateManager                _stateManager;
        private MainMenuStateHandler        _mainMenuStateHandler;
        private GameStateHandler            _gameStateHandler;
        private LevelSuccessStateHandler    _levelSuccessStateHandler;
        private LevelFailStateHandler       _levelFailStateHandler;
        private GamePauseStateHandler       _gamePauseStateHandler;

        private GameData _gameData;
        
        //  METHODS
        private void Awake()
        {
            _gameData = new GameData();

            Init();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void Start()
        {
            _stateManager.ChangeState("MainMenu");
        }
        
        private void Init()
        {
            _stateManager = new StateManager();
            
            _mainMenuStateHandler       = new MainMenuStateHandler(this);
            _gameStateHandler           = new GameStateHandler(this);
            _levelSuccessStateHandler   = new LevelSuccessStateHandler(this);
            _levelFailStateHandler      = new LevelFailStateHandler(this);
            _gamePauseStateHandler      = new GamePauseStateHandler(this);
            
            _stateManager.AddHandler(_mainMenuStateHandler);
            _stateManager.AddHandler(_gameStateHandler);
            _stateManager.AddHandler(_levelSuccessStateHandler);
            _stateManager.AddHandler(_levelFailStateHandler);
            _stateManager.AddHandler(_gamePauseStateHandler);
        }
        
        private void SubscribeToEvents()
        {
            TimeManager.OnTimerFinished += OnLevelComplete;
        }

        private void UnsubscribeFromEvents()
        {
            TimeManager.OnTimerFinished -= OnLevelComplete;
        }
        
#region Event Handlers
        
        private void OnLevelComplete()
        {
            _stateManager.ChangeState("LevelSuccessState");
        }
        
        public void OnPlayerDied()
        {
            _stateManager.ChangeState("LevelFailState");
        }
        
        public void OnEnemyKilled()
        {
            _gameData.IncrementKillCount();
        }
        
#endregion
        
#region View callbacks
        
        public void OnStartGameButtonClicked()
        {
            _stateManager.ChangeState("GameState");
        }
        
        public void OnGamePaused()
        {
            if (_stateManager.CurrentState == "GameState")
            {
                _stateManager.ChangeState("GamePauseState");
            }
        }
        
        public void OnGameResumed()
        {
            if (_stateManager.CurrentState == "GamePauseState")
            {
                _stateManager.ChangeState("GameState");
            }
        }
        
        public void OnLevelRestarted()
        {
            _stateManager.ChangeState("GameState");
        }
        
        public void OnNextLevelButtonClicked()
        {
            _gameData.CompleteLevel();
            _stateManager.ChangeState("GameState");
        }
        
        public void OnQuitToMainMenuButtonClicked()
        {
            _stateManager.ChangeState("MainMenuState");
        }
        
#endregion
        
#region IGameManager implementations
        
        public void ShowMainMenuUI()
        {
            SetUIActive(_mainMenuUIView, true);
        }
        
        public void HideMainMenuUI()
        {
            SetUIActive(_mainMenuUIView, false);
        }
        
        public void ShowGameUI()
        {
            SetUIActive(_gameUIView, true);
        }
        
        public void HideGameUI()
        {
            SetUIActive(_gameUIView, false);
        }
        
        public void ShowLevelSuccessUI(int levelKills, int totalKills)
        {
            //TODO: Update level success UI text with levelKills and totalKills
            //gameWonUI.GetComponent<GameWonUIController>().SetKillCounts(levelKills, totalKills);
            SetUIActive(_levelSuccessUIView, true);
        }
        
        public void HideLevelSuccessUI()
        {
            SetUIActive(_levelSuccessUIView, false);
        }
        
        public void ShowLevelFailUI()
        {
            SetUIActive(_levelFailUIView, true);
        }
        
        public void HideLevelFailUI()
        {
            SetUIActive(_levelFailUIView, false);
        }
        
        public void ShowGamePauseUI()
        {
            SetUIActive(_gamePauseUIView, true);
        }
        
        public void HideGamePauseUI()
        {
            SetUIActive(_gamePauseUIView, false);
        }
        
        private void SetUIActive(GameObject ui, bool active)
        {
            if (ui != null)
            {
                ui.SetActive(active);
            }
        }
        
        public void EnableControls()
        {
            // Enable your player movement system
            // Example: FindObjectOfType<PlayerController>().enabled = true;
        }
        
        public void DisableControls()
        {
            // Disable your player movement system
            // Example: FindObjectOfType<PlayerController>().enabled = false;
        }
        
        public void StartEnemySpawning()
        {
            // Start your enemy spawning system
            // Example: FindObjectOfType<EnemySpawner>().StartSpawning();
        }
        
        public void StopEnemySpawning()
        {
            // Stop your enemy spawning system
            // Example: FindObjectOfType<EnemySpawner>().StopSpawning();
        }
        
        public void StopAllEnemies()
        {
            // Stop or destroy all active enemies
            // Example: 
            // Enemy[] enemies = FindObjectsOfType<Enemy>();
            // foreach (Enemy enemy in enemies) enemy.Stop();
        }
        
#endregion

        public void ResetAllGameData()
        {
            _gameData.ResetAllData();
        }
        
        public void SaveGameData()
        {
            _gameData.SaveData();
        }

#region Unity event callbacks
    
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _gameData.SaveData();
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                _gameData.SaveData();
            }
        }
    
#endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_stateManager.CurrentState == "GameState")
                {
                    OnGamePaused();
                }
                else if (_stateManager.CurrentState == "GamePauseState")
                {
                    OnGameResumed();
                }
            }
        }
        
    }
}