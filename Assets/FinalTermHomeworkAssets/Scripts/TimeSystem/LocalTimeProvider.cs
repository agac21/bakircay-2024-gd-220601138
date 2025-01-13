using System;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.TimeSystem
{
    public class LocalTimeProvider : MonoBehaviour, ITimeProvider
    {
        public static LocalTimeProvider Instance;
        public event Action<DateTime> OnTick;
        public DateTime Now => DateTime.Now;

        private float _counter;

        private void Awake()
        {
            _counter = 0;
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _counter += Time.deltaTime;
            if (_counter >= 1)
            {
                _counter = 0;
                OnTick?.Invoke(DateTime.Now);
            }
        }
    }
}