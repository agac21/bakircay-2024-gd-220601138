using System.Collections.Generic;
using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;

namespace MidtermHomeworkAssets.Scripts
{
    public class ObjectTracker
    {
        private readonly List<IInteractableObject> _runtimeObjects = new();
        private GameEventHub _eventHub;

        public ObjectTracker(GameEventHub eventHub)
        {
            _eventHub = eventHub;
        }

        public void Init()
        {
            _eventHub.OnAdd += Add;
            _eventHub.OnRemove += Remove;
        }

        public void DeInit()
        {
            _eventHub.OnAdd -= Add;
            _eventHub.OnRemove -= Remove;
        }

        private void Add(IInteractableObject obj)
        {
            _runtimeObjects.Add(obj);
        }

        private void Remove(IInteractableObject obj)
        {
            _runtimeObjects.Remove(obj);
        }

        public IReadOnlyList<IInteractableObject> GetList()
        {
            return _runtimeObjects;
        }
    }
}