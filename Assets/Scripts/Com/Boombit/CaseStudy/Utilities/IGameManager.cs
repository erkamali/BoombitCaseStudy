using Com.Boombit.CaseStudy.Data;

namespace Com.Boombit.CaseStudy.Utilities
{
    public interface IGameManager
    {
        TimeManager TimeManager { get; }
        IGameData   GameData    { get; }
        
        // UI related
        void ShowMainMenuUI();
        void HideMainMenuUI();
        void ShowGameUI();
        void HideGameUI();
        void ShowLevelSuccessUI(int levelKills, int totalKills);
        void HideLevelSuccessUI();
        void ShowLevelFailUI();
        void HideLevelFailUI();
        void ShowGamePauseUI();
        void HideGamePauseUI();
        
        // Game state related
        void EnableControls();
        void DisableControls();
        void StartEnemySpawning();
        void StopEnemySpawning();
        void StopAllEnemies();
    }
}