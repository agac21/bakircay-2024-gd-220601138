using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.SkillSystem
{
    public class AutoMatchSkill : Skill
    {
        private ObjectTracker _tracker;
        private GameEventHub _eventHub;

        public AutoMatchSkill(ObjectTracker tracker, GameEventHub eventHub, int duration) : base(duration)
        {
            _eventHub = eventHub;
            _tracker = tracker;
        }

        protected override void ApplyInner()
        {
            var area = _tracker.GetPlacementArea();


            var addedObj = area.GetAddedObject();
            if (addedObj == null)
            {
                var seq = DOTween.Sequence();
                var os = _tracker.GetList();
                for (var i = 0; i < os.Count; i++)
                {
                    var o = os[i];
                    if (i == 0)
                    {
                        seq.Join(area.OnAdded(o));
                    }
                    else
                    {
                        var other = os[0];

                        if (o.ObjectId == other.ObjectId)
                        {
                            seq.Join(area.OnAdded(o));
                            _eventHub.Remove(other);
                            _eventHub.Remove(o);
                            o.GetTransform().GetComponent<Rigidbody>().isKinematic = true;
                            other.GetTransform().GetComponent<Rigidbody>().isKinematic = true;
                        }
                    }
                }

                seq.Append(area.OnMatched());
                _eventHub.Matched();
            }
            else
            {
                var pairObj = _tracker.GetList().FirstOrDefault(o => o.ObjectId == addedObj.ObjectId && addedObj != o);
                if (pairObj != null)
                {
                    _eventHub.Remove(addedObj);
                    _eventHub.Remove(pairObj);
                    var seq = DOTween.Sequence();
                    seq.Join(area.OnAdded(pairObj));
                    seq.Append(area.OnMatched());

                    pairObj.GetTransform().GetComponent<Rigidbody>().isKinematic = true;
                    _eventHub.Matched();
                }
            }

            base.ApplyInner();
        }
    }
}