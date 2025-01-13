using System;
using FinalTermHomeworkAssets.Scripts.TimeSystem;

namespace FinalTermHomeworkAssets.Scripts.SkillSystem
{
    public class Skill
    {
        private int _duration;
        private bool _isActive = true;
        public TimeTask TimeTask { get; private set; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnActiveToggled?.Invoke(_isActive);
            }
        }

        public event Action<bool> OnActiveToggled;

        public Skill(int duration)
        {
            _duration = duration;
        }

        public bool TryApply()
        {
            if (!IsActive) return false;
            TimeTask = TimeTask.Create(TimeSpan.FromSeconds(_duration), () => IsActive = true);
            TimeTaskManager.Instance.AddTimeTask(TimeTask);
            IsActive = false;
            ApplyInner();
            return true;
        }

        protected virtual void ApplyInner()
        {
        }

        public void Reset()
        {
            TimeTask?.Reset();
            IsActive = true;
        }
    }
}