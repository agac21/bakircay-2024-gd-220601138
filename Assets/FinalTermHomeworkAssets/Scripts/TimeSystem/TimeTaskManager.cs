using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.TimeSystem
{
    public class TimeTaskManager
    {
        private static TimeTaskManager _timeTaskManager;

        public static TimeTaskManager Instance
        {
            get
            {
                _timeTaskManager ??= new TimeTaskManager(LocalTimeProvider.Instance);
                return _timeTaskManager;
            }
        }

        private ITimeProvider _timeProvider;

        private WaitForSeconds _secondWait;

        private readonly List<TimeTask> _timeTaskList = new();

        private readonly List<TimeTask> _timeTaskRemoveQueue = new();
        private bool _invokingTimeTasks;

        private bool _isInitialized;

        private TimeTaskManager(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            _timeProvider.OnTick += UpdateTimeTasks;
        }

        public TimeTask Register(DateTime startTime, TimeSpan requiredTime, Action<int> onUpdate = null, Action onTrigger = null)
        {
            var remainingTime = (int)(startTime + requiredTime - _timeProvider.Now).TotalSeconds;
            return Register(remainingTime, onUpdate, onTrigger);
        }

        public TimeTask Register(int duration, Action<int> onUpdate = null, Action onTrigger = null)
        {
            var timeTask = new TimeTask
            {
                period = duration,
                OnUpdate = onUpdate,
                OnTrigger = onTrigger,
            };
            AddTimeTask(timeTask);
            return timeTask;
        }

        public void AddTimeTask(TimeTask timeTask)
        {
            if (IsActive(timeTask)) return;

            var startDate = _timeProvider.Now;
            timeTask.triggerDate = startDate + TimeSpan.FromSeconds(timeTask.period);

            _timeTaskList.Add(timeTask);
            OnUpdateTimeTask(timeTask, startDate);
        }

        public void RemoveTimeTask(TimeTask timeTask)
        {
            if (timeTask == null) return;

            var index = _timeTaskList.IndexOf(timeTask);
            if (index == -1) return;

            if (_invokingTimeTasks) _timeTaskRemoveQueue.Add(timeTask);
            else
            {
                timeTask.OnUpdate?.Invoke(-1);
                _timeTaskList.RemoveAt(index);
            }
        }

        public bool IsActive(TimeTask timeTask)
        {
            return _timeTaskList.Contains(timeTask);
        }

        private void UpdateTimeTasks(DateTime now)
        {
            for (var index = _timeTaskList.Count - 1; index >= 0; index--)
            {
                var timeTask = _timeTaskList[index];
                if (now >= timeTask.triggerDate)
                {
                    timeTask.OnTrigger?.Invoke();

                    if (timeTask.isRepeating)
                    {
                        timeTask.triggerDate += TimeSpan.FromSeconds(timeTask.period + timeTask.cooldown);
                    }
                    else
                    {
                        _timeTaskList.Remove(timeTask);
                    }
                }

                OnUpdateTimeTask(timeTask, now);
            }

            _timeTaskList.RemoveAll(task => _timeTaskRemoveQueue.Contains(task));
            _timeTaskRemoveQueue.Clear();
        }

        private void OnUpdateTimeTask(TimeTask timeTask, DateTime now)
        {
            try
            {
                var remainingTime = Mathf.Clamp((int)(timeTask.triggerDate - now).TotalSeconds, 0, int.MaxValue);
                timeTask.cachedRemaniningTime = remainingTime;
                timeTask.OnUpdate?.Invoke(remainingTime);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}