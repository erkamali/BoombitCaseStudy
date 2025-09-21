using System.Collections;
using System.Collections.Generic;
using Com.Boombit.CaseStudy.Game.Views;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    public class LevelManager : MonoBehaviour
    {
        //  MEMBERS
        //
        //      Editor
        [Header("Level Configuration")]
        [SerializeField] private Levels         _levels;
        [SerializeField] private Transform[]    _enemySpawnPoints;
        [SerializeField] private Transform      _playerTransform;
        
        // Properties
        public LevelData    CurrentLevelData    { get { return _currentLevelData; } }
        public int          CurrentLevelIndex   { get { return _currentLevelIndex; } }
        public bool         IsSpawning          { get { return _isSpawning; } }
        public int          ActiveEnemyCount    { get { return _activeEnemies.Count; } }
        public int          TotalEnemiesSpawned { get { return _totalEnemiesSpawned; } }

        //      Private
        private LevelData           _currentLevelData;
        private int                 _currentLevelIndex = 0;
        private bool                _isSpawning = false;
        private List<GameObject>    _activeEnemies = new List<GameObject>();
        private int                 _totalEnemiesSpawned = 0;
        private Coroutine           _spawnCoroutine;
        private EnemySpawner        _enemySpawner;

        //  METHODS        
        private void Awake()
        {
            _enemySpawner = GetComponent<EnemySpawner>();
            if (_enemySpawner == null)
            {
                _enemySpawner = gameObject.AddComponent<EnemySpawner>();
            }
        }
        
        private void Start()
        {
            if (_levels != null && _levels.LevelsArray.Length > 0)
            {
                PrepareLevel(0);
            }
        }
        
        public void PrepareLevel(int levelIndex)
        {
            if (_levels == null || 
                levelIndex >= _levels.LevelsArray.Length || 
                levelIndex < 0)
            {
                Debug.LogError($"Invalid level index!");
                return;
            }
            
            _currentLevelIndex = levelIndex;
            _currentLevelData = _levels.LevelsArray[levelIndex];
            
            // First, clear all enemies
            ClearAllEnemies();
            
            // Reset spawn state
            _isSpawning = false;            
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
        
        public void PrepareNextLevel()
        {
            int nextLevelIndex = _currentLevelIndex + 1;
            
            if (nextLevelIndex < _levels.LevelsArray.Length)
            {
                PrepareLevel(nextLevelIndex);
            }
            else
            {
                PrepareLevel(0);
            }
        }
        
        public void PrepareCurrentLevel()
        {
            PrepareLevel(_currentLevelIndex);
        }
        
        public void StartEnemySpawning()
        {
            if (_currentLevelData == null)
            {
                Debug.LogError("No level data available");
                return;
            }
            
            if (_isSpawning)
            {
                Debug.LogError("Already spawning");
                return;
            }
            
            _isSpawning = true;
            
            _enemySpawner.Initialize(_currentLevelData, _enemySpawnPoints, this);
            
            // Start spawning enemies
            _spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
        
        public void StopEnemySpawning()
        {
            if (!_isSpawning)
            {
                return;
            }
            
            _isSpawning = false;
            
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
        
        // Spawn coroutine
        private IEnumerator SpawnEnemies()
        {
            while (_isSpawning && _currentLevelData != null)
            {
                // Check if we should spawn more enemies
                bool canSpawn = _activeEnemies.Count < _currentLevelData.MaxEnemiesAtOnce;
                
                canSpawn = _totalEnemiesSpawned < _currentLevelData.MaxEnemies;
                
                if (canSpawn && _enemySpawnPoints.Length > 0)
                {
                    _enemySpawner.SpawnRandomEnemy();
                    _totalEnemiesSpawned++;
                }
                
                // Wait for next spawn
                float spawnInterval = _currentLevelData.SpawnInterval / _currentLevelData.SpawnRateMultiplier;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        
        public void RegisterEnemy(GameObject enemy)
        {
            if (!_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Add(enemy);
            }
        }
        
        public void UnregisterEnemy(GameObject enemy)
        {
            _activeEnemies.Remove(enemy);
        }
        
        public void ClearAllEnemies()
        {
            foreach (GameObject enemy in _activeEnemies.ToArray())
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            _activeEnemies.Clear();
            _totalEnemiesSpawned = 0;
        }
        
        public void StopAllEnemies()
        {
            foreach (GameObject enemy in _activeEnemies)
            {
                if (enemy != null)
                {
                    // Disable enemy AI/movement
                    EnemyView enemyView = enemy.GetComponent<EnemyView>();
                    enemyView.enabled = false;
                }
            }
        }
        
        // Utility methods for GameManager integration
        public float GetLevelDuration()
        {
            return _currentLevelData != null ? _currentLevelData.LevelDuration : 0f;
        }
        
        public bool HasNextLevel()
        {
            return _levels != null && _currentLevelIndex + 1 < _levels.LevelsArray.Length;
        }
        
        public int GetTotalLevelCount()
        {
            return _levels != null ? _levels.LevelsArray.Length : 0;
        }
    }
}