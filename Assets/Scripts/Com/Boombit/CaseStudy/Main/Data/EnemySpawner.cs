using System.Collections.Generic;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    public class EnemySpawner : MonoBehaviour
    {
        //  MEMBERS
        //      Private
        private LevelData           _levelData;
        private Transform[]         _spawnPoints;
        private LevelManager        _levelManager;
        private List<EnemyTypeInfo> _availableEnemyTypes = new List<EnemyTypeInfo>();
        
        //  METHODS
        public void Initialize(LevelData data, Transform[] spawnPoints, LevelManager levelManager)
        {
            _levelData      = data;
            _spawnPoints    = spawnPoints;
            _levelManager   = levelManager;
            
            RefreshAvailableEnemyTypes();
        }
        
        public GameObject SpawnRandomEnemy()
        {
            if (_availableEnemyTypes.Count == 0)
            {
                RefreshAvailableEnemyTypes();
                if (_availableEnemyTypes.Count == 0)
                {
                    Debug.LogError("No available enemy!");
                    return null; 
                }
            }
            
            EnemyTypeInfo selectedEnemyType = SelectWeightedEnemyType();
            if (selectedEnemyType == null)
            {
                Debug.LogError("Could not select an enemy type!");
                return null;
            }
            
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            
            GameObject enemyObj = Instantiate(selectedEnemyType.EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Register with level manager
            _levelManager.RegisterEnemy(enemyObj);
            
            // Add cleanup component
            EnemyCleanup cleanup = enemyObj.GetComponent<EnemyCleanup>();
            if (cleanup == null)
            {
                cleanup = enemyObj.AddComponent<EnemyCleanup>();
            }
            cleanup.Initialize(_levelManager);
            
            return enemyObj;
        }
        
        private void RefreshAvailableEnemyTypes()
        {
            _availableEnemyTypes.Clear();
            
            foreach (var enemyType in _levelData.EnemyTypes)
            {
                if (Time.timeSinceLevelLoad >= enemyType.MinSpawnDelay)
                {
                    _availableEnemyTypes.Add(enemyType);
                }
            }
        }
        
        private EnemyTypeInfo SelectWeightedEnemyType()
        {
            float totalWeight = 0f;
            foreach (var enemyType in _availableEnemyTypes)
            {
                totalWeight += enemyType.SpawnWeight;
            }
            
            if (totalWeight <= 0f)
            {
                // All weights 0, select random
                return _availableEnemyTypes[Random.Range(0, _availableEnemyTypes.Count)];
            }
            
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;
            
            foreach (var enemyType in _availableEnemyTypes)
            {
                currentWeight += enemyType.SpawnWeight;
                if (randomValue <= currentWeight)
                {
                    return enemyType;
                }
            }
            
            // Could not select any, select first
            return _availableEnemyTypes[0];
        }
    }
}