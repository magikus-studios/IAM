using System;
using UnityEngine;
using UnityEngine.Events;

namespace IAM
{
    [CreateAssetMenu(fileName ="Event Asset", menuName = "IAM/Event Asset")]
    public class EventControllerAsset : ScriptableObject
    {
        private Action _action;

        public void Play() { _action?.Invoke(); }
        public void Subscribe(UnityEvent unityEvent) { _action += ()=> { unityEvent?.Invoke(); }; }
        public void Unsubscribe(UnityEvent unityEvent) { _action -= ()=> { unityEvent?.Invoke(); }; }
    }
}