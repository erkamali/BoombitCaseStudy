using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public class LevelFailUIView : BaseUIView
    {
        //  MEMBERS
        //      Editor
        [Header("UI Elements")]
        [SerializeField] private Button     _restartButton;
        [SerializeField] private Button     _mainMenuButton;
        [SerializeField] private TMP_Text   _levelFailText;
        [SerializeField] private TMP_Text   _levelKillCountText;
        [SerializeField] private TMP_Text   _totalKillCountText;

        //  MEMBERS        
        protected override void SetupUI()
        {
            _restartButton.onClick.AddListener(() =>  _gameManager.RestartLevel());    
            _mainMenuButton.onClick.AddListener(() => _gameManager.BackToMainMenu());
        }
        
        protected override void OnShow()
        {
            UpdateStats(_gameManager.GameData.CurrentLevelKills, _gameManager.GameData.TotalKills);
        }

        public void UpdateStats(int levelKills, int totalKills)
        {
            _levelFailText.text      = "Game Over!";
            _levelKillCountText.text = $"Enemies Defeated: {levelKills}";
            _totalKillCountText.text = $"Total Kills: {totalKills}";
        }
    }
}