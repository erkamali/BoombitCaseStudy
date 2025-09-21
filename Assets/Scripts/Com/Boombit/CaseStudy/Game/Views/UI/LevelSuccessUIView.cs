using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public class LevelSuccessUIView : BaseUIView
    {
        //  MEMBERS
        //      Editor
        [Header("UI Elements")]
        [SerializeField] private Button     _nextLevelButton;
        [SerializeField] private Button     _mainMenuButton;
        [SerializeField] private TMP_Text   _levelSuccessText;
        [SerializeField] private TMP_Text   _levelKillCountText;
        [SerializeField] private TMP_Text   _totalKillCountText;

        //  MEMBERS        
        protected override void SetupUI()
        {
            _nextLevelButton.onClick.AddListener(() => _gameManager.NextLevel());
            _mainMenuButton.onClick.AddListener(() =>  _gameManager.BackToMainMenu());
        }

        protected override void OnShow()
        {
            UpdateStats(_gameManager.GameData.CurrentLevelKills, _gameManager.GameData.TotalKills);
        }

        public void UpdateStats(int levelKills, int totalKills)
        {
            _levelSuccessText.text   = "Level Complete!";
            _levelKillCountText.text = $"Enemies Defeated: {levelKills}";
            _totalKillCountText.text = $"Total Kills: {totalKills}";
        }
    }
}