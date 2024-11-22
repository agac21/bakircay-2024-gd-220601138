using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public class PlacementArea : MonoBehaviour
    {
        [SerializeField] private float m_radius;
        public bool Contains(Transform t)
        {
            var otherPos= t.position;
            var pos = transform.position;
            otherPos.y = pos.y;
            return Vector3.Distance(pos, otherPos) < m_radius;
        }
    }
}