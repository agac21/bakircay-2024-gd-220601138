using System;
using TMPro;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.TimeSystem.UI
{
    public class TimeTaskView : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI m_timerView;

        private TimeTask _currentTask;

        private void OnDestroy()
        {
            DeInit();
        }

        public void Init(TimeTask timeTask)
        {
            if (_currentTask != null) DeInit();
            if (timeTask == null) return;
            _currentTask = timeTask;

            _currentTask.OnUpdate += UpdateTime;
            _currentTask.OnTrigger += SetCompleteView;

            SetTimerView();
            UpdateTime(_currentTask.cachedRemaniningTime);
        }

        public void DeInit()
        {
            if (_currentTask == null) return;
            _currentTask.OnUpdate -= UpdateTime;
            _currentTask.OnTrigger -= SetCompleteView;
            _currentTask = null;
        }

        private void SetTimerView()
        {
            m_timerView.gameObject.SetActive(true);
        }

        private void SetCompleteView()
        {
            if (m_timerView != null && m_timerView.gameObject != null)
            {
                m_timerView.gameObject.SetActive(false);
            }
        }

        private void UpdateTime(int remainingTime)
        {
            if (_currentTask == null) return;
            if (!m_timerView.gameObject.activeInHierarchy) m_timerView.gameObject.SetActive(true);
            m_timerView.SetText(ToString(TimeSpan.FromSeconds(remainingTime)));
        }

        private static string ToString(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
            {
                return $"{timeSpan.Days}d {timeSpan.Hours}h";
            }

            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
            }

            if (timeSpan.Minutes > 0)
            {
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            }

            return $"{timeSpan.Seconds}s";
        }
    }
}