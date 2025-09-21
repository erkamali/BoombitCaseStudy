using System.Collections.Generic;
using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Game.Views;
using Com.Boombit.CaseStudy.Main.Data;
using Com.Boombit.CaseStudy.Main.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        //  MEMBERS
        //      Editor
        [SerializeField] private TimeManager    _timeManager;
        [SerializeField] private LevelManager   _levelManager;

        [Header("References")]
        [SerializeField] private SceneReferences      _sceneReferences;
        [SerializeField] private ResourceReferences   _resourceReferences;
        
        [Header("UIViews")]
        [SerializeField] private MainMenuUIView     _mainMenuUIView;
        //[SerializeField] private GameUIView         _gameUIView;
        [SerializeField] private LevelSuccessUIView _levelSuccessUIView;
        [SerializeField] private LevelFailUIView    _levelFailUIView;
        [SerializeField] private GamePauseUIView    _gamePauseUIView;
        
        //      Properties
        public TimeManager  TimeManager { get { return _timeManager; } }
        public IGameData    GameData    { get { return _gameData; } }
        
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

            _levelManager.Initialize(this);

            InitStateManagers();
            SubscribeToEvents();

        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void Start()
        {
            _stateManager.ChangeState("MainMenuState");
        }
        
        private void InitStateManagers()
        {
            _stateManager = new StateManager();
            
            _mainMenuStateHandler       = new MainMenuStateHandler(this, _sceneReferences, _resourceReferences);
            _gameStateHandler           = new GameStateHandler(this, _sceneReferences, _resourceReferences);
            _levelSuccessStateHandler   = new LevelSuccessStateHandler(this, _sceneReferences, _resourceReferences);
            _levelFailStateHandler      = new LevelFailStateHandler(this, _sceneReferences, _resourceReferences);
            _gamePauseStateHandler      = new GamePauseStateHandler(this, _sceneReferences, _resourceReferences);
            
            _stateManager.AddHandler(_mainMenuStateHandler);
            _stateManager.AddHandler(_gameStateHandler);
            _stateManager.AddHandler(_levelSuccessStateHandler);
            _stateManager.AddHandler(_levelFailStateHandler);
            _stateManager.AddHandler(_gamePauseStateHandler);
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

#region TimeManager callbacks

        public void OnTimerFinished()
        {
            _stateManager.ChangeState("LevelSuccessState");
        }

        public void OnTimeUpdated(float currentTime, float normalizedTime)
        {
            if (_stateManager.CurrentState == "GameState")
            {
                _gameStateHandler.UpdateTimer(currentTime, normalizedTime);
            }
        }

#endregion
        
#region View callbacks
    
        public void StartGame()
        {
            _gameData.SetCurrentLevel(0);

            _levelManager.UnloadLevel();

            _levelManager.LoadLevel(_gameData.CurrentLevel);

            float levelDuration = _levelManager.GetLevelDuration(_gameData.CurrentLevel);
            _timeManager.Init(levelDuration);
            _timeManager.StartTimer();

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
            _levelManager.UnloadLevel();

            _gameData.ResetCurrentLevelKillCount();

            _levelManager.LoadLevel(_gameData.CurrentLevel);
            
            float levelDuration = _levelManager.GetLevelDuration(_gameData.CurrentLevel);
            _timeManager.Init(levelDuration);
            _timeManager.StartTimer();

            _stateManager.ChangeState("GameState");
        }
        
        public void NextLevel()
        {
            _gameData.CompleteLevel();

            int totalLevels = _levelManager.GetTotalLevelCount();
            int currentLevelIndex = _gameData.CurrentLevel - 1;

            if (_gameData.CurrentLevel < totalLevels)
            {
                _levelManager.UnloadLevel();

                _levelManager.LoadLevel(_gameData.CurrentLevel);

                float levelDuration = _levelManager.GetLevelDuration(_gameData.CurrentLevel);
                _timeManager.Init(levelDuration);
                _timeManager.StartTimer();
                
                _stateManager.ChangeState("GameState");
            }
            else
            {
                Debug.Log("All levels completed!");
                _stateManager.ChangeState("MainMenuState");
            }

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

        // Level related
        public PlayerData CreatePlayerData(PlayerConfig playerConfig)
        {
            PlayerData playerData = _gameData.CreatePlayer(playerConfig);
            return playerData;
        }

        public EnemyData CreateEnemyData(EnemyConfig enemyConfig)
        {
            EnemyData enemyData = _gameData.CreateEnemy(enemyConfig);
            return enemyData;
        }
        
        // UI related
        public void ShowMainMenuUI()
        {
            
        }
        
        public void HideMainMenuUI()
        {
        }
        
        public void ShowGameUI()
        {

        }
        
        public void HideGameUI()
        {

        }
        
        public void ShowLevelSuccessUI(int levelKills, int totalKills)
        {

        }
        
        public void HideLevelSuccessUI()
        {

        }
        
        public void ShowLevelFailUI()
        {

        }
        
        public void HideLevelFailUI()
        {

        }
        
        public void ShowGamePauseUI()
        {

        }
        
        public void HideGamePauseUI()
        {

        }
        
        public void EnableControls()
        {
            PlayerView playerView = FindFirstObjectByType<PlayerView>();
            playerView.SetControlsEnabled(true);
        }
        
        public void DisableControls()
        {
            PlayerView playerView = FindFirstObjectByType<PlayerView>();
            playerView.SetControlsEnabled(false);
        }

        public void StopAllEnemies()
        {
            IEnumerator<EnemyData> enemies = _gameData.GetEnemies();
            while (enemies.MoveNext())
            {
                EnemyData enemy = enemies.Current;
                EnemyView enemyView = _levelManager.GetEnemy(enemy.ID);
                if (enemyView != null)
                {
                    enemyView.Stop();
                }
            }
        }

        public void OnCharacterTakeDamage(int characterId, float damage)
        {
            if (characterId == 0)
            {
                _gameData.Player.TakeDamage(damage);
                
                Debug.Log("Player took " + damage + " damage.");
                
                if (_gameData.Player.IsDead)
                {
                    Debug.Log("Player health reached 0");

                    PlayerView playerView = _levelManager.Player;
                    playerView.Die();
                }
                
                // TODO: Health bars?
            }
            else
            {
                if (_gameData.HasEnemy(characterId))
                {
                    EnemyData enemy = _gameData.GetEnemy(characterId);
                    enemy.TakeDamage(damage);
                    
                    Debug.Log("Enemy with characterId " + characterId +" took " + damage + " damage");
                    
                    if (enemy.IsDead)
                    {
                        Debug.Log("Enemy health reached 0");
                        EnemyView enemyView = _levelManager.GetEnemy(characterId);
                        enemyView.Die();
                    }
                }
                else
                {
                    Debug.LogWarning("Enemy not found");
                }
            }
        }

        public void OnCharacterDied(int characterId)
        {
            if (characterId == 0)
            {
                Debug.Log("Player died");
                
                _stateManager.ChangeState("LevelFailState");
            }
            else
            {
                Debug.Log("Enemy with characterId " + characterId +" died");

                if (_levelManager.HasEnemy(characterId))
                {
                    _levelManager.RemoveEnemy(characterId);

                    _gameData.RemoveEnemy(characterId);
                }
                
                // Increment kill count
                _gameData.IncrementKillCount();
            }
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