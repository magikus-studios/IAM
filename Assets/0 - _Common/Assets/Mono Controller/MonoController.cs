using UnityEngine;
using UnityEngine.Events;

namespace IAM
{
    public class MonoController : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAwake;
        [SerializeField] private UnityEvent _onStart;

        private void Awake() { _onAwake?.Invoke(); }
        private void Start() { _onStart?.Invoke(); }
    }
}