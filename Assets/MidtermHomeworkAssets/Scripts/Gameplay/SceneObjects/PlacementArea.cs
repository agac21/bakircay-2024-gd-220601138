using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public class PlacementArea : MonoBehaviour
    {
        [SerializeField] private float m_radius;

        [SerializeField] private Transform m_leftSlotTransform, m_rightSlotTransform, m_middleTransform;

        private IInteractableObject m_leftSlot;
        private IInteractableObject m_rightSlot;


        public bool Contains(Transform t)
        {
            var otherPos = t.position;
            var pos = transform.position;
            otherPos.y = pos.y;
            return Vector3.Distance(pos, otherPos) < m_radius;
        }


        public IInteractableObject GetAddedObject()
        {
            return m_leftSlot;
        }


        public Tween OnAdded(IInteractableObject obj)
        {
            if (m_leftSlot == null)
            {
                m_leftSlot = obj;
                var t = m_leftSlot.GetTransform();
                var targetTransform = m_leftSlotTransform;
                return animate(obj, targetTransform);
            }

            if (m_rightSlot == null)
            {
                m_rightSlot = obj;
                var t = m_rightSlot.GetTransform();
                var targetTransform = m_rightSlotTransform;
                return animate(obj, targetTransform);
            }

            Tween animate(IInteractableObject interactive, Transform target)
            {
                var t = interactive.GetTransform();
                var seq = DOTween.Sequence();
                seq.Join(t.DOMove(target.position, .3f).SetEase(Ease.OutQuint));
                seq.Join(t.DORotate(target.eulerAngles, .3f).SetEase(Ease.OutQuint));
                seq.Join(t.DOScale(target.localScale, .3f).SetEase(Ease.OutQuint));
                return seq;
            }

            return null;
        }

        public void Remove(IInteractableObject obj)
        {
            if (m_leftSlot == obj)
            {
                m_leftSlot = null;
            }
            else if (m_rightSlot == obj)
            {
                m_rightSlot = null;
            }
        }

        public void Clean()
        {
            m_leftSlot = null;
            m_rightSlot = null;
        }

        public Tween OnMatched()
        {
            var seq = DOTween.Sequence();

            var leftSlot = m_leftSlot;
            seq.Join(leftSlot.GetTransform().DOScale(Vector3.zero, .5f).SetEase(Ease.InBack));
            seq.Join(leftSlot.GetTransform().DOMove(m_middleTransform.position, .5f).SetEase(Ease.InBack));

            var rightSlot = m_rightSlot;
            seq.Join(rightSlot.GetTransform().DOScale(Vector3.zero, .5f).SetEase(Ease.InBack));

            seq.Join(rightSlot.GetTransform().DOMove(m_middleTransform.position, .5f).SetEase(Ease.InBack));
            seq.AppendCallback(() =>
            {
                Destroy(rightSlot.GetTransform().gameObject);
                Destroy(leftSlot.GetTransform().gameObject);
            });

            Clean();
            return seq;
        }

        public void OnThrowAway(IInteractableObject obj)
        {
            if (m_leftSlot == obj)
            {
                m_leftSlot = null;
            }
            else if (m_rightSlot == obj)
            {
                m_rightSlot = null;
            }

            var rb = obj.GetTransform().GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(0, Random.Range(2, 8), Random.Range(5, 15)), ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0,5)), ForceMode.Impulse);
        }
    }
}