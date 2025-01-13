using System;

namespace FinalTermHomeworkAssets.Scripts.TimeSystem
{
    public interface ITimeProvider
    {
        public event Action<DateTime> OnTick;
        public DateTime Now { get; }
    }
}