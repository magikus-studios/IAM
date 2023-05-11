using UnityEngine;
using UnityEngine.Events;

namespace IAM
{
    public class EventController : MonoBehaviour
    {
        [SerializeField] private EventControllerAsset _EventAsset;
        [SerializeField] private UnityEvent Event;
        public void Play() { Event?.Invoke(); }

        private void OnEnable() { if (_EventAsset != null) { _EventAsset.Subscribe(Event); } }
        private void OnDisable() { if (_EventAsset != null) { _EventAsset.Unsubscribe(Event); } }
    }
}