using Com.Boombit.CaseStudy.Constants;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Data
{
    public class GameData : IGameData
    {
        //  MEMBERS
        //      Properties
        public int CurrentLevelKills    { get { return _currentLevelKillCount; } }
        public int TotalKills           { get { return _totalKillCount; } }
        public int CurrentLevel         { get { return _currentLevel; } }

        //      Private
        private int _currentLevelKillCount  = 0;
        private int _totalKillCount         = 0;
        private int _currentLevel           = 1;

        
        //  CONSTRUCTORS
        public GameData()
        {
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
            _totalKillCount = PlayerPrefs.GetInt(GameConstants.TOTAL_KILLS, 0);
            _currentLevel   = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
            _currentLevelKillCount = 0;
        }

#endregion
        
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
            
            PlayerPrefs.DeleteKey(GameConstants.TOTAL_KILLS);
            PlayerPrefs.DeleteKey(GameConstants.CURRENT_LEVEL);
            PlayerPrefs.Save();
        }
    }
}