using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Main.Data;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public interface IGameManager
    {
        TimeManager TimeManager { get; }
        IGameData   GameData    { get; }

        // Level related
        PlayerData CreatePlayerData(PlayerConfig playerConfig);
        EnemyData CreateEnemyData(EnemyConfig enemyConfig);

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
        void StopAllEnemies();

        void OnCharacterTakeDamage(int characterId, float damage);
        void OnCharacterDied(int characterId);

        // Time related
        void OnTimeUpdated(float currentTime, float normalizedTime);
        void OnTimerFinished();

        // View callbacks
        void StartGame();
        void PauseGame();
        void ResumeGame();
        void RestartLevel();
        void NextLevel();
        void BackToMainMenu();
        void QuitGame();
    }
}