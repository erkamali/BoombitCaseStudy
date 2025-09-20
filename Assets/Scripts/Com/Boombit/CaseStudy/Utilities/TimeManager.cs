using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Utilities
{
    public class TimeManager : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("UI References")]
        [SerializeField] private Image      _remainingTimeBar;
        [SerializeField] private TMP_Text   _remainingTimeText;
        
        //      Private
        private float   _levelDuration;
        private float   _currentTime;
        private bool    _isTimerRunning = false;
        
        //      Events
        public static event Action          OnTimerFinished;

        //  METHODS
        private void Init(float duration)
        {
            _levelDuration = duration;
            _currentTime = duration;
            _isTimerRunning = false;
            UpdateUI();
        }

        private void Update()
        {
            if (_isTimerRunning && _currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                UpdateUI();
                
                if (_currentTime <= 0)
                {
                    TimerFinished();
                }
            }
        }

        public void StartTimer()
        {
            if (_currentTime > 0)
            {
                _isTimerRunning = true;
            }
        }

        public void PauseTimer()
        {
            _isTimerRunning = false;
        }

        public void ResumeTimer()
        {
            if (_currentTime > 0)
            {
                _isTimerRunning = true;
            }
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
            _currentTime = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _remainingTimeBar.fillAmount = _currentTime / _levelDuration;

            _remainingTimeText.text = _currentTime.ToString();
        }

        private void TimerFinished()
        {
            _isTimerRunning = false;
            _currentTime = 0;
            UpdateUI();
            
            // Notify other systems that timer finished
            OnTimerFinished?.Invoke();
        }

        public float GetRemainingTime()
        {
            return _currentTime;
        }

        public bool IsTimerRunning()
        {
            return _isTimerRunning;
        }

        public bool IsTimerFinished()
        {
            return _currentTime <= 0;
        }
    }
}