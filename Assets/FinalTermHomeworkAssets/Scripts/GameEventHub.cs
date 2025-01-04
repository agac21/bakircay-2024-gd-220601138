using System;
using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;

namespace FinalTermHomeworkAssets.Scripts
{
    public class GameEventHub
    {
        public event Action<IInteractableObject> OnAdd;
        public event Action<IInteractableObject> OnRemove;
        public event Action OnMatched;
        public event Action<int> OnScoreChanged;

        public void Add(IInteractableObject obj)
        {
            OnAdd?.Invoke(obj);
        }

        public void Remove(IInteractableObject obj)
        {
            OnRemove?.Invoke(obj);
        }

        public void Matched()
        {
            OnMatched?.Invoke();
        }
        public void ScoreChanged(int score)
        {
            OnScoreChanged?.Invoke(score);
        }
    }
}