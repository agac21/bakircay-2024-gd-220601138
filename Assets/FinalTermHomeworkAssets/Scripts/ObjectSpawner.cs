using System;
using System.Collections.Generic;
using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FinalTermHomeworkAssets.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 xRange;
        [SerializeField] private Vector2 yRange;
        [SerializeField] private Vector2 zRange;
        [SerializeField] private List<Color> m_colors;

        [SerializeField] private List<InteractiveObject> objectPrefabs;
        private GameEventHub _eventHub;

        public void SetDependencies(GameEventHub eventHub)
        {
            _eventHub = eventHub;
        }

        public void GenerateObjects()
        {
            var copyColorList = new List<Color>(m_colors);
            for (var i = 0; i < Random.Range(6, 9); i++)
            {
                var id = Guid.NewGuid();
                var color = copyColorList[Random.Range(0, copyColorList.Count)];
                copyColorList.Remove(color);

                var objPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Count)];
                for (var j = 0; j < 2; j++)
                {
                    var obj = spawn(objPrefab);
                    obj.SetOriginalColor(color);
                    obj.ObjectId = id.ToString();
                }
            }


            InteractiveObject spawn(InteractiveObject chosenPrefab)
            {
                var spawnPosition = GetRandomPosition();
                var spawnedInstance = Instantiate(chosenPrefab, spawnPosition, Quaternion.identity);

                ApplyRandomProperties(spawnedInstance.transform);

                _eventHub.Add(spawnedInstance);
                return spawnedInstance;
            }
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
            t.eulerAngles = new Vector3(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            );
        }
    }
}