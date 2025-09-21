using Com.Boombit.CaseStudy.Data;
using Com.Boombit.CaseStudy.Views;
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
        
        [Header("UIViews")]
        [SerializeField] private MainMenuUIView     _mainMenuUIView;
        [SerializeField] private GameUIView         _gameUIView;
        [SerializeField] private LevelSuccessUIView _levelSuccessUIView;
        [SerializeField] private LevelFailUIView    _levelFailUIView;
        [SerializeField] private GamePauseUIView    _gamePauseUIView;
        
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

            InitStateManagers();
            InitUIViews();
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
        
        private void InitStateManagers()
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

        private void InitUIViews()
        {
            _mainMenuUIView.Init(this);
            _gameUIView.Init(this);
            _levelSuccessUIView.Init(this);
            _levelFailUIView.Init(this);
            _gamePauseUIView.Init(this);
            
            HideAllUIViews();
            _mainMenuUIView.Show();
        }
        
        private void SubscribeToEvents()
        {
            TimeManager.OnTimerFinished += OnTimerFinished;
            TimeManager.OnTimeUpdated   += OnTimeUpdated;
        }

        private void UnsubscribeFromEvents()
        {
            TimeManager.OnTimerFinished -= OnTimerFinished;
            TimeManager.OnTimeUpdated   -= OnTimeUpdated;
        }

        private void HideAllUIViews()
        {
            _mainMenuUIView.Hide();
            _gameUIView.Hide();
            _levelSuccessUIView.Hide();
            _levelFailUIView.Hide();
            _gamePauseUIView.Hide();
        }

#region TimeManager callbacks

        public void OnTimerFinished()
        {
            _stateManager.ChangeState("LevelSuccessState");
        }

        public void OnTimeUpdated(float currentTime, float normalizedTime)
        {
            if (_gameUIView != null && _gameUIView.gameObject.activeInHierarchy)
            {
                _gameUIView.OnTimeUpdated(currentTime, normalizedTime);
            }
        }

#endregion

        public void OnPlayerDied()
        {
            _stateManager.ChangeState("LevelFailState");
        }
        
        public void OnEnemyKilled()
        {
            _gameData.IncrementKillCount();
        }
        
#region View callbacks
    
        public void StartGame()
        {
            _stateManager.ChangeState("GameState");
        }
        
        public void PauseGame()
        {
            if (_stateManager.CurrentState == "GameState")
            {
                _stateManager.ChangeState("GamePauseState");
            }
        }
        
        public void ResumeGame()
        {
            if (_stateManager.CurrentState == "GamePauseState")
            {
                _stateManager.ChangeState("GameState");
            }
        }
        
        public void RestartLevel()
        {
            _stateManager.ChangeState("GameState");
        }
        
        public void NextLevel()
        {
            _gameData.CompleteLevel();
            _stateManager.ChangeState("GameState");
        }
        
        public void BackToMainMenu()
        {
            _stateManager.ChangeState("MainMenuState");
        }
        
        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
#endregion
        
#region IGameManager implementations
        
        public void ShowMainMenuUI()
        {
            HideAllUIViews();
            _mainMenuUIView.Show();
        }
        
        public void HideMainMenuUI()
        {
            _mainMenuUIView.Hide();
        }
        
        public void ShowGameUI()
        {
            HideAllUIViews();
            _gameUIView.Show();
        }
        
        public void HideGameUI()
        {
            _gameUIView.Hide();
        }
        
        public void ShowLevelSuccessUI(int levelKills, int totalKills)
        {
            HideAllUIViews();
            _levelSuccessUIView.Show();
            _levelSuccessUIView.UpdateStats(levelKills, totalKills);
        }
        
        public void HideLevelSuccessUI()
        {
            _levelSuccessUIView.Hide();
        }
        
        public void ShowLevelFailUI()
        {
            HideAllUIViews();
            _levelFailUIView.Show();
        }
        
        public void HideLevelFailUI()
        {
            _levelFailUIView.Hide();
        }
        
        public void ShowGamePauseUI()
        {
            _gamePauseUIView.Show();
        }
        
        public void HideGamePauseUI()
        {
            _gamePauseUIView.Hide();
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
                    PauseGame();
                }
                else if (_stateManager.CurrentState == "GamePauseState")
                {
                    ResumeGame();
                }
            }
        }
        
    }
}