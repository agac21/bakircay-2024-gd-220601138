using System;
using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;

namespace MidtermHomeworkAssets.Scripts
{
    public class GameEventHub
    {
        public event Action<IInteractableObject> OnAdd;
        public event Action<IInteractableObject> OnRemove;

        public void Add(IInteractableObject obj)
        {
            OnAdd?.Invoke(obj);
        }

        public void Remove(IInteractableObject obj)
        {
            OnRemove?.Invoke(obj);
        }
    }
}