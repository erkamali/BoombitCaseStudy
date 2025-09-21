using System;
using TMPro;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class TimeManager : MonoBehaviour
    {
        //  MEMBERS
        //      Private
        private float   _levelDuration;
        private float   _currentTime;
        private bool    _isTimerRunning = false;
        
        //      Events
        public static event Action                  OnTimerFinished;
        public static event Action<float, float>    OnTimeUpdated;

        //  METHODS
        public void Init(float duration)
        {
            _levelDuration  = duration;
            _currentTime    = duration;
            _isTimerRunning = false;
            
            float normalizedTime = _currentTime / _levelDuration;
            OnTimeUpdated.Invoke(_currentTime, normalizedTime);
        }

        private void Update()
        {
            if (_isTimerRunning && _currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                
                float normalizedTime = _currentTime / _levelDuration;
                OnTimeUpdated.Invoke(_currentTime, normalizedTime);
                
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
                UnfreezeGame();
            }
        }

        public void PauseTimer()
        {
            _isTimerRunning = false;
            FreezeGame();
        }

        public void ResumeTimer()
        {
            if (_currentTime > 0)
            {
                _isTimerRunning = true;
                UnfreezeGame();
            }
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
            _currentTime    = 0;
            UnfreezeGame();
        
            OnTimeUpdated.Invoke(0f, 0f);
        }

        public void FreezeGame()
        {
            Time.timeScale = 0f;
        }

        public void UnfreezeGame()
        {
            Time.timeScale = 1f;
        }

        private void TimerFinished()
        {
            _isTimerRunning = false;
            _currentTime    = 0;
            
            OnTimeUpdated.Invoke(0f, 0f);
            
            OnTimerFinished.Invoke();
        }

        public float GetRemainingTime()
        {
            return _currentTime;
        }

        public float GetRemainingTimeNormalized()
        {
            if (_levelDuration > 0)
            {
                return _currentTime / _levelDuration;
            }
            else
            {
                return 0f;
            }
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