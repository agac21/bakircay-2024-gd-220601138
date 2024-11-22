using System.Collections.Generic;
using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 xRange;
        [SerializeField] private Vector2 yRange;
        [SerializeField] private Vector2 zRange;

        [SerializeField] private List<InteractiveObject> objectPrefabs;
        private GameEventHub _eventHub;

        public void SetDependencies(GameEventHub eventHub)
        {
            _eventHub = eventHub;
        }

        public void GenerateObjects()
        {
            var spawnCount = Random.Range(5, 10);

            for (var count = 0; count < spawnCount; count++)
            {
                var chosenPrefab = SelectRandomPrefab();
                var spawnPosition = GetRandomPosition();
                var spawnedInstance = Instantiate(chosenPrefab, spawnPosition, Quaternion.identity);

                ApplyRandomProperties(spawnedInstance.transform);

                _eventHub.Add(spawnedInstance);
            }
        }

        private InteractiveObject SelectRandomPrefab()
        {
            return objectPrefabs[Random.Range(0, objectPrefabs.Count)];
        }

        private Vector3 GetRandomPosition()
        {
            var x = Random.Range(xRange.x, xRange.y);
            var y = Random.Range(yRange.x, yRange.y);
            var z = Random.Range(zRange.x, zRange.y);
            return new Vector3(x, y, z);
        }

        private void ApplyRandomProperties(Transform t)
        {
            t.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            t.eulerAngles = new Vector3(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            );
        }
    }
}