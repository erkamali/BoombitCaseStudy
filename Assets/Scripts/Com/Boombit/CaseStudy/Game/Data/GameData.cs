using System.Collections.Generic;
using Com.Boombit.CaseStudy.Game.Constants;
using Com.Boombit.CaseStudy.Main.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Data
{
    public class GameData : IGameData
    {
        //  MEMBERS
        //      Properties
        public int              CurrentLevelKills   { get { return _currentLevelKillCount; } }
        public int              TotalKills          { get { return _totalKillCount; } }
        public int              CurrentLevel        { get { return _currentLevel; } }

        public PlayerData                   Player  { get { return _player; } }
        public Dictionary<int, EnemyData>   Enemies { get { return _enemies; } }

        //      Private
        private int _currentLevelKillCount  = 0;
        private int _totalKillCount         = 0;
        private int _currentLevel           = 0;

        private PlayerData                  _player;
        private Dictionary<int, EnemyData>  _enemies;
        private int                         _nextEnemyId;

        
        //  CONSTRUCTORS
        public GameData()
        {
            _nextEnemyId = 1;
            _enemies = new Dictionary<int, EnemyData>();
            LoadData();
        }

        //  METHODS
#region IGameData implementations

        public void IncrementKillCount()
        {
            _currentLevelKillCount++;
        }
        
        public void ResetCurrentLevelKillCount()
        {
            _currentLevelKillCount = 0;
        }
        
        public void CompleteLevel()
        {
            _totalKillCount += _currentLevelKillCount;
            _currentLevel++;
            _currentLevelKillCount = 0;

            SaveData();
        }
        
        public void SaveData()
        {
            PlayerPrefs.SetInt(GameConstants.TOTAL_KILLS,   _totalKillCount);
            PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, _currentLevel);
            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            if (PlayerPrefs.HasKey(GameConstants.TOTAL_KILLS))
            {
                _totalKillCount = PlayerPrefs.GetInt(GameConstants.TOTAL_KILLS);
            }
            else
            {
                _totalKillCount = 0;
            }

            if (PlayerPrefs.HasKey(GameConstants.CURRENT_LEVEL))
            {
                _currentLevel = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL);
            }
            else
            {
                _currentLevel = 0;
            }
            _currentLevelKillCount = 0;
        }

#endregion

        public PlayerData CreatePlayer(PlayerConfig config)
        {
            _player = new PlayerData(
                0,
                config.MaxHealth,
                config.MaxHealth, // Start with full health
                config.MoveSpeed,
                config.RotationSpeed,
                config.AttackDamage,
                config.AttackRange,
                config.InputDeadZone,
                config.WeaponFireRate
            );
            
            return _player;
        }

        public EnemyData CreateEnemy(EnemyConfig config)
        {
            EnemyData enemy = new EnemyData(
                _nextEnemyId,
                config.MaxHealth,
                config.MaxHealth, // Start with full health
                config.MoveSpeed,
                config.RotationSpeed,
                config.AttackDamage,
                config.AttackRange,
                config.PathUpdatePeriod,
                config.AttackCooldown
            );
            
            _enemies[_nextEnemyId] = enemy;
            _nextEnemyId++;
            return enemy;
        }

        public bool HasEnemy(int id)
        {
            return _enemies.ContainsKey(id);
        }

        public EnemyData GetEnemy(int id)
        {
            if (_enemies.ContainsKey(id))
            {
                return _enemies[id];
            }

            return null;
        }

        public IEnumerator<EnemyData> GetEnemies()
        {
            return _enemies.Values.GetEnumerator();
        }

        public void RemoveEnemy(int id)
        {
            _enemies.Remove(id);
        }
        
        public void SetCurrentLevel(int level)
        {
            _currentLevel = level;
            SaveData();
        }
        
        public void SetTotalKills(int kills)
        {
            _totalKillCount = kills;
            SaveData();
        }
        
        public void ResetAllData()
        {
            _currentLevelKillCount  = 0;
            _totalKillCount         = 0;
            _currentLevel           = 1;

            _player = null;
            _enemies.Clear();
            
            PlayerPrefs.DeleteKey(GameConstants.TOTAL_KILLS);
            PlayerPrefs.DeleteKey(GameConstants.CURRENT_LEVEL);
            PlayerPrefs.Save();
        }
    }
}