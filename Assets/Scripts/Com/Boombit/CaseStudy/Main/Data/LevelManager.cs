using System.Collections.Generic;
using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Game.Utilities;
using Com.Boombit.CaseStudy.Game.Views;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    public class LevelManager : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("Level Configuration")]
        [SerializeField] private Levels _levels;

        //      Private
        private PlayerView                  _player;
        private Dictionary<int, EnemyView>  _enemies;
        private GameObject                  _currentLevelLayout;
        private IGameManager                _gameManager;

        //      Properties
        public PlayerView   Player      { get { return _player; } }
        public int          EnemyCount  { get { return _enemies.Count; } }

        //  METHODS
        public void Initialize(IGameManager gameManager)
        {
            _enemies = new Dictionary<int, EnemyView>();
            _gameManager = gameManager;
        }

        public void LoadLevel(int levelIndex)
        {
            if (!IsValidLevelIndex(levelIndex))
            {
                Debug.LogError("Invalid level index: " + levelIndex);
                return;
            }
            
            LevelData levelData = _levels.GetLevel(levelIndex);
            
            CreateLevelLayout(levelData);
            
            CreatePlayer(levelData);
            
            CreateEnemies(levelData);
        }

        public void UnloadLevel()
        {
            if (_player != null)
            {
                Destroy(_player.gameObject);
                _player = null;
            }
            
            foreach (var enemy in _enemies.Values)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            _enemies.Clear();
            
            if (_currentLevelLayout != null)
            {
                Destroy(_currentLevelLayout);
                _currentLevelLayout = null;
            }
        }

        private void CreateLevelLayout(LevelData levelData)
        {
            if (levelData.LevelLayoutPrefab != null)
            {
                _currentLevelLayout = Instantiate(levelData.LevelLayoutPrefab);
            }
            else
            {
                Debug.LogWarning("No level layout prefab found");
            }
        }

        private void CreatePlayer(LevelData levelData)
        {
            if (levelData.PlayerPrefab == null)
            {
                Debug.LogError("No player prefab found");
                return;
            }
            
            if (levelData.PlayerSpawnTransform == null)
            {
                Debug.LogError("No player spawn transform found");
                return;
            }
            
            PlayerData playerData = _gameManager.CreatePlayerData(levelData.PlayerConfig);
            
            Vector3     spawnPos    = levelData.PlayerSpawnTransform.position;
            Quaternion  spawnRot    = levelData.PlayerSpawnTransform.rotation;
            GameObject  playerObj   = Instantiate(levelData.PlayerPrefab, spawnPos, spawnRot);
            
            _player = playerObj.GetComponent<PlayerView>();
            if (_player != null)
            {
                _player.Initialize(playerData, _gameManager);
            }
        }

        private void CreateEnemies(LevelData levelData)
        {
            if (levelData.EnemyTypes == null || levelData.EnemyTypes.Length == 0)
            {
                Debug.LogWarning("No enemy types for this level");
                return;
            }
            
            for (int i = 0; i < levelData.MaxEnemies; i++)
            {
                CreateEnemy(levelData);
            }
        }

        private void CreateEnemy(LevelData levelData)
        {
            EnemyTypeInfo selectedEnemyType = SelectWeightedEnemyType(levelData.EnemyTypes);
            if (selectedEnemyType == null || selectedEnemyType.EnemyPrefab == null)
            {
                Debug.LogError("Failed to select valid enemy type");
                return;
            }
            
            EnemyData enemyData = _gameManager.CreateEnemyData(selectedEnemyType.EnemyConfig);
            Vector3 spawnPos = GetRandomSpawnPosition(levelData);
            GameObject enemyObj = Instantiate(selectedEnemyType.EnemyPrefab, spawnPos, Quaternion.identity);
            
            EnemyView enemyView = enemyObj.GetComponent<EnemyView>();
            if (enemyView != null && _player != null)
            {
                enemyView.Initialize(enemyData, _player.transform, _gameManager);
                _enemies[enemyData.ID] = enemyView;
            }
            else
            {
                Debug.LogError("EnemyView component not found");
                Destroy(enemyObj);
            }
        }

        private EnemyTypeInfo SelectWeightedEnemyType(EnemyTypeInfo[] enemyTypes)
        {
            if (enemyTypes.Length == 0) return null;
            
            float totalWeight = 0f;
            foreach (var enemyType in enemyTypes)
            {
                totalWeight += enemyType.SpawnWeight;
            }
            
            if (totalWeight <= 0f)
            {
                return enemyTypes[Random.Range(0, enemyTypes.Length)];
            }
            
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;
            
            foreach (var enemyType in enemyTypes)
            {
                currentWeight += enemyType.SpawnWeight;
                if (randomValue <= currentWeight)
                {
                    return enemyType;
                }
            }
            
            // Fallback
            return enemyTypes[0];
        }

        private Vector3 GetRandomSpawnPosition(LevelData levelData)
        {
            if (levelData.EnemySpawnPoints == null || levelData.EnemySpawnPoints.Length == 0)
            {
                // Fallback to random position
                float range = 10f;
                return new Vector3(
                    Random.Range(-range, range),
                    0f,
                    Random.Range(-range, range)
                );
            }
            
            Transform spawnPoint = levelData.EnemySpawnPoints[Random.Range(0, levelData.EnemySpawnPoints.Length)];
            return spawnPoint.position;
        }

        private bool IsValidLevelIndex(int levelIndex)
        {
            return _levels != null && _levels.IsValidLevelIndex(levelIndex);
        }

#region CharacterView methods

        public EnemyView GetEnemy(int enemyId)
        {
            _enemies.TryGetValue(enemyId, out EnemyView enemy);
            return enemy;
        }
        
        public bool HasEnemy(int enemyId)
        {
            return _enemies.ContainsKey(enemyId);
        }
        
        public void RemoveEnemy(int enemyId)
        {
            if (_enemies.ContainsKey(enemyId))
            {
                EnemyView enemy = _enemies[enemyId];
                _enemies.Remove(enemyId);
                
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

#endregion

        public float GetLevelDuration(int levelIndex)
        {
            if (IsValidLevelIndex(levelIndex))
            {
                return _levels.GetLevel(levelIndex).LevelDuration;
            }
            return 0f;
        }

        public int GetTotalLevelCount()
        {
            return _levels.LevelsArray.Length;
        }
    }
}