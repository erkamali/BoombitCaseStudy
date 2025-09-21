namespace Com.Boombit.CaseStudy.Game.Data
{
    public interface IGameData
    {
        int CurrentLevelKills   { get; }
        int TotalKills          { get; }
        int CurrentLevel        { get; }
        
        void IncrementKillCount();
        void ResetCurrentLevelKillCount();
        void CompleteLevel();
        void LoadData();
        void SaveData();
    }
}