using System;

namespace FinalTermHomeworkAssets.Scripts.TimeSystem
{
    public class TimeTask
    {
        public int period;
        public int cooldown;
        public bool isRepeating;
        public Action<int> OnUpdate;
        public Action OnTrigger;
        public DateTime triggerDate;

        public int cachedRemaniningTime = -1;

        public void Reset()
        {
            TimeTaskManager.Instance.RemoveTimeTask(this);
            OnTrigger = null;
            OnUpdate = null;
        }

        public static TimeTask Create(TimeSpan remainingTime, Action onTrigger = null)
        {
            var timeTask = new TimeTask
            {
                period = (int)remainingTime.TotalSeconds,
                OnTrigger = onTrigger,
            };
            return timeTask;
        }
    }
}