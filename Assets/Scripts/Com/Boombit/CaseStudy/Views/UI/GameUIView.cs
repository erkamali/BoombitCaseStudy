using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Views
{
    public class GameUIView : BaseUIView
    {
        //  MEMBERS
        //      Editor
        [Header("UI Elements")]
        [SerializeField] private Button     _pauseButton;
        [SerializeField] private TMP_Text   _currentLevelKillCountText;
        [SerializeField] private TMP_Text   _totalKillCountText;
        [SerializeField] private TMP_Text   _currentLevelText;
        
        [Header("Timer UI")]
        [SerializeField] private Image      _timerBar;
        [SerializeField] private TMP_Text   _timerText;
        [SerializeField] private GameObject _timerContainer;

        //  METHODS        
        protected override void SetupUI()
        {
            _pauseButton.onClick.AddListener(() => _gameManager.PauseGame());
        }
        
        protected override void OnShow()
        {
            UpdateGameplayUI();
            
            _timerContainer.SetActive(true);
        }
        
        protected override void OnHide()
        {
            _timerContainer.SetActive(false);
        }
        
        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                UpdateGameplayUI();
            }
        }
        
        private void UpdateGameplayUI()
        {
            _currentLevelKillCountText.text = $"Kills: {_gameManager.GameData.CurrentLevelKills}";    
            _totalKillCountText.text        = $"Kills: {_gameManager.GameData.TotalKills}";    
            _currentLevelText.text          = $"Level: {_gameManager.GameData.CurrentLevel}";
        }
        
        // Called by GameManager when time updates
        public void OnTimeUpdated(float currentTime, float normalizedTime)
        {
            _timerBar.fillAmount = normalizedTime;
            
            // Change color based on remaining time
            if (normalizedTime < 0.2f)
            {
                _timerBar.color = Color.red;
            }
            else if (normalizedTime < 0.5f)
            {
                _timerBar.color = Color.yellow;
            }
            else
            {
                _timerBar.color = Color.green;
            }
            
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}