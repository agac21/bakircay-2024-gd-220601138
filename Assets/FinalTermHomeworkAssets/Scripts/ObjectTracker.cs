using System.Collections.Generic;
using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;

namespace FinalTermHomeworkAssets.Scripts
{
    public class ObjectTracker
    {
        private readonly List<IInteractableObject> _runtimeObjects = new();
        private GameEventHub _eventHub;
        private PlacementArea _placementArea;

        public int MatchedScore { get; private set; }

        public ObjectTracker(GameEventHub eventHub,PlacementArea placementArea)
        {
            _placementArea = placementArea;
            _eventHub = eventHub;
        }

        public void Init()
        {
            _eventHub.OnAdd += Add;
            _eventHub.OnRemove += Remove;
            _eventHub.OnMatched += IncreaseScore;
        }

        public void DeInit()
        {
            _eventHub.OnAdd -= Add;
            _eventHub.OnRemove -= Remove;
            _eventHub.OnMatched -= IncreaseScore;
        }

        private void IncreaseScore()
        {
            MatchedScore++;
            _eventHub.ScoreChanged(MatchedScore);
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

        public PlacementArea GetPlacementArea()
        {
            return _placementArea;
        }

        public void ResetScore()
        {
            MatchedScore = 0;
            _eventHub.ScoreChanged(MatchedScore);
        }
    }
}