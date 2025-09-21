using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Views
{
    public class GamePauseUIView : BaseUIView
    {
        //  MEMBERS
        //      Editor
        [Header("UI Elements")]
        [SerializeField] private Button     _resumeButton;
        [SerializeField] private Button     _mainMenuButton;
        [SerializeField] private Button     _restartButton;
        [SerializeField] private TMP_Text   _pausedText;
        
        //  MEMBERS
        protected override void SetupUI()
        {
            _resumeButton.onClick.AddListener(() =>   _gameManager.ResumeGame());    
            _mainMenuButton.onClick.AddListener(() => _gameManager.BackToMainMenu());    
            _restartButton.onClick.AddListener(() =>  _gameManager.RestartLevel());
        }
        
        protected override void OnShow()
        {
            _pausedText.text = "Game Paused";
        }
    }
}